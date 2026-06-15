using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace EODHD.CSharpApiClient
{
    /// <summary>
    /// Client-side leaky-bucket rate limiter. Hands out up to a fixed number of request permits per
    /// minute, refilling continuously so bursts are smoothed without exceeding the configured rate.
    /// Callers await <see cref="GateRequestAsync"/> before each request.
    /// <para>
    /// Base algorithm adapted from the IQFeed.CSharpApiClient leaky-bucket rate limiter
    /// (https://github.com/mathpaquette/IQFeed.CSharpApiClient).
    /// </para>
    /// </summary>
    public sealed class RequestRateLimiter : IDisposable
    {
        private readonly SemaphoreSlim requestGateSemaphore;
        private readonly Task leakyBucketRefillTask;

        private bool disposed;
        private volatile bool started;
        private volatile bool running = true;

        /// <summary>
        /// Initialises a new rate limiter.
        /// </summary>
        /// <param name="requestsPerMinute">Maximum number of requests permitted per minute. Must be positive.</param>
        /// <param name="availableRequests">
        /// Initial number of permits available before refilling begins. Defaults (-1) to
        /// <paramref name="requestsPerMinute"/> — a full bucket.
        /// </param>
        public RequestRateLimiter(int requestsPerMinute, int availableRequests = -1)
        {
            if(requestsPerMinute <= 0)
            {
                throw new ArgumentException("Value must be positive.", nameof(requestsPerMinute));
            }

            if(availableRequests < 0)
            {
                availableRequests = requestsPerMinute;
            }

            this.IntervalPerRequest = TimeSpan.FromTicks(TimeSpan.FromMinutes(1).Ticks / requestsPerMinute);
            this.RequestsPerMinute = requestsPerMinute;
            this.requestGateSemaphore = new SemaphoreSlim(availableRequests, requestsPerMinute);
            this.leakyBucketRefillTask = this.StartLeakyBucketRefill(this.IntervalPerRequest, requestsPerMinute);
        }

        /// <summary>
        /// Gets the maximum number of requests permitted per minute.
        /// </summary>
        public int RequestsPerMinute { get; }

        /// <summary>
        /// Gets the interval between successive permit refills.
        /// </summary>
        public TimeSpan IntervalPerRequest { get; }

        /// <summary>
        /// Gets the number of request permits currently available.
        /// </summary>
        public int AvailableRequests => this.requestGateSemaphore.CurrentCount;

        /// <summary>
        /// Gets a value indicating whether the background refill loop is still running.
        /// </summary>
        public bool IsRunning => !this.leakyBucketRefillTask.IsCompleted;

        /// <summary>
        /// Awaits a request permit, blocking asynchronously until one is available.
        /// </summary>
        /// <returns>A task that completes once a permit has been acquired.</returns>
        public Task GateRequestAsync()
        {
            this.started = true;
            return this.requestGateSemaphore.WaitAsync();
        }

        /// <summary>
        /// Stops the refill loop and releases resources.
        /// </summary>
        public void Dispose()
        {
            if(this.disposed)
            {
                return;
            }

            this.running = false;
            this.leakyBucketRefillTask.Wait();
            this.requestGateSemaphore.Dispose();
            this.disposed = true;
        }

        private async Task StartLeakyBucketRefill(TimeSpan interval, int maxCount)
        {
            // Start only after the first request goes through.
            while(!this.started && this.running)
            {
                await Task.Delay(TimeSpan.FromMilliseconds(10)).ConfigureAwait(false);
            }

            if(this.running)
            {
                // Wait one full minute before refilling so the initial capacity is served gracefully
                // without risking going over the limit.
                await Task.Delay(TimeSpan.FromMinutes(1)).ConfigureAwait(false);
            }

            int intervalTicks = (int)interval.Ticks;
            int remainderTicks = 0;
            Stopwatch stopwatch = new Stopwatch();

            while(this.running)
            {
                stopwatch.Restart();
                await Task.Delay(interval).ConfigureAwait(false);
                stopwatch.Stop();

                int totalTicks = remainderTicks + (int)stopwatch.ElapsedTicks;
                remainderTicks = totalTicks % intervalTicks;
                int releaseCount = totalTicks / intervalTicks;
                int releaseCapacity = maxCount - this.requestGateSemaphore.CurrentCount;

                if(releaseCapacity == 0 || releaseCount == 0)
                {
                    continue;
                }

                releaseCount = releaseCount > releaseCapacity ? releaseCapacity : releaseCount;
                this.requestGateSemaphore.Release(releaseCount);
            }
        }
    }
}
