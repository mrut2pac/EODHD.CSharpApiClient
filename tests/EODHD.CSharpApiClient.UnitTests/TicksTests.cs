using System;
using System.Net;

using EODHD.CSharpApiClient.DataModel.Ticks;

using Xunit;

namespace EODHD.CSharpApiClient.UnitTests
{
    public class TicksTests
    {
        private static EodhdClient CreateClient(string responseJson, out FakeHttpTransport transport)
        {
            transport = new FakeHttpTransport(HttpStatusCode.OK, responseJson);
            return new EodhdClient(new EodhdClientOptions { ApiToken = "test" }, transport);
        }

        [Fact]
        public async System.Threading.Tasks.Task GetTicks_TransposesColumnsIntoRows()
        {
            const string json =
                "{\"mkt\":[\"D\",\"Q\"],\"sub_mkt\":[\"Q\",\"\"],\"price\":[187.22,187.23]," +
                "\"seq\":[699924226,699924760],\"shares\":[10,1],\"sl\":[\"@ TI\",\"@ T\"]," +
                "\"ts\":[1700000001537,1700000010213]}";

            using EodhdClient client = CreateClient(json, out FakeHttpTransport transport);

            Tick[] ticks = await client.GetTicksAsync("AAPL.US", limit: 2);

            Assert.Equal(2, ticks.Length);
            Assert.Equal("D", ticks[0].Market);
            Assert.Equal("Q", ticks[0].SubMarket);
            Assert.Equal(187.22, ticks[0].Price);
            Assert.Equal(699924226, ticks[0].Sequence);
            Assert.Equal(10, ticks[0].Shares);
            Assert.Equal("@ TI", ticks[0].SalesCondition);
            Assert.Equal(1700000001537, ticks[0].Timestamp);
            // "ex" column absent in this response -> null, not an exception.
            Assert.Null(ticks[0].Exchange);
            Assert.Equal(187.23, ticks[1].Price);

            string uri = transport.LastRequestUri.ToString();
            Assert.Contains("ticks", uri, StringComparison.Ordinal);
            Assert.Contains("s=AAPL.US", uri, StringComparison.Ordinal);
            Assert.Contains("limit=2", uri, StringComparison.Ordinal);
        }

        [Fact]
        public async System.Threading.Tasks.Task GetTicks_SendsFromToAsUnixSeconds()
        {
            using EodhdClient client = CreateClient("{\"ts\":[]}", out FakeHttpTransport transport);

            await client.GetTicksAsync(
                "AAPL.US",
                from: new DateTime(2023, 11, 14, 22, 13, 20, DateTimeKind.Utc),
                to: new DateTime(2023, 11, 14, 22, 23, 20, DateTimeKind.Utc));

            string uri = transport.LastRequestUri.ToString();
            Assert.Contains("from=1700000000", uri, StringComparison.Ordinal);
            Assert.Contains("to=1700000600", uri, StringComparison.Ordinal);
        }

        [Fact]
        public async System.Threading.Tasks.Task GetTicks_EmptyResponse_ReturnsEmpty()
        {
            using EodhdClient client = CreateClient("{}", out FakeHttpTransport _);

            Tick[] ticks = await client.GetTicksAsync("AAPL.US");

            Assert.Empty(ticks);
        }

        [Fact]
        public async System.Threading.Tasks.Task GetTicks_NullSymbol_Throws()
        {
            using EodhdClient client = CreateClient("{}", out FakeHttpTransport _);

            await Assert.ThrowsAsync<ArgumentNullException>(() => client.GetTicksAsync(null));
        }
    }
}
