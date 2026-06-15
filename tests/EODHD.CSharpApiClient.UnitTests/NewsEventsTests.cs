using System;
using System.Collections.Generic;
using System.Net;

using EODHD.CSharpApiClient.DataModel.EconomicEvents;
using EODHD.CSharpApiClient.DataModel.News;
using EODHD.CSharpApiClient.DataModel.Sentiment;

using Xunit;

namespace EODHD.CSharpApiClient.UnitTests
{
    public class NewsEventsTests
    {
        private static EodhdClient CreateClient(string responseJson, out FakeHttpTransport transport)
        {
            transport = new FakeHttpTransport(HttpStatusCode.OK, responseJson);
            return new EodhdClient(new EodhdClientOptions { ApiToken = "test" }, transport);
        }

        [Fact]
        public async System.Threading.Tasks.Task GetNews_DeserializesArticleAndSentiment_AndSendsSymbolParam()
        {
            const string json =
                "[{\"date\":\"2024-01-02T12:34:56+00:00\",\"title\":\"Apple rises\",\"content\":\"body\"," +
                "\"link\":\"https://example.com/a\",\"symbols\":[\"AAPL.US\"],\"tags\":[\"technology\"]," +
                "\"sentiment\":{\"polarity\":0.42,\"neg\":0.1,\"neu\":0.5,\"pos\":0.4}}]";

            using EodhdClient client = CreateClient(json, out FakeHttpTransport transport);

            NewsArticle[] news = await client.GetNewsAsync(symbol: "AAPL.US", limit: 5);

            Assert.Single(news);
            Assert.Equal("Apple rises", news[0].Title);
            Assert.Equal(new[] { "AAPL.US" }, news[0].Symbols);
            Assert.Equal(0.42, news[0].Sentiment.Polarity);
            Assert.Equal(0.4, news[0].Sentiment.Positive);
            string uri = transport.LastRequestUri.ToString();
            Assert.Contains("s=AAPL.US", uri, StringComparison.Ordinal);
            Assert.Contains("limit=5", uri, StringComparison.Ordinal);
        }

        [Fact]
        public void GetNews_NoSymbolOrTag_Throws()
        {
            using EodhdClient client = new EodhdClient(new EodhdClientOptions { ApiToken = "test" },
                new FakeHttpTransport(HttpStatusCode.OK, "[]"));

            Assert.Throws<ArgumentException>(() => client.GetNews());
        }

        [Fact]
        public async System.Threading.Tasks.Task GetNewsSentiments_KeyedBySymbol_Deserializes()
        {
            const string json =
                "{\"AAPL.US\":[{\"date\":\"2022-04-22\",\"count\":31,\"normalized\":0.1835}]," +
                "\"BTC-USD.CC\":[{\"date\":\"2022-04-22\",\"count\":120,\"normalized\":-0.2}]}";

            using EodhdClient client = CreateClient(json, out FakeHttpTransport transport);

            Dictionary<string, List<SentimentPoint>> sentiment =
                await client.GetNewsSentimentsAsync(new[] { "AAPL.US", "BTC-USD.CC" });

            Assert.Equal(2, sentiment.Count);
            Assert.Equal(31, sentiment["AAPL.US"][0].Count);
            Assert.Equal(0.1835, sentiment["AAPL.US"][0].Normalized);
            Assert.Equal(new DateTime(2022, 4, 22), sentiment["AAPL.US"][0].Date);
            // The comma between symbols is percent-encoded by Uri.EscapeDataString (EODHD decodes it).
            Assert.Contains("s=AAPL.US%2CBTC-USD.CC", transport.LastRequestUri.ToString(), StringComparison.Ordinal);
        }

        [Fact]
        public async System.Threading.Tasks.Task GetEconomicEvents_SpaceSeparatedDate_DeserializesAndSendsCountry()
        {
            const string json =
                "[{\"type\":\"GDP Growth Rate\",\"country\":\"US\",\"date\":\"2024-01-25 13:30:00\"," +
                "\"actual\":3.3,\"previous\":4.9,\"estimate\":2.0,\"change\":-1.6," +
                "\"change_percentage\":-32.65,\"period\":\"Q4\",\"comparison\":\"qoq\"}]";

            using EodhdClient client = CreateClient(json, out FakeHttpTransport transport);

            EconomicEvent[] events = await client.GetEconomicEventsAsync(country: "US", comparison: "qoq");

            Assert.Single(events);
            Assert.Equal("GDP Growth Rate", events[0].Type);
            Assert.Equal(new DateTime(2024, 1, 25, 13, 30, 0), events[0].Date);
            Assert.Equal(3.3, events[0].Actual);
            Assert.Equal(-32.65, events[0].ChangePercentage);
            string uri = transport.LastRequestUri.ToString();
            Assert.Contains("country=US", uri, StringComparison.Ordinal);
            Assert.Contains("comparison=qoq", uri, StringComparison.Ordinal);
        }
    }
}
