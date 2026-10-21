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
            FakeHttpTransport transport = new FakeHttpTransport(HttpStatusCode.OK, responseJson);
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
            Assert.Equal("2022-10-28", data.General.DelistedDate);
            Assert.Equal(65959661940996m, data.General.MarketCap);
            Assert.Equal(0.0789, data.Components[0].Weight);
            Assert.Equal(1105000000d, data.Financials.CashFlow.Yearly["2025-09-30"].IssuanceOfCapitalStock);
            Assert.Equal(21000000m, data.Statistics.MaxSupply);
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
            Assert.Equal("2026-07-30", aapl.Earnings["Last_0"].ReportDate);
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
            Assert.Equal("13:00", day.EarlyClose);
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

            Assert.Equal("2026-10-06", earnings.From);
            Assert.Equal("2026-10-16", earnings.To);
        }
    }
}
