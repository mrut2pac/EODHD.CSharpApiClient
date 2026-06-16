using System.Collections.Generic;
using System.Threading.Tasks;

using EODHD.CSharpApiClient.DataModel.Commodities;
using EODHD.CSharpApiClient.DataModel.IdMappings;
using EODHD.CSharpApiClient.DataModel.Quotes;
using EODHD.CSharpApiClient.Exceptions;

using Xunit;

namespace EODHD.CSharpApiClient.IntegrationTests
{
    public class CommoditiesIdMappingQuoteIntegrationTests : IntegrationTestBase
    {
        [SkippableFact]
        public async Task GetCommodityHistoricalPrices_Brent_ReturnsSeries()
        {
            this.SkipIfNoApiKey();
            using EodhdClient client = this.CreateClient();

            CommodityPrice[] prices;
            try
            {
                prices = await client.GetCommodityHistoricalPricesAsync("BRENT");
            }
            catch (EodhdHttpException ex)
            {
                SkipIfNoLicense(ex);
                throw;
            }

            Assert.NotNull(prices);
            Skip.If(prices.Length == 0, "No commodity prices returned.");
            Assert.Contains(prices, p => p.Date.HasValue && p.Value.HasValue);
        }

        [SkippableFact]
        public async Task GetIdMapping_AppleSymbol_ReturnsIdentifiers()
        {
            this.SkipIfNoApiKey();
            using EodhdClient client = this.CreateClient();

            IdMapping[] mappings;
            try
            {
                mappings = await client.GetIdMappingAsync(symbol: "AAPL.US");
            }
            catch (EodhdHttpException ex)
            {
                SkipIfNoLicense(ex);
                throw;
            }

            Assert.NotNull(mappings);
            Skip.If(mappings.Length == 0, "No identifier mappings returned.");
            Assert.Equal("US0378331005", mappings[0].Isin);
        }

        [SkippableFact]
        public async Task GetUsDelayedQuotes_Apple_ReturnsQuote()
        {
            this.SkipIfNoApiKey();
            using EodhdClient client = this.CreateClient();

            Dictionary<string, UsDelayedQuote> quotes;
            try
            {
                quotes = await client.GetUsDelayedQuotesAsync(new[] { "AAPL" });
            }
            catch (EodhdHttpException ex)
            {
                SkipIfNoLicense(ex);
                throw;
            }

            Assert.NotNull(quotes);
            Skip.If(quotes.Count == 0, "No quotes returned.");
            Assert.True(quotes.ContainsKey("AAPL.US"));
            Assert.Equal("AAPL.US", quotes["AAPL.US"].Symbol);
        }

        // Marketplace endpoint: verified live via EODHD's public demo token (not gated on EODHD_API_KEY).
        [SkippableFact]
        public async Task GetOptionUnderlyingSymbols_Demo_ReturnsSymbols()
        {
            using EodhdClient client = CreateDemoClient();

            string[] symbols;
            try
            {
                symbols = await client.GetOptionUnderlyingSymbolsAsync(limit: 10);
            }
            catch (EodhdHttpException ex)
            {
                SkipIfNoLicense(ex);
                throw;
            }

            Assert.NotNull(symbols);
            Skip.If(symbols.Length == 0, "No underlying symbols returned.");
            Assert.All(symbols, s => Assert.False(string.IsNullOrEmpty(s)));
        }
    }
}
