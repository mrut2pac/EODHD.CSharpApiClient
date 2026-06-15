using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using EODHD.CSharpApiClient.DataModel.EconomicEvents;
using EODHD.CSharpApiClient.DataModel.News;
using EODHD.CSharpApiClient.DataModel.Sentiment;
using EODHD.CSharpApiClient.Exceptions;

using Xunit;

namespace EODHD.CSharpApiClient.IntegrationTests
{
    public class NewsEventsIntegrationTests : IntegrationTestBase
    {
        [SkippableFact]
        public async Task GetNews_Apple_ReturnsArticles()
        {
            this.SkipIfNoApiKey();
            using EodhdClient client = this.CreateClient();

            NewsArticle[] news;
            try
            {
                news = await client.GetNewsAsync(symbol: "AAPL.US", limit: 10);
            }
            catch (EodhdHttpException ex)
            {
                SkipIfNoLicense(ex);
                throw;
            }

            Assert.NotNull(news);
            Skip.If(news.Length == 0, "No news returned for the symbol.");
            Assert.False(string.IsNullOrEmpty(news[0].Title));
        }

        [SkippableFact]
        public async Task GetNewsSentiments_Apple_ReturnsSeries()
        {
            this.SkipIfNoApiKey();
            using EodhdClient client = this.CreateClient();

            Dictionary<string, List<SentimentPoint>> sentiment;
            try
            {
                // Two symbols exercises the comma-separated `s` parameter against the live API.
                sentiment = await client.GetNewsSentimentsAsync(
                    new[] { "AAPL.US", "MSFT.US" }, new DateTime(2022, 1, 1), new DateTime(2022, 6, 1));
            }
            catch (EodhdHttpException ex)
            {
                SkipIfNoLicense(ex);
                throw;
            }

            Assert.NotNull(sentiment);
            Skip.If(sentiment.Count == 0, "No sentiment returned on this subscription/date range.");
            // Confirms the comma-separated `s` parameter is accepted by the live API.
            Assert.True(sentiment.ContainsKey("AAPL.US") && sentiment.ContainsKey("MSFT.US"),
                "Expected both requested symbols in the multi-symbol sentiment response.");
        }

        [SkippableFact]
        public async Task GetTweetsSentiments_Apple_ReturnsSeries()
        {
            this.SkipIfNoApiKey();
            using EodhdClient client = this.CreateClient();

            Dictionary<string, List<SentimentPoint>> sentiment;
            try
            {
                sentiment = await client.GetTweetsSentimentsAsync(
                    new[] { "AAPL.US", "MSFT.US" }, new DateTime(2022, 1, 1), new DateTime(2022, 6, 1));
            }
            catch (EodhdHttpException ex)
            {
                SkipIfNoLicense(ex);
                throw;
            }

            Assert.NotNull(sentiment);
            Skip.If(sentiment.Count == 0, "No tweet sentiment returned on this subscription/date range.");
        }

        [SkippableFact]
        public async Task GetEconomicEvents_Us_ReturnsEvents()
        {
            this.SkipIfNoApiKey();
            using EodhdClient client = this.CreateClient();

            EconomicEvent[] events;
            try
            {
                events = await client.GetEconomicEventsAsync(
                    new DateTime(2024, 1, 1), new DateTime(2024, 1, 31), country: "US", limit: 50);
            }
            catch (EodhdHttpException ex)
            {
                SkipIfNoLicense(ex);
                throw;
            }

            Assert.NotNull(events);
            Skip.If(events.Length == 0, "No economic events returned for the range.");
            Assert.False(string.IsNullOrEmpty(events[0].Type));
        }
    }
}
