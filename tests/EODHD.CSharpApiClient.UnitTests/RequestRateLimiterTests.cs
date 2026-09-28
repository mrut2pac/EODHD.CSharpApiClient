using System;
using System.Diagnostics;

using Xunit;

namespace EODHD.CSharpApiClient.UnitTests
{
    public class RequestRateLimiterTests
    {
        [Fact]
        public void Constructor_RequestsPerMinute_IsExposed()
        {
            using RequestRateLimiter limiter = new RequestRateLimiter(1200);
            Assert.Equal(1200, limiter.RequestsPerMinute);
        }

        [Fact]
        public void Constructor_DefaultAvailableRequests_EqualsRequestsPerMinute()
        {
            using RequestRateLimiter limiter = new RequestRateLimiter(1200);
            Assert.Equal(1200, limiter.AvailableRequests);
        }

        [Fact]
        public void Constructor_ExplicitAvailableRequests_IsHonoured()
        {
            using RequestRateLimiter limiter = new RequestRateLimiter(1200, 1000);
            Assert.Equal(1000, limiter.AvailableRequests);
        }

        [Fact]
        public void Constructor_IntervalPerRequest_IsOneMinuteDividedByRate()
        {
            using RequestRateLimiter limiter = new RequestRateLimiter(600);
            Assert.Equal(TimeSpan.FromMilliseconds(60000 / 600), limiter.IntervalPerRequest);
        }

        [Fact]
        public void Constructor_NonPositiveRate_Throws()
        {
            Assert.Throws<ArgumentException>(() => new RequestRateLimiter(0));
        }

        [Fact]
        public async System.Threading.Tasks.Task GateRequest_ConsumesAvailablePermits()
        {
            using RequestRateLimiter limiter = new RequestRateLimiter(60);

            await limiter.GateRequestAsync();
            await limiter.GateRequestAsync();

            Assert.Equal(58, limiter.AvailableRequests);
        }

        [Fact]
        public void Dispose_StopsTheRefillLoop()
        {
            RequestRateLimiter limiter = new RequestRateLimiter(50);
            limiter.Dispose();
            Assert.False(limiter.IsRunning);
        }

        [Fact]
        public void CalculateReleaseCount_OneIntervalElapsed_ReleasesOne()
        {
            TimeSpan interval = TimeSpan.FromMilliseconds(100);

            int releaseCount = RequestRateLimiter.CalculateReleaseCount(interval, interval, TimeSpan.Zero, out TimeSpan carriedOver);

            Assert.Equal(1, releaseCount);
            Assert.Equal(TimeSpan.Zero, carriedOver);
        }

        [Fact]
        public void CalculateReleaseCount_PartialInterval_CarriesTheRemainderIntoTheNextPass()
        {
            TimeSpan interval = TimeSpan.FromMilliseconds(100);

            int first = RequestRateLimiter.CalculateReleaseCount(TimeSpan.FromMilliseconds(250), interval, TimeSpan.Zero, out TimeSpan carriedOver);
            Assert.Equal(2, first);
            Assert.Equal(TimeSpan.FromMilliseconds(50), carriedOver);

            int second = RequestRateLimiter.CalculateReleaseCount(TimeSpan.FromMilliseconds(60), interval, carriedOver, out carriedOver);
            Assert.Equal(1, second);
            Assert.Equal(TimeSpan.FromMilliseconds(10), carriedOver);
        }

        [Fact]
        public void CalculateReleaseCount_LessThanAnInterval_ReleasesNothingAndKeepsTheTime()
        {
            TimeSpan interval = TimeSpan.FromMilliseconds(100);

            int releaseCount = RequestRateLimiter.CalculateReleaseCount(TimeSpan.FromMilliseconds(40), interval, TimeSpan.Zero, out TimeSpan carriedOver);

            Assert.Equal(0, releaseCount);
            Assert.Equal(TimeSpan.FromMilliseconds(40), carriedOver);
        }

        [Fact]
        public void CalculateReleaseCount_LongIntervalAtLowRate_DoesNotOverflow()
        {
            // 1 request/minute: five minutes is 3e9 ticks, past Int32.MaxValue
            TimeSpan interval = TimeSpan.FromMinutes(1);

            int releaseCount = RequestRateLimiter.CalculateReleaseCount(TimeSpan.FromMinutes(5), interval, TimeSpan.Zero, out TimeSpan carriedOver);

            Assert.Equal(5, releaseCount);
            Assert.Equal(TimeSpan.Zero, carriedOver);
        }

        [Fact]
        public async System.Threading.Tasks.Task Dispose_AfterFirstRequest_DoesNotWaitOutTheRefillDelay()
        {
            RequestRateLimiter limiter = new RequestRateLimiter(60);
            await limiter.GateRequestAsync();

            // let the refill loop see the first request and enter its one-minute warm-up delay
            await System.Threading.Tasks.Task.Delay(TimeSpan.FromMilliseconds(200));

            Stopwatch stopwatch = Stopwatch.StartNew();
            limiter.Dispose();
            stopwatch.Stop();

            Assert.False(limiter.IsRunning);
            Assert.True(stopwatch.Elapsed < TimeSpan.FromSeconds(5), $"Dispose took {stopwatch.Elapsed}.");
        }
    }
}
