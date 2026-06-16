using System;
using System.Threading.Tasks;

using EODHD.CSharpApiClient.DataModel.Ticks;
using EODHD.CSharpApiClient.Exceptions;

using Xunit;

namespace EODHD.CSharpApiClient.IntegrationTests
{
    public class TicksIntegrationTests : IntegrationTestBase
    {
        [SkippableFact]
        public async Task GetTicks_Apple_ReturnsTrades()
        {
            this.SkipIfNoApiKey();
            using EodhdClient client = this.CreateClient();

            Tick[] ticks;
            try
            {
                ticks = await client.GetTicksAsync(
                    "AAPL.US",
                    from: new DateTime(2023, 11, 14, 22, 13, 20, DateTimeKind.Utc),
                    to: new DateTime(2023, 11, 14, 22, 23, 20, DateTimeKind.Utc),
                    limit: 100);
            }
            catch(EodhdHttpException ex)
            {
                SkipIfNoLicense(ex);
                throw;
            }

            Assert.NotNull(ticks);
            Skip.If(ticks.Length == 0, "No ticks returned for the window.");
            Assert.All(ticks, t => Assert.True(t.Timestamp.HasValue));
            Assert.Contains(ticks, t => t.Price.HasValue);
        }
    }
}
