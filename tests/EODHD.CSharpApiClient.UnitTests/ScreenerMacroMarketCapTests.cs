using System;
using System.Net;

using EODHD.CSharpApiClient.DataModel.Macro;
using EODHD.CSharpApiClient.DataModel.MarketCap;
using EODHD.CSharpApiClient.DataModel.Screener;

using Xunit;

namespace EODHD.CSharpApiClient.UnitTests
{
    public class ScreenerMacroMarketCapTests
    {
        private static EodhdClient CreateClient(string responseJson, out FakeHttpTransport transport)
        {
            transport = new FakeHttpTransport(HttpStatusCode.OK, responseJson);
            return new EodhdClient(new EodhdClientOptions { ApiToken = "test" }, transport);
        }

        [Fact]
        public async System.Threading.Tasks.Task GetScreener_UnwrapsDataAndDeserializesFields()
        {
            const string json =
                "{\"data\":[{\"code\":\"AAPL\",\"name\":\"Apple Inc\",\"last_day_data_date\":\"2026-06-12\"," +
                "\"adjusted_close\":251.15,\"refund_1d\":-3.8,\"refund_1d_p\":-1.49,\"refund_5d\":-20.35," +
                "\"refund_5d_p\":-7.5,\"exchange\":\"US\",\"currency_symbol\":\"$\"," +
                "\"market_capitalization\":2606922247853,\"earnings_share\":null,\"dividend_yield\":0.0094," +
                "\"sector\":\"Technology\",\"industry\":\"Consumer Electronics\",\"avgvol_1d\":205," +
                "\"avgvol_200d\":233.49}]}";

            using EodhdClient client = CreateClient(json, out FakeHttpTransport _);

            ScreenerResult[] results = await client.GetScreenerAsync();

            Assert.Single(results);
            ScreenerResult result = results[0];
            Assert.Equal("AAPL", result.Code);
            Assert.Equal(2606922247853m, result.MarketCapitalization);
            Assert.Null(result.EarningsShare);
            Assert.Equal(233.49, result.AverageVolume200D);
            Assert.Equal("Technology", result.Sector);
        }

        [Fact]
        public async System.Threading.Tasks.Task GetScreener_SerializesFiltersAsJsonTriples()
        {
            using EodhdClient client = CreateClient("{\"data\":[]}", out FakeHttpTransport transport);

            await client.GetScreenerAsync(
                new[]
                {
                    new ScreenerFilter("market_capitalization", ">", 1000000000000L),
                    new ScreenerFilter("sector", "=", "Technology"),
                },
                signals: new[] { "200d_new_hi" },
                sort: "market_capitalization.desc",
                limit: 5);

            string uri = Uri.UnescapeDataString(transport.LastRequestUri.ToString());
            Assert.Contains("filters=[[\"market_capitalization\",\">\",1000000000000],[\"sector\",\"=\",\"Technology\"]]", uri, StringComparison.Ordinal);
            Assert.Contains("signals=200d_new_hi", uri, StringComparison.Ordinal);
            Assert.Contains("sort=market_capitalization.desc", uri, StringComparison.Ordinal);
            Assert.Contains("limit=5", uri, StringComparison.Ordinal);
        }

        [Fact]
        public async System.Threading.Tasks.Task GetScreener_EmptyData_ReturnsEmptyArray()
        {
            using EodhdClient client = CreateClient("{\"data\":[]}", out FakeHttpTransport _);

            ScreenerResult[] results = await client.GetScreenerAsync();

            Assert.NotNull(results);
            Assert.Empty(results);
        }

        [Fact]
        public async System.Threading.Tasks.Task GetMacroIndicator_DeserializesAndSendsIndicator()
        {
            const string json =
                "[{\"CountryCode\":\"FRA\",\"CountryName\":\"France\"," +
                "\"Indicator\":\"Inflation, consumer prices (annual %)\",\"Date\":\"2024-12-31\"," +
                "\"Period\":\"Annual\",\"Value\":1.999},{\"CountryCode\":\"FRA\",\"CountryName\":\"France\"," +
                "\"Indicator\":\"Inflation, consumer prices (annual %)\",\"Date\":\"2023-12-31\"," +
                "\"Period\":\"Annual\",\"Value\":null}]";

            using EodhdClient client = CreateClient(json, out FakeHttpTransport transport);

            MacroIndicatorValue[] values = await client.GetMacroIndicatorAsync("FRA", MacroIndicator.InflationConsumerPricesAnnual);

            Assert.Equal(2, values.Length);
            Assert.Equal("FRA", values[0].CountryCode);
            Assert.Equal(new DateTime(2024, 12, 31), values[0].Date);
            Assert.Equal(1.999, values[0].Value);
            Assert.Null(values[1].Value);
            Assert.Contains("macro-indicator/FRA", transport.LastRequestUri.ToString(), StringComparison.Ordinal);
            Assert.Contains("indicator=inflation_consumer_prices_annual", transport.LastRequestUri.ToString(), StringComparison.Ordinal);
        }

        [Fact]
        public async System.Threading.Tasks.Task GetHistoricalMarketCap_KeyedObject_ProjectedToDateOrderedArray()
        {
            // The endpoint returns an object keyed by sequential index; ensure it is unwrapped and sorted.
            const string json =
                "{\"1\":{\"date\":\"2024-12-31\",\"value\":3766499857000}," +
                "\"0\":{\"date\":\"2024-12-03\",\"value\":3667854451000}}";

            using EodhdClient client = CreateClient(json, out FakeHttpTransport _);

            HistoricalMarketCap[] caps = await client.GetHistoricalMarketCapAsync("AAPL.US");

            Assert.Equal(2, caps.Length);
            Assert.Equal(new DateTime(2024, 12, 3), caps[0].Date);
            Assert.Equal(3667854451000m, caps[0].Value);
            Assert.Equal(new DateTime(2024, 12, 31), caps[1].Date);
        }
    }
}
