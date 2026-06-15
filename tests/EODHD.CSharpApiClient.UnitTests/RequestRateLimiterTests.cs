using System;

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
    }
}
