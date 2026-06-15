using System;
using System.Collections.Generic;
using System.Net;

using EODHD.CSharpApiClient.DataModel;
using EODHD.CSharpApiClient.DataModel.UpcomingIpos;

using Xunit;

namespace EODHD.CSharpApiClient.UnitTests
{
    public class RestCoreTests
    {
        private static EodhdClient CreateClient(string responseJson, out FakeHttpTransport transport)
        {
            transport = new FakeHttpTransport(HttpStatusCode.OK, responseJson);
            return new EodhdClient(new EodhdClientOptions { ApiToken = "test" }, transport);
        }

        [Fact]
        public async System.Threading.Tasks.Task GetIntraday_SpaceSeparatedDateTime_ParsesAndSendsInterval()
        {
            const string json =
                "[{\"timestamp\":1640984100,\"gmtoffset\":0,\"datetime\":\"2021-12-31 20:55:00\"," +
                "\"open\":177.5,\"high\":177.9,\"low\":177.4,\"close\":177.6,\"volume\":1234567}]";

            using EodhdClient client = CreateClient(json, out FakeHttpTransport transport);

            List<IntradayHistoricalStockPrice> bars = await client.GetIntradayHistoricalStockPricesAsync(
                "AAPL.US", IntradayInterval.OneHour);

            Assert.Single(bars);
            Assert.Equal(1640984100L, bars[0].Timestamp);
            Assert.Equal(new DateTime(2021, 12, 31, 20, 55, 0), bars[0].DateTime);
            Assert.Equal(177.6, bars[0].Close);
            Assert.Contains("interval=1h", transport.LastRequestUri.ToString(), StringComparison.Ordinal);
        }

        [Fact]
        public async System.Threading.Tasks.Task GetLivePrice_NaValues_MapToNull()
        {
            const string json =
                "{\"code\":\"AAPL.US\",\"timestamp\":1640984100,\"gmtoffset\":0,\"open\":\"NA\"," +
                "\"high\":\"NA\",\"low\":\"NA\",\"close\":\"NA\",\"volume\":\"NA\"," +
                "\"previousClose\":177.0,\"change\":\"NA\",\"change_p\":\"NA\"}";

            using EodhdClient client = CreateClient(json, out FakeHttpTransport _);

            LiveStockPrice quote = await client.GetLivePriceAsync("AAPL.US");

            Assert.Equal("AAPL.US", quote.Code);
            Assert.Null(quote.Close);
            Assert.Null(quote.Volume);
            Assert.Null(quote.ChangePercent);
            Assert.Equal(177.0, quote.PreviousClose);
        }

        [Fact]
        public async System.Threading.Tasks.Task GetLivePrices_Multi_SendsSParameterAndParsesNumbers()
        {
            const string json =
                "[{\"code\":\"AAPL.US\",\"close\":177.6,\"change_p\":1.23}," +
                "{\"code\":\"MSFT.US\",\"close\":330.1,\"change_p\":-0.5}]";

            using EodhdClient client = CreateClient(json, out FakeHttpTransport transport);

            LiveStockPrice[] quotes = await client.GetLivePricesAsync("AAPL.US", new[] { "MSFT.US" });

            Assert.Equal(2, quotes.Length);
            Assert.Equal(177.6, quotes[0].Close);
            Assert.Equal(-0.5, quotes[1].ChangePercent);
            Assert.Contains("s=MSFT.US", transport.LastRequestUri.ToString(), StringComparison.Ordinal);
        }

        [Fact]
        public async System.Threading.Tasks.Task Search_DeserializesFieldsAndSendsFilters()
        {
            const string json =
                "[{\"Code\":\"AAPL\",\"Exchange\":\"US\",\"Name\":\"Apple Inc\",\"Type\":\"Common Stock\"," +
                "\"Country\":\"USA\",\"Currency\":\"USD\",\"ISIN\":\"US0378331005\"," +
                "\"previousClose\":177.0,\"previousCloseDate\":\"2024-01-02\",\"isPrimary\":true}]";

            using EodhdClient client = CreateClient(json, out FakeHttpTransport transport);

            SearchResult[] results = await client.SearchAsync("apple", limit: 10, type: "stock");

            Assert.Single(results);
            Assert.Equal("AAPL", results[0].Code);
            Assert.Equal("US0378331005", results[0].Isin);
            Assert.True(results[0].IsPrimary);
            string uri = transport.LastRequestUri.ToString();
            Assert.Contains("limit=10", uri, StringComparison.Ordinal);
            Assert.Contains("type=stock", uri, StringComparison.Ordinal);
        }

        [Fact]
        public async System.Threading.Tasks.Task GetExchangesList_Deserializes()
        {
            const string json =
                "[{\"Name\":\"USA Stocks\",\"Code\":\"US\",\"OperatingMIC\":\"XNAS, XNYS\"," +
                "\"Country\":\"USA\",\"Currency\":\"USD\",\"CountryISO2\":\"US\",\"CountryISO3\":\"USA\"}]";

            using EodhdClient client = CreateClient(json, out FakeHttpTransport _);

            List<Exchange> exchanges = await client.GetExchangesListAsync();

            Assert.Single(exchanges);
            Assert.Equal("US", exchanges[0].Code);
            Assert.Equal("USA", exchanges[0].CountryISO3);
        }

        [Fact]
        public async System.Threading.Tasks.Task GetUpcomingIpos_SnakeCaseFields_Deserialize()
        {
            const string json =
                "{\"type\":\"IPOs\",\"description\":\"...\",\"from\":\"2024-01-01\",\"to\":\"2024-01-31\"," +
                "\"ipos\":[{\"code\":\"XYZ\",\"name\":\"XYZ Corp\",\"exchange\":\"NASDAQ\",\"currency\":\"USD\"," +
                "\"start_date\":\"2024-01-15\",\"price_from\":18.0,\"price_to\":20.0,\"offer_price\":19.0," +
                "\"shares\":1000000,\"deal_type\":\"Priced\",\"symbol\":\"XYZ\"}]}";

            using EodhdClient client = CreateClient(json, out FakeHttpTransport _);

            UpcomingIpos ipos = await client.GetUpcomingIposAsync();

            Assert.Single(ipos.Ipos);
            Ipo ipo = ipos.Ipos[0];
            Assert.Equal("XYZ Corp", ipo.Name);
            Assert.Equal(new DateTime(2024, 1, 15), ipo.StartDate);
            Assert.Equal(19.0, ipo.OfferPrice);
            Assert.Equal("Priced", ipo.DealType);
        }

        [Fact]
        public async System.Threading.Tasks.Task GetHistoricalDividends_Deserialize()
        {
            const string json =
                "[{\"date\":\"2024-02-09\",\"declarationDate\":\"2024-02-01\",\"recordDate\":\"2024-02-12\"," +
                "\"paymentDate\":\"2024-02-15\",\"period\":\"Quarterly\",\"value\":0.24," +
                "\"unadjustedValue\":0.24,\"currency\":\"USD\"}]";

            using EodhdClient client = CreateClient(json, out FakeHttpTransport _);

            HistoricalDividend[] dividends = await client.GetHistoricalDividendsAsync("AAPL.US");

            Assert.Single(dividends);
            Assert.Equal(new DateTime(2024, 2, 9), dividends[0].Date);
            Assert.Equal(0.24, dividends[0].Value);
            Assert.Equal("Quarterly", dividends[0].Period);
        }
    }
}
