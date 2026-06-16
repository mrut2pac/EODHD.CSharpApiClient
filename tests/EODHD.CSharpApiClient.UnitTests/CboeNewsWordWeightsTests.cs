using System;
using System.Collections.Generic;
using System.Net;

using EODHD.CSharpApiClient.DataModel.Cboe;

using Xunit;

namespace EODHD.CSharpApiClient.UnitTests
{
    public class CboeNewsWordWeightsTests
    {
        private static EodhdClient CreateClient(string responseJson, out FakeHttpTransport transport)
        {
            transport = new FakeHttpTransport(HttpStatusCode.OK, responseJson);
            return new EodhdClient(new EodhdClientOptions { ApiToken = "test" }, transport);
        }

        [Fact]
        public async System.Threading.Tasks.Task GetCboeIndices_UnwrapsAttributes()
        {
            const string json =
                "{\"meta\":{\"total\":123},\"data\":[" +
                "{\"id\":\"BNL30N\",\"type\":\"cboe-index\",\"attributes\":{\"region\":\"Netherlands\"," +
                "\"index_code\":\"BNL30N\",\"feed_type\":\"snapshot_official_opening\",\"date\":\"2025-09-22\"," +
                "\"index_close\":297.2,\"index_divisor\":2498050528.313623}}]}";

            using EodhdClient client = CreateClient(json, out FakeHttpTransport transport);

            CboeIndex[] indices = await client.GetCboeIndicesAsync(limit: 1);

            Assert.Single(indices);
            Assert.Equal("BNL30N", indices[0].IndexCode);
            Assert.Equal("Netherlands", indices[0].Region);
            Assert.Equal(297.2, indices[0].IndexClose);
            Assert.Null(indices[0].Components);
            string uri = Uri.UnescapeDataString(transport.LastRequestUri.ToString());
            Assert.Contains("cboe/indices", uri, StringComparison.Ordinal);
            Assert.Contains("page[limit]=1", uri, StringComparison.Ordinal);
        }

        [Fact]
        public async System.Threading.Tasks.Task GetCboeIndex_FlattensComponentsAndSendsFilters()
        {
            const string json =
                "{\"meta\":{\"total\":1},\"data\":[{\"id\":\"BNL30N-2025-09-22\",\"type\":\"cboe-index\"," +
                "\"attributes\":{\"region\":\"Netherlands\",\"index_code\":\"BNL30N\"," +
                "\"feed_type\":\"snapshot_official_opening\",\"date\":\"2025-09-22\",\"index_close\":297.2," +
                "\"index_divisor\":2498050528.313623,\"effective_date\":null,\"review_date\":null}," +
                "\"components\":[{\"id\":\"c1\",\"type\":\"cboe-index-component\",\"attributes\":{" +
                "\"symbol\":\"KPN.AS\",\"isin\":\"NL0000009082\",\"name\":\"KPN KON ORD\",\"sedol\":null," +
                "\"closing_price\":4.122,\"currency\":\"EUR\",\"total_shares\":2147483647," +
                "\"market_cap\":17551955219.5425,\"index_weighting\":2.364131,\"sector\":\"Telecommunications\"}}]}]}";

            using EodhdClient client = CreateClient(json, out FakeHttpTransport transport);

            CboeIndex index = await client.GetCboeIndexAsync("BNL30N", "snapshot_official_opening", new DateTime(2025, 9, 22));

            Assert.NotNull(index);
            Assert.Equal("BNL30N", index.IndexCode);
            Assert.NotNull(index.Components);
            Assert.Single(index.Components);
            CboeIndexComponent component = index.Components[0];
            Assert.Equal("KPN.AS", component.Symbol);
            Assert.Equal("NL0000009082", component.Isin);
            Assert.Equal(2147483647m, component.TotalShares);
            Assert.Equal(17551955219.5425m, component.MarketCap);
            Assert.Equal("Telecommunications", component.Sector);
            string uri = Uri.UnescapeDataString(transport.LastRequestUri.ToString());
            Assert.Contains("cboe/index", uri, StringComparison.Ordinal);
            Assert.Contains("filter[index_code]=BNL30N", uri, StringComparison.Ordinal);
            Assert.Contains("filter[feed_type]=snapshot_official_opening", uri, StringComparison.Ordinal);
            Assert.Contains("filter[date]=2025-09-22", uri, StringComparison.Ordinal);
        }

        [Fact]
        public async System.Threading.Tasks.Task GetCboeIndex_NoCode_Throws()
        {
            using EodhdClient client = CreateClient("{\"data\":[]}", out FakeHttpTransport _);

            await Assert.ThrowsAsync<ArgumentException>(
                () => client.GetCboeIndexAsync(string.Empty, "snapshot_official_opening", new DateTime(2025, 9, 22)));
        }

        [Fact]
        public async System.Threading.Tasks.Task GetNewsWordWeights_ReturnsWordToWeightMap()
        {
            const string json =
                "{\"data\":{\"stock\":0.0156,\"market\":0.00867,\"ai\":0.00843},\"meta\":{},\"links\":{}}";

            using EodhdClient client = CreateClient(json, out FakeHttpTransport transport);

            Dictionary<string, double> weights = await client.GetNewsWordWeightsAsync("AAPL.US");

            Assert.Equal(3, weights.Count);
            Assert.Equal(0.0156, weights["stock"]);
            Assert.Equal(0.00843, weights["ai"]);
            Assert.Contains("news-word-weights", transport.LastRequestUri.ToString(), StringComparison.Ordinal);
            Assert.Contains("s=AAPL.US", transport.LastRequestUri.ToString(), StringComparison.Ordinal);
        }

        [Fact]
        public async System.Threading.Tasks.Task GetNewsWordWeights_NoSymbol_Throws()
        {
            using EodhdClient client = CreateClient("{\"data\":{}}", out FakeHttpTransport _);

            await Assert.ThrowsAsync<ArgumentException>(() => client.GetNewsWordWeightsAsync(null));
        }
    }
}
