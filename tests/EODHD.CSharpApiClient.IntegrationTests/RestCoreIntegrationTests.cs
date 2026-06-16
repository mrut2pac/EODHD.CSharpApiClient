using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using EODHD.CSharpApiClient.DataModel;
using EODHD.CSharpApiClient.DataModel.UpcomingIpos;
using EODHD.CSharpApiClient.Exceptions;

using Xunit;

namespace EODHD.CSharpApiClient.IntegrationTests
{
    public class RestCoreIntegrationTests : IntegrationTestBase
    {
        [SkippableFact]
        public async Task GetIntraday_Apple_ReturnsBars()
        {
            this.SkipIfNoApiKey();
            using EodhdClient client = this.CreateClient();

            List<IntradayHistoricalStockPrice> bars;
            try
            {
                bars = await client.GetIntradayHistoricalStockPricesAsync("AAPL.US", IntradayInterval.FiveMinutes);
            }
            catch(EodhdHttpException ex)
            {
                SkipIfNoLicense(ex);
                throw;
            }

            Assert.NotNull(bars);
            Skip.If(bars.Count == 0, "No intraday bars returned for the default window.");
            Assert.NotNull(bars[0].DateTime);
        }

        [SkippableFact]
        public async Task GetHistoricalDividends_Apple_ReturnsDividends()
        {
            this.SkipIfNoApiKey();
            using EodhdClient client = this.CreateClient();

            HistoricalDividend[] dividends;
            try
            {
                dividends = await client.GetHistoricalDividendsAsync("AAPL.US", new DateTime(2015, 1, 1), new DateTime(2024, 1, 1));
            }
            catch(EodhdHttpException ex)
            {
                SkipIfNoLicense(ex);
                throw;
            }

            Assert.NotNull(dividends);
            Assert.True(dividends.Length > 0);
            Assert.True(dividends[0].Value > 0);
        }

        [SkippableFact]
        public async Task GetLivePrice_Apple_ReturnsQuote()
        {
            this.SkipIfNoApiKey();
            using EodhdClient client = this.CreateClient();

            LiveStockPrice quote = await client.GetLivePriceAsync("AAPL.US");

            Assert.NotNull(quote);
            Assert.Equal("AAPL.US", quote.Code);
        }

        [SkippableFact]
        public async Task GetLivePrices_Multiple_ReturnsQuotePerSymbol()
        {
            this.SkipIfNoApiKey();
            using EodhdClient client = this.CreateClient();

            LiveStockPrice[] quotes = await client.GetLivePricesAsync("AAPL.US", new[] { "MSFT.US" });

            Assert.NotNull(quotes);
            Assert.True(quotes.Length >= 2);
        }

        [SkippableFact]
        public async Task Search_Apple_ReturnsResults()
        {
            this.SkipIfNoApiKey();
            using EodhdClient client = this.CreateClient();

            SearchResult[] results;
            try
            {
                results = await client.SearchAsync("apple", limit: 10);
            }
            catch(EodhdHttpException ex)
            {
                SkipIfNoLicense(ex);
                throw;
            }

            Assert.NotNull(results);
            Assert.True(results.Length > 0);
            Assert.Contains(results, r => r.Code == "AAPL");
        }

        [SkippableFact]
        public async Task GetExchangesList_ReturnsKnownExchanges()
        {
            this.SkipIfNoApiKey();
            using EodhdClient client = this.CreateClient();

            List<Exchange> exchanges = await client.GetExchangesListAsync();

            Assert.NotNull(exchanges);
            Assert.Contains(exchanges, e => e.Code == "US");
        }

        [SkippableFact]
        public async Task GetUpcomingIpos_KnownRange_ReturnsEntries()
        {
            this.SkipIfNoApiKey();
            using EodhdClient client = this.CreateClient();

            UpcomingIpos ipos;
            try
            {
                ipos = await client.GetUpcomingIposAsync(new DateTime(2024, 1, 1), new DateTime(2024, 3, 31));
            }
            catch(EodhdHttpException ex)
            {
                SkipIfNoLicense(ex);
                throw;
            }

            Assert.NotNull(ipos);
            Assert.NotNull(ipos.Ipos);
            Skip.If(ipos.Ipos.Count == 0, "No IPOs returned for the requested range.");
        }
    }
}
