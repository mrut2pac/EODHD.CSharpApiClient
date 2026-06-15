using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using EODHD.CSharpApiClient.DataModel;
using EODHD.CSharpApiClient.DataModel.BulkFundamental;
using EODHD.CSharpApiClient.DataModel.ExchangeInfo;
using EODHD.CSharpApiClient.DataModel.Fundamental;
using EODHD.CSharpApiClient.DataModel.UpcomingEarnings;
using EODHD.CSharpApiClient.DataModel.UpcomingSplits;
using EODHD.CSharpApiClient.Exceptions;

using Xunit;

namespace EODHD.CSharpApiClient.IntegrationTests
{
    public class EodhdClientIntegrationTests : IntegrationTestBase
    {
        private static readonly string[] AppleOnly = new string[] { "AAPL.US" };
        private static readonly string[] BulkSymbols = new string[] { "AAPL.US", "MSFT.US", "SPY.US", "LEH.US", "TTD.US" };

        [SkippableFact]
        public async Task GetExchangeDetails_Us_ReturnsHolidays()
        {
            this.SkipIfNoApiKey();
            using EodhdClient client = this.CreateClient();

            int year = DateTime.UtcNow.Year;
            ExchangeDetails details = await client.GetExchangeDetailsAsync("US", new DateTime(year - 1, 1, 1), new DateTime(year + 1, 1, 1));

            Assert.NotNull(details);
            Assert.True(details.ExchangeHolidays.Count > 0);
        }

        [SkippableFact]
        public async Task GetSymbolChangeHistory_KnownRange_ContainsFbToMeta()
        {
            this.SkipIfNoApiKey();
            using EodhdClient client = this.CreateClient();

            SymbolChange[] changes = await client.GetSymbolChangeHistoryAsync(new DateTime(2022, 1, 1), new DateTime(DateTime.UtcNow.Year + 1, 1, 1));

            Assert.NotNull(changes);
            Assert.Equal(1, changes.Count(c => c.OldSymbol == "FB" && c.NewSymbol == "META"));
        }

        [SkippableFact]
        public async Task GetExchangeSymbols_Active_ContainsAaplNotLeh()
        {
            this.SkipIfNoApiKey();
            using EodhdClient client = this.CreateClient();

            List<ExchangeSymbol> symbols = await client.GetExchangeSymbolsAsync("US", delisted: false);

            Assert.Equal(1, symbols.Count(s => s.Code == "AAPL"));
            Assert.Equal(0, symbols.Count(s => s.Code == "LEH"));
        }

        [SkippableFact]
        public async Task GetExchangeSymbols_Delisted_ContainsLehNotAapl()
        {
            this.SkipIfNoApiKey();
            using EodhdClient client = this.CreateClient();

            List<ExchangeSymbol> symbols = await client.GetExchangeSymbolsAsync("US", delisted: true);

            Assert.Equal(0, symbols.Count(s => s.Code == "AAPL"));
            Assert.Equal(1, symbols.Count(s => s.Code == "LEH"));
        }

        [SkippableFact]
        public async Task GetUpcomingEarnings_All_ContainsApple()
        {
            this.SkipIfNoApiKey();
            using EodhdClient client = this.CreateClient();

            DateTime now = DateTime.UtcNow;
            UpcomingEarnings earnings = await client.GetUpcomingEarningsAsync(now, now.AddDays(100));

            Assert.NotNull(earnings);
            Assert.True(earnings.Earnings.Count > 1);
        }

        [SkippableFact]
        public async Task GetUpcomingEarnings_AppleOnly_ReturnsOnlyApple()
        {
            this.SkipIfNoApiKey();
            using EodhdClient client = this.CreateClient();

            DateTime now = DateTime.UtcNow;
            UpcomingEarnings earnings = await client.GetUpcomingEarningsAsync(now, now.AddDays(100), AppleOnly);

            Assert.NotNull(earnings);
            Assert.All(earnings.Earnings, e => Assert.Equal("AAPL.US", e.Code));
        }

        [SkippableFact]
        public async Task GetUpcomingSplits_2020_ContainsTeslaFiveForOne()
        {
            this.SkipIfNoApiKey();
            using EodhdClient client = this.CreateClient();

            UpcomingSplits splits = await client.GetUpcomingSplitsAsync(new DateTime(2020, 1, 1), new DateTime(2020, 12, 31));

            Assert.NotNull(splits);
            Split tesla = splits.Splits.Single(s => s.Code == "TSLA.US");
            Assert.Equal("TSLA", tesla.Symbol);
            Assert.Equal("US", tesla.Exchange);
            Assert.Equal(1.0 / 5.0, tesla.SplitFactor);
        }

        [SkippableFact]
        public async Task GetEndOfDay_ActiveSymbol_ReturnsBars()
        {
            this.SkipIfNoApiKey();
            using EodhdClient client = this.CreateClient();

            DateTime now = DateTime.UtcNow;
            List<HistoricalStockPrice> bars = await client.GetEndOfDayHistoricalStockPricesAsync(
                "AAPL.US", now.AddDays(-365), now, EndOfDayPeriod.Daily);

            Assert.NotNull(bars);
            Assert.True(bars.Count > 0);
        }

        [SkippableFact]
        public async Task GetEndOfDay_DelistedSymbol_ReturnsBars()
        {
            this.SkipIfNoApiKey();
            using EodhdClient client = this.CreateClient();

            List<HistoricalStockPrice> bars = await client.GetEndOfDayHistoricalStockPricesAsync(
                "LEH.US", new DateTime(1995, 1, 1), new DateTime(2010, 1, 1), EndOfDayPeriod.Daily);

            Assert.NotNull(bars);
            Assert.True(bars.Count > 0);
        }

        [SkippableFact]
        public async Task GetFundamentals_ActiveSymbol_ReturnsGeneralAndHighlights()
        {
            this.SkipIfNoApiKey();
            using EodhdClient client = this.CreateClient();

            FundamentalData data;
            try
            {
                data = await client.GetFundamentalsDataAsync("AAPL.US");
            }
            catch (EodhdHttpException ex)
            {
                SkipIfNoLicense(ex);
                throw;
            }

            Assert.NotNull(data);
            Assert.NotNull(data.General);
            Assert.Equal("AAPL", data.General.Code);
            Assert.False(data.General.IsDelisted.GetValueOrDefault());
            Assert.NotNull(data.Highlights);
        }

        [SkippableFact]
        public async Task GetFundamentals_DelistedSymbol_IsMarkedDelisted()
        {
            this.SkipIfNoApiKey();
            using EodhdClient client = this.CreateClient();

            FundamentalData data;
            try
            {
                data = await client.GetFundamentalsDataAsync("LEH.US");
            }
            catch (EodhdHttpException ex)
            {
                SkipIfNoLicense(ex);
                throw;
            }

            Assert.NotNull(data);
            Assert.Equal("LEH", data.General.Code);
            Assert.True(data.General.IsDelisted.GetValueOrDefault());
        }

        [SkippableFact]
        public async Task GetBulkFundamentals_Dictionary_ReturnsRequestedSymbols()
        {
            this.SkipIfNoApiKey();
            using EodhdClient client = this.CreateClient();

            Dictionary<string, BulkFundamentalData> data;
            try
            {
                data = await client.GetBulkFundamentalsDataAsync("US", symbols: BulkSymbols);
            }
            catch (EodhdHttpException ex)
            {
                SkipIfNoLicense(ex);
                throw;
            }

            Assert.NotNull(data);
            Assert.Equal(BulkSymbols.Length, data.Count);
        }

        [SkippableFact]
        public async Task GetBulkFundamentalsExtended_List_ReturnsRequestedSymbols()
        {
            this.SkipIfNoApiKey();
            using EodhdClient client = this.CreateClient();

            List<BulkFundamentalData> data;
            try
            {
                data = await client.GetBulkFundamentalsExtendedDataAsync("US", symbols: BulkSymbols);
            }
            catch (EodhdHttpException ex)
            {
                SkipIfNoLicense(ex);
                throw;
            }

            Assert.NotNull(data);
            Assert.Equal(BulkSymbols.Length, data.Count);
        }

        [SkippableFact]
        public async Task GetHistoricalSplits_Msft_ReturnsPositiveFactors()
        {
            this.SkipIfNoApiKey();
            using EodhdClient client = this.CreateClient();

            HistoricalSplit[] splits = await client.GetHistoricalSplitsAsync("MSFT.US");

            Assert.NotNull(splits);
            Assert.True(splits.Length > 0);
            Assert.True(splits[0].Factor > 0);
        }

        [SkippableFact]
        public async Task GetBulkEndOfDay_MostRecentDay_ContainsApple()
        {
            this.SkipIfNoApiKey();
            using EodhdClient client = this.CreateClient();

            BulkHistoricalStockPrice[] prices = await client.GetBulkEndOfDayHistoricalStockPricesAsync();

            Assert.NotNull(prices);
            Assert.True(prices.Length > 0);
            Assert.Equal(1, prices.Count(p => p.Symbol == "AAPL"));
        }

        [SkippableFact]
        public async Task GetBulkHistoricalSplits_TeslaSplitDay_ContainsTesla()
        {
            this.SkipIfNoApiKey();
            using EodhdClient client = this.CreateClient();

            BulkHistoricalSplit[] splits = await client.GetBulkHistoricalSplitsAsync(new DateTime(2022, 8, 25));

            Assert.NotNull(splits);
            BulkHistoricalSplit tesla = splits.Single(s => s.Symbol == "TSLA");
            Assert.Equal(1.0 / 3.0, tesla.Factor);
        }
    }
}
