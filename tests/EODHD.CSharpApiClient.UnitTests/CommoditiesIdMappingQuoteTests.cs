using System;
using System.Collections.Generic;
using System.Net;

using EODHD.CSharpApiClient.DataModel.Commodities;
using EODHD.CSharpApiClient.DataModel.IdMappings;
using EODHD.CSharpApiClient.DataModel.Quotes;

using Xunit;

namespace EODHD.CSharpApiClient.UnitTests
{
    public class CommoditiesIdMappingQuoteTests
    {
        private static EodhdClient CreateClient(string responseJson, out FakeHttpTransport transport)
        {
            transport = new FakeHttpTransport(HttpStatusCode.OK, responseJson);
            return new EodhdClient(new EodhdClientOptions { ApiToken = "test" }, transport);
        }

        [Fact]
        public async System.Threading.Tasks.Task GetCommodityHistoricalPrices_UnwrapsDateValueRows()
        {
            const string json =
                "{\"meta\":{\"name\":\"Crude Oil Prices: Brent - Europe\",\"interval\":\"monthly\",\"unit\":\"Dollars per Barrel\"}," +
                "\"data\":[{\"date\":\"2026-05-01\",\"value\":92.88},{\"date\":\"2026-04-01\",\"value\":124.24}]}";

            using EodhdClient client = CreateClient(json, out FakeHttpTransport transport);

            CommodityPrice[] prices = await client.GetCommodityHistoricalPricesAsync("BRENT", interval: "monthly");

            Assert.Equal(2, prices.Length);
            Assert.Equal(new DateTime(2026, 5, 1), prices[0].Date);
            Assert.Equal(92.88, prices[0].Value);
            string uri = transport.LastRequestUri.ToString();
            Assert.Contains("commodities/historical/BRENT", uri, StringComparison.Ordinal);
            Assert.Contains("interval=monthly", uri, StringComparison.Ordinal);
        }

        [Fact]
        public async System.Threading.Tasks.Task GetIdMapping_DeserializesIdentifiersAndSendsSymbolFilter()
        {
            const string json =
                "{\"meta\":{\"total\":1},\"data\":[{\"symbol\":\"AAPL.US\",\"isin\":\"US0378331005\"," +
                "\"figi\":\"BBG000B9XRY4\",\"lei\":\"HWUPKR0MPOU8FGXBT394\",\"cusip\":\"037833100\",\"cik\":\"0000320193\"}]}";

            using EodhdClient client = CreateClient(json, out FakeHttpTransport transport);

            IdMapping[] mappings = await client.GetIdMappingAsync(symbol: "AAPL.US");

            Assert.Single(mappings);
            Assert.Equal("US0378331005", mappings[0].Isin);
            Assert.Equal("BBG000B9XRY4", mappings[0].Figi);
            Assert.Equal("0000320193", mappings[0].Cik);
            string uri = Uri.UnescapeDataString(transport.LastRequestUri.ToString());
            Assert.Contains("id-mapping", uri, StringComparison.Ordinal);
            Assert.Contains("filter[symbol]=AAPL.US", uri, StringComparison.Ordinal);
        }

        [Fact]
        public async System.Threading.Tasks.Task GetIdMapping_NoFilters_Throws()
        {
            using EodhdClient client = CreateClient("{\"data\":[]}", out FakeHttpTransport _);

            await Assert.ThrowsAsync<ArgumentException>(() => client.GetIdMappingAsync());
        }

        [Fact]
        public async System.Threading.Tasks.Task GetUsDelayedQuotes_KeyedBySymbol_MapsWideFieldSet()
        {
            const string json =
                "{\"meta\":{\"count\":1},\"data\":{\"AAPL.US\":{\"symbol\":\"AAPL.US\",\"exchange\":\"XNAS\"," +
                "\"isoExchange\":\"XNAS\",\"bidPrice\":296.15,\"askSize\":40,\"bidTime\":1781555138000," +
                "\"lastTradePrice\":296.1595,\"marketCap\":4347959000000,\"sharesOutstanding\":15000000000," +
                "\"forwardPE\":29.5,\"previousCloseDate\":\"2026-06-12 17:00:00\",\"primary\":true}}}";

            using EodhdClient client = CreateClient(json, out FakeHttpTransport transport);

            Dictionary<string, UsDelayedQuote> quotes = await client.GetUsDelayedQuotesAsync(new[] { "AAPL" });

            Assert.True(quotes.ContainsKey("AAPL.US"));
            UsDelayedQuote q = quotes["AAPL.US"];
            Assert.Equal("XNAS", q.Exchange);
            Assert.Equal(296.15, q.BidPrice);
            Assert.Equal(40, q.AskSize);
            Assert.Equal(1781555138000, q.BidTimeMs);
            Assert.Equal(DateTimeOffset.FromUnixTimeMilliseconds(1781555138000), q.BidTimeUtc);
            Assert.Equal(4347959000000m, q.MarketCap);
            Assert.Equal(29.5, q.ForwardPe);
            Assert.True(q.Primary);
            Assert.Equal("2026-06-12 17:00:00", q.PreviousCloseDate);
            Assert.Contains("s=AAPL", transport.LastRequestUri.ToString(), StringComparison.Ordinal);
        }

        [Fact]
        public async System.Threading.Tasks.Task GetOptionUnderlyingSymbols_CompactStringArray()
        {
            const string json =
                "{\"meta\":{\"total\":6758,\"fields\":[\"underlying_symbol\"],\"compact\":true}," +
                "\"data\":[\"A\",\"AA\",\"AAPL\",\"MSFT\"]}";

            using EodhdClient client = CreateClient(json, out FakeHttpTransport transport);

            string[] symbols = await client.GetOptionUnderlyingSymbolsAsync(limit: 4);

            Assert.Equal(4, symbols.Length);
            Assert.Contains("AAPL", symbols);
            string uri = Uri.UnescapeDataString(transport.LastRequestUri.ToString());
            Assert.Contains("mp/unicornbay/options/underlying-symbols", uri, StringComparison.Ordinal);
            Assert.Contains("page[limit]=4", uri, StringComparison.Ordinal);
        }
    }
}
