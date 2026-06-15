using System;
using System.Threading.Tasks;

using EODHD.CSharpApiClient.DataModel.Macro;
using EODHD.CSharpApiClient.DataModel.MarketCap;
using EODHD.CSharpApiClient.DataModel.Screener;
using EODHD.CSharpApiClient.Exceptions;

using Xunit;

namespace EODHD.CSharpApiClient.IntegrationTests
{
    public class ScreenerMacroMarketCapIntegrationTests : IntegrationTestBase
    {
        [SkippableFact]
        public async Task GetScreener_LargeCapTechnology_ReturnsMatches()
        {
            this.SkipIfNoApiKey();
            using EodhdClient client = this.CreateClient();

            ScreenerResult[] results;
            try
            {
                results = await client.GetScreenerAsync(
                    new[] { new ScreenerFilter("market_capitalization", ">", 1000000000000L) },
                    sort: "market_capitalization.desc",
                    limit: 5);
            }
            catch (EodhdHttpException ex)
            {
                SkipIfNoLicense(ex);
                throw;
            }

            Assert.NotNull(results);
            Skip.If(results.Length == 0, "Screener returned no matches for the requested filter.");
            Assert.True(results[0].MarketCapitalization > 1000000000000m);
        }

        [SkippableFact]
        public async Task GetMacroIndicator_UsaInflation_ReturnsSeries()
        {
            this.SkipIfNoApiKey();
            using EodhdClient client = this.CreateClient();

            MacroIndicatorValue[] values;
            try
            {
                values = await client.GetMacroIndicatorAsync("USA", MacroIndicator.InflationConsumerPricesAnnual);
            }
            catch (EodhdHttpException ex)
            {
                SkipIfNoLicense(ex);
                throw;
            }

            Assert.NotNull(values);
            Assert.True(values.Length > 0);
            Assert.Equal("USA", values[0].CountryCode);
        }

        [SkippableFact]
        public async Task GetMacroIndicator_DefaultIndicator_ReturnsGdp()
        {
            this.SkipIfNoApiKey();
            using EodhdClient client = this.CreateClient();

            MacroIndicatorValue[] values;
            try
            {
                values = await client.GetMacroIndicatorAsync("USA");
            }
            catch (EodhdHttpException ex)
            {
                SkipIfNoLicense(ex);
                throw;
            }

            Assert.NotNull(values);
            Assert.True(values.Length > 0);
        }

        [SkippableFact]
        public async Task GetHistoricalMarketCap_Apple_ReturnsWeeklySeries()
        {
            this.SkipIfNoApiKey();
            using EodhdClient client = this.CreateClient();

            HistoricalMarketCap[] caps;
            try
            {
                caps = await client.GetHistoricalMarketCapAsync("AAPL.US", new DateTime(2024, 1, 1), new DateTime(2024, 12, 31));
            }
            catch (EodhdHttpException ex)
            {
                SkipIfNoLicense(ex);
                throw;
            }

            Assert.NotNull(caps);
            Skip.If(caps.Length == 0, "No historical market-cap data returned for the requested range.");
            Assert.True(caps[0].Date <= caps[caps.Length - 1].Date);
            Assert.True(caps[0].Value > 0);
        }
    }
}
