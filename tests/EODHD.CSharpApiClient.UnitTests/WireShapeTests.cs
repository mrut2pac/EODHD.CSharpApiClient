using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

using EODHD.CSharpApiClient.DataModel;
using EODHD.CSharpApiClient.DataModel.BulkFundamental;
using EODHD.CSharpApiClient.DataModel.ExchangeInfo;
using EODHD.CSharpApiClient.DataModel.Fundamental;
using EODHD.CSharpApiClient.DataModel.UpcomingEarnings;

using Xunit;

namespace EODHD.CSharpApiClient.UnitTests
{
    /// <summary>
    /// Pins model shapes to live EODHD payloads (trimmed): fields the API sends as a bare number or as a
    /// quoted string depending on the symbol, <c>"NA"</c> sentinels, and wire keys that need an explicit
    /// <c>[JsonPropertyName]</c> to bind.
    /// </summary>
    public class WireShapeTests
    {
        private static EodhdClient CreateClient(string responseJson)
        {
            return CreateClient(responseJson, out FakeHttpTransport _);
        }

        private static EodhdClient CreateClient(string responseJson, out FakeHttpTransport transport)
        {
            transport = new FakeHttpTransport(HttpStatusCode.OK, responseJson);
            return new EodhdClient(new EodhdClientOptions { ApiToken = "test" }, transport);
        }

        [Theory]
        [InlineData("14459.0312")]
        [InlineData("\"14459.0312\"")]
        public async Task GetFundamentals_EtfAverageMarketCap_ReadsNumberOrString(string wireValue)
        {
            string json = "{\"ETF_Data\":{\"Average_Mkt_Cap_Mil\":" + wireValue + "}}";

            using EodhdClient client = CreateClient(json);

            FundamentalData data = await client.GetFundamentalsDataAsync("FFR_old.US");

            Assert.Equal(14459.0312m, data.ETFData.AverageMarketCapitalizationMillions);
        }

        [Fact]
        public async Task GetFundamentals_Etf_MixedMarketCapBucketsIndexNameAndSectors()
        {
            const string json =
                "{\"ETF_Data\":{\"Index_Name\":\"S&P 500 Index\"," +
                "\"Market_Capitalisation\":{\"Mega\":\"0\",\"Big\":24.3164,\"Medium\":38.5743,\"Small\":30.2246,\"Micro\":6.4142}," +
                "\"Sector_Weights\":{\"Real Estate\":{\"Equity_%\":\"99.1\",\"Relative_to_Category\":\"1.2\"}," +
                "\"Communication Services\":{\"Equity_%\":0.8,\"Relative_to_Category\":null}}}}";

            using EodhdClient client = CreateClient(json);

            FundamentalData data = await client.GetFundamentalsDataAsync("VNQ.US");

            ETFData etf = data.ETFData;
            Assert.Equal("S&P 500 Index", etf.IndexName);
            Assert.Equal(0d, etf.MarketCapitalization.Mega);
            Assert.Equal(24.3164, etf.MarketCapitalization.Big);
            Assert.Equal(6.4142, etf.MarketCapitalization.Micro);
            Assert.Equal(99.1, etf.SectorWeights.RealEstate.EquityPercent);
            Assert.Equal(0.8, etf.SectorWeights.CommunicationServices.EquityPercent);
        }

        [Fact]
        public async Task GetFundamentals_StringPropertySentAsNumberOrBoolean_KeepsWireText()
        {
            // CIK and ISIN are modeled as strings; a bare number/boolean must not fail the whole response.
            const string json = "{\"General\":{\"Code\":\"AAPL\",\"CIK\":320193,\"ISIN\":true,\"Phone\":1.50e2}}";

            using EodhdClient client = CreateClient(json);

            FundamentalData data = await client.GetFundamentalsDataAsync("AAPL.US");

            Assert.Equal("320193", data.General.CIK);
            Assert.Equal("true", data.General.ISIN);
            Assert.Equal("1.50e2", data.General.Phone);
        }

        [Fact]
        public async Task GetFundamentals_GeneralComponentsCashFlowAndStatistics_BindPreviouslyDroppedFields()
        {
            const string json =
                "{\"General\":{\"Exchange\":\"LSE\",\"OperatingMIC\":\"XLON\",\"ExchangeMarket\":\"LSE - MAIN MARKET\"," +
                "\"IsDelisted\":true,\"DelistedDate\":\"2022-10-28\",\"MarketCap\":65959661940996}," +
                "\"Components\":{\"0\":{\"Code\":\"NVDA\",\"Weight\":0.0789}}," +
                "\"Financials\":{\"Cash_Flow\":{\"yearly\":{\"2025-09-30\":{\"issuanceOfCapitalStock\":\"1105000000.00\"}}}}," +
                "\"Statistics\":{\"MaxSupply\":21000000,\"LowAllTime\":0.05,\"HighAllTime\":126000}}";

            using EodhdClient client = CreateClient(json);

            FundamentalData data = await client.GetFundamentalsDataAsync("VOD.LSE");

            Assert.Equal("XLON", data.General.OperatingMIC);
            Assert.Equal("LSE - MAIN MARKET", data.General.ExchangeMarket);
            Assert.Equal(new DateTime(2022, 10, 28), data.General.DelistedDate);
            Assert.Equal(65959661940996m, data.General.MarketCap);
            Assert.Equal(0.0789, data.Components[0].Weight);
            Assert.Equal(1105000000d, data.Financials.CashFlow.Yearly["2025-09-30"].IssuanceOfCapitalStock);
            Assert.Equal(21000000m, data.Statistics.MaxSupply);
        }

        // Trimmed from VFIAX.US: each section is followed by a lower-case duplicate key carrying null.
        private const string MutualFundJson =
            "{\"Fund_Style\":null,\"Nav\":681.0144,\"Update_Date\":\"2026-09-30\",\"Inception_Date\":\"1976-08-31\",\"Expense_Ratio_Date\":\"0000-00-00\",\"Portfolio_Net_Assets\":\"999999999999\",\"Yield_1Year_YTD\":17.4842,\"Expense_Ratio\":0.0381," +
            "\"Asset_Allocation\":{\"0\":{\"Net_%\":-0.1137,\"Long_%\":0.0542,\"Type\":\"Cash\",\"Short_%\":\"0.00000\",\"Category_Average\":null,\"Benchmark\":null}}," +
            "\"Top_Holdings\":{\"0\":{\"Name\":\"TSMC\",\"Weight\":\"4.10%\"}}," +
            "\"Value_Growth\":{\"0\":{\"Name\":\"Price/Book\",\"Category_Average\":4.1,\"Benchmark\":null,\"Stock_Portfolio\":\"3.9\"}}," +
            "\"Market_Capitalization\":{\"0\":{\"Size\":\"Giant\",\"Category_Average\":40.1,\"Benchmark\":null,\"Portfolio_%\":42.5}}," +
            "\"Sector_Weights\":{\"Bond Sector\":{\"0\":{\"Name\":\"Government\",\"Amount_%\":12.5,\"Stocks_%\":3.5}}}," +
            "\"World_Regions\":{\"Americas\":{\"0\":{\"Name\":\"North America\",\"Stocks_%\":9.0241}}}," +
            "\"Top_Countries\":{\"0\":{\"Country\":\"Japan\",\"Amount_%\":\"15.98\"}}," +
            "\"market_capitalization\":null,\"world_regions\":null,\"sector_weights\":null,\"asset_allocation\":null}";

        [Fact]
        public async Task GetFundamentals_MutualFund_KeepsSectionsDespiteNullDuplicateKeys()
        {
            using EodhdClient client = CreateClient("{\"MutualFund_Data\":" + MutualFundJson + "}");

            FundamentalData data = await client.GetFundamentalsDataAsync("VFIAX.US");

            AssertMutualFund(data.MutualFundData);
        }

        [Fact]
        public async Task GetFundamentals_MutualFundNullDuplicateFirst_KeepsLaterData()
        {
            const string json =
                "{\"MutualFund_Data\":{\"market_capitalization\":null," +
                "\"Market_Capitalization\":{\"0\":{\"Size\":\"Giant\",\"Portfolio_%\":42.5}}}}";

            using EodhdClient client = CreateClient(json);

            FundamentalData data = await client.GetFundamentalsDataAsync("VFIAX.US");

            Assert.Equal(42.5, data.MutualFundData.MarketCapitalization[0].PortfolioPercent);
        }

        [Fact]
        public async Task GetFundamentals_Filters_AreTrimmedDeduplicatedAndSent()
        {
            using EodhdClient client = CreateClient("{\"Code\":\"AAPL\"}", out FakeHttpTransport transport);

            FundamentalData data = await client.GetFundamentalsDataAsync("AAPL.US", " General , general,");

            Assert.Contains("filter=General&", transport.LastRequestUri.ToString() + "&", StringComparison.Ordinal);
            Assert.Equal("AAPL", data.General.Code);
        }

        [Fact]
        public async Task GetFundamentals_SingleSectionFilter_RewrapsUnwrappedSection()
        {
            // A single-section filter returns the section's own object, not { "MutualFund_Data": ... }.
            using EodhdClient client = CreateClient(MutualFundJson);

            FundamentalData data = await client.GetFundamentalsDataAsync("VFIAX.US", "MutualFund_Data");

            AssertMutualFund(data.MutualFundData);
        }

        [Fact]
        public async Task GetFundamentals_FilteredSectionTheSymbolLacks_IsNull()
        {
            using EodhdClient single = CreateClient("\"NA\"");
            FundamentalData none = await single.GetFundamentalsDataAsync("AAPL.US", "ETF_Data");
            Assert.Null(none.ETFData);

            using EodhdClient multi = CreateClient("{\"General\":{\"Code\":\"AAPL\"},\"ETF_Data\":\"NA\"}");
            FundamentalData some = await multi.GetFundamentalsDataAsync("AAPL.US", "General,ETF_Data");
            Assert.Equal("AAPL", some.General.Code);
            Assert.Null(some.ETFData);
        }

        [Fact]
        public async Task GetFundamentals_FieldPathFilter_ThrowsArgumentException()
        {
            using EodhdClient client = CreateClient("\"AAPL\"");

            ArgumentException ex = await Assert.ThrowsAsync<ArgumentException>(() => client.GetFundamentalsDataAsync("AAPL.US", "General::Code"));

            Assert.Equal("filters", ex.ParamName);
        }

        [Fact]
        public async Task GetFundamentals_Crypto_TechAndResources()
        {
            const string json =
                "{\"Tech\":{\"Developers\":{\"0\":\"Satoshi Nakamoto - Founder\"}}," +
                "\"Resources\":{\"Links\":{\"website\":{\"0\":\"https://bitcoin.org/\"},\"facebook\":null},\"Thumbnail\":\"/img/btc.png\"}}";

            using EodhdClient client = CreateClient(json);

            FundamentalData data = await client.GetFundamentalsDataAsync("BTC-USD.CC");

            Assert.Equal("Satoshi Nakamoto - Founder", data.Tech.Developers[0]);
            Assert.Equal("https://bitcoin.org/", data.Resources.Links["website"][0]);
            Assert.Null(data.Resources.Links["facebook"]);
            Assert.Equal("/img/btc.png", data.Resources.Thumbnail);
        }

        private static void AssertMutualFund(MutualFundData fund)
        {
            Assert.Equal(681.0144, fund.Nav);
            Assert.Equal(new DateTime(2026, 9, 30), fund.UpdateDate);
            Assert.Equal(new DateTime(1976, 8, 31), fund.InceptionDate);
            Assert.Null(fund.ExpenseRatioDate);
            Assert.Equal(999999999999m, fund.PortfolioNetAssets);
            Assert.Equal(17.4842, fund.Yield1Year);
            Assert.Equal(-0.1137, fund.AssetAllocation[0].NetPercentage);
            Assert.Equal(0d, fund.AssetAllocation[0].ShortPercentage);
            Assert.Equal("4.10%", fund.TopHoldings[0].Weight);
            Assert.Equal("Giant", fund.MarketCapitalization[0].Size);
            Assert.Equal(42.5, fund.MarketCapitalization[0].PortfolioPercent);
            Assert.Equal(40.1, fund.MarketCapitalization[0].CategoryAverage);
            Assert.Equal(4.1, fund.ValueGrowth[0].CategoryAverage);
            Assert.Equal(3.9, fund.ValueGrowth[0].StockPortfolio);
            Assert.Equal("Government", fund.SectorWeights["Bond Sector"][0].Name);
            Assert.Equal(12.5, fund.SectorWeights["Bond Sector"][0].AmountPercent);
            Assert.Equal(3.5, fund.SectorWeights["Bond Sector"][0].StocksPercent);
            Assert.Equal(9.0241, fund.WorldRegions["Americas"][0].StocksPercent);
            Assert.Equal("Japan", fund.TopCountries[0].Country);
            Assert.Equal(15.98, fund.TopCountries[0].AmountPercent);
        }

        [Fact]
        public async Task GetBulkFundamentals_BindsCashAndEquivalentsFilingDateReportDateAndIdentifiers()
        {
            const string json =
                "{\"0\":{\"General\":{\"Code\":\"AAPL\",\"OperatingMIC\":\"XNAS\",\"OpenFigi\":\"BBG000B9XRY4\"}," +
                "\"Earnings\":{\"Last_0\":{\"date\":\"2026-06-30\",\"reportDate\":\"2026-07-30\"}}," +
                "\"Financials\":{\"Balance_Sheet\":{\"yearly_last_0\":{\"cashAndEquivalents\":\"1597000000.00\"}}," +
                "\"Cash_Flow\":{\"quarterly_last_0\":{\"filing_date\":\"2026-08-05\"}}}}}";

            using EodhdClient client = CreateClient(json);

            Dictionary<string, BulkFundamentalData> bulk = await client.GetBulkFundamentalsDataAsync("US");

            BulkFundamentalData aapl = bulk["0"];
            Assert.Equal("XNAS", aapl.General.OperatingMIC);
            Assert.Equal("BBG000B9XRY4", aapl.General.OpenFigi);
            Assert.Equal(new DateTime(2026, 7, 30), aapl.Earnings["Last_0"].ReportDate);
            Assert.Equal(1597000000d, aapl.Financials.BalanceSheet.YearlyLast0.CashAndEquivalents);
            Assert.Equal("2026-08-05", aapl.Financials.CashFlow.QuarterlyLast0.FilingDate);
        }

        [Fact]
        public async Task GetLivePrice_DelistedSymbolNaTimestamp_MapsToNull()
        {
            const string json =
                "{\"code\":\"FFR_old.US\",\"timestamp\":\"NA\",\"gmtoffset\":0,\"open\":\"NA\",\"high\":\"NA\"," +
                "\"low\":\"NA\",\"close\":\"NA\",\"volume\":\"NA\",\"previousClose\":\"NA\",\"change\":\"NA\",\"change_p\":\"NA\"}";

            using EodhdClient client = CreateClient(json);

            LiveStockPrice quote = await client.GetLivePriceAsync("FFR_old.US");

            Assert.Equal("FFR_old.US", quote.Code);
            Assert.Null(quote.TimestampSec);
            Assert.Null(quote.TimestampUtc);
            Assert.Null(quote.Close);
        }

        [Fact]
        public async Task GetExchangeDetails_EarlyCloseDays_Deserialize()
        {
            const string json =
                "{\"Name\":\"USA Stocks\",\"Code\":\"US\",\"ExchangeEarlyCloseDays\":{\"0\":{\"Holiday\":\"Pre-Christmas Thursday 2026\"," +
                "\"Date\":\"2026-12-24\",\"Type\":\"official\",\"EarlyClose\":\"13:00\"}}}";

            using EodhdClient client = CreateClient(json);

            ExchangeDetails details = await client.GetExchangeDetailsAsync("US");

            ExchangeEarlyCloseDay day = details.ExchangeEarlyCloseDays[0];
            Assert.Equal("Pre-Christmas Thursday 2026", day.Holiday);
            Assert.Equal(new DateTime(2026, 12, 24), day.Date);
            Assert.Equal("official", day.Type);
            Assert.Equal(new TimeOnly(13, 0), day.EarlyClose);
        }

        [Fact]
        public async Task GetUserInfo_SubscriptionModeAndCanManageOrganizations_Deserialize()
        {
            const string json =
                "{\"name\":\"Test\",\"subscriptionType\":\"commercial\",\"subscriptionMode\":\"paid\",\"canManageOrganizations\":false}";

            using EodhdClient client = CreateClient(json);

            UserInfo user = await client.GetUserInfoAsync();

            Assert.Equal("paid", user.SubscriptionMode);
            Assert.False(user.CanManageOrganizations);
        }

        [Fact]
        public async Task GetUpcomingEarnings_FromAndTo_Deserialize()
        {
            const string json =
                "{\"type\":\"Earnings\",\"description\":\"Historical and upcoming Earnings\",\"from\":\"2026-10-06\",\"to\":\"2026-10-16\",\"earnings\":[]}";

            using EodhdClient client = CreateClient(json);

            UpcomingEarnings earnings = await client.GetUpcomingEarningsAsync();

            Assert.Equal(new DateTime(2026, 10, 6), earnings.From);
            Assert.Equal(new DateTime(2026, 10, 16), earnings.To);
        }
    }
}
