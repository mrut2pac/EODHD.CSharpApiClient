using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using EODHD.CSharpApiClient.DataModel.EconomicEvents;
using EODHD.CSharpApiClient.DataModel.News;
using EODHD.CSharpApiClient.DataModel.Sentiment;

namespace EODHD.CSharpApiClient
{
    public sealed partial class EodhdClient
    {
        // ================================================================
        // Financial News API
        // ================================================================

        /// <summary>
        /// Returns financial news articles for a symbol and/or topic tag. At least one of
        /// <paramref name="symbol"/> or <paramref name="tag"/> must be supplied.
        /// </summary>
        /// <param name="symbol">Optional ticker to fetch news for (e.g. <c>"AAPL.US"</c>).</param>
        /// <param name="tag">Optional topic tag to fetch news for (e.g. <c>"technology"</c>).</param>
        /// <param name="from">Optional inclusive start date.</param>
        /// <param name="to">Optional inclusive end date.</param>
        /// <param name="limit">Optional results per request (1–1000, default 50).</param>
        /// <param name="offset">Optional pagination offset (default 0).</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>The matching news articles.</returns>
        public Task<NewsArticle[]> GetNewsAsync(string symbol = null, string tag = null, DateTime? from = null, DateTime? to = null, int? limit = null, int? offset = null, CancellationToken ct = default)
        {
            if(string.IsNullOrWhiteSpace(symbol) && string.IsNullOrWhiteSpace(tag))
            {
                throw new ArgumentException("At least one of symbol or tag must be supplied.", nameof(symbol));
            }

            return this.GetJsonAsync<NewsArticle[]>(
                "news",
                ct,
                ("s", symbol),
                ("t", tag),
                ("from", FormatDate(from)),
                ("to", FormatDate(to)),
                ("limit", FormatInt(limit)),
                ("offset", FormatInt(offset)));
        }

        /// <summary>
        /// Returns financial news articles for a symbol and/or topic tag. At least one of
        /// <paramref name="symbol"/> or <paramref name="tag"/> must be supplied.
        /// </summary>
        /// <param name="symbol">Optional ticker to fetch news for (e.g. <c>"AAPL.US"</c>).</param>
        /// <param name="tag">Optional topic tag to fetch news for (e.g. <c>"technology"</c>).</param>
        /// <param name="from">Optional inclusive start date.</param>
        /// <param name="to">Optional inclusive end date.</param>
        /// <param name="limit">Optional results per request (1–1000, default 50).</param>
        /// <param name="offset">Optional pagination offset (default 0).</param>
        /// <returns>The matching news articles.</returns>
        public NewsArticle[] GetNews(string symbol = null, string tag = null, DateTime? from = null, DateTime? to = null, int? limit = null, int? offset = null)
        {
            return this.GetNewsAsync(symbol, tag, from, to, limit, offset).GetAwaiter().GetResult();
        }

        // ================================================================
        // Sentiment API
        // ================================================================

        /// <summary>
        /// Returns daily aggregated news sentiment for one or more symbols, keyed by symbol code.
        /// </summary>
        /// <param name="symbols">The symbols to fetch sentiment for.</param>
        /// <param name="from">Optional inclusive start date.</param>
        /// <param name="to">Optional inclusive end date.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A dictionary of daily sentiment series keyed by symbol code.</returns>
        public Task<Dictionary<string, List<SentimentPoint>>> GetNewsSentimentsAsync(IReadOnlyList<string> symbols, DateTime? from = null, DateTime? to = null, CancellationToken ct = default)
        {
            return this.GetSentimentsAsync("sentiments", symbols, from, to, ct);
        }

        /// <summary>
        /// Returns daily aggregated news sentiment for one or more symbols, keyed by symbol code.
        /// </summary>
        /// <param name="symbols">The symbols to fetch sentiment for.</param>
        /// <param name="from">Optional inclusive start date.</param>
        /// <param name="to">Optional inclusive end date.</param>
        /// <returns>A dictionary of daily sentiment series keyed by symbol code.</returns>
        public Dictionary<string, List<SentimentPoint>> GetNewsSentiments(IReadOnlyList<string> symbols, DateTime? from = null, DateTime? to = null)
        {
            return this.GetNewsSentimentsAsync(symbols, from, to).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Returns daily aggregated social-media (tweets) sentiment for one or more symbols, keyed by
        /// symbol code.
        /// </summary>
        /// <param name="symbols">The symbols to fetch sentiment for.</param>
        /// <param name="from">Optional inclusive start date.</param>
        /// <param name="to">Optional inclusive end date.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A dictionary of daily sentiment series keyed by symbol code.</returns>
        public Task<Dictionary<string, List<SentimentPoint>>> GetTweetsSentimentsAsync(IReadOnlyList<string> symbols, DateTime? from = null, DateTime? to = null, CancellationToken ct = default)
        {
            return this.GetSentimentsAsync("tweets-sentiments", symbols, from, to, ct);
        }

        /// <summary>
        /// Returns daily aggregated social-media (tweets) sentiment for one or more symbols, keyed by
        /// symbol code.
        /// </summary>
        /// <param name="symbols">The symbols to fetch sentiment for.</param>
        /// <param name="from">Optional inclusive start date.</param>
        /// <param name="to">Optional inclusive end date.</param>
        /// <returns>A dictionary of daily sentiment series keyed by symbol code.</returns>
        public Dictionary<string, List<SentimentPoint>> GetTweetsSentiments(IReadOnlyList<string> symbols, DateTime? from = null, DateTime? to = null)
        {
            return this.GetTweetsSentimentsAsync(symbols, from, to).GetAwaiter().GetResult();
        }

        // ================================================================
        // Economic Events API
        // ================================================================

        /// <summary>
        /// Returns economic calendar events, optionally filtered.
        /// </summary>
        /// <param name="from">Optional inclusive start date.</param>
        /// <param name="to">Optional inclusive end date.</param>
        /// <param name="country">Optional ISO 3166 country code filter (e.g. <c>"US"</c>).</param>
        /// <param name="comparison">Optional comparison basis (<c>"mom"</c>, <c>"qoq"</c>, <c>"yoy"</c>).</param>
        /// <param name="type">Optional event-type filter (e.g. <c>"GDP Growth Rate"</c>).</param>
        /// <param name="offset">Optional pagination offset (0–1000, default 0).</param>
        /// <param name="limit">Optional number of results (0–1000, default 50).</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>The economic events.</returns>
        public Task<EconomicEvent[]> GetEconomicEventsAsync(DateTime? from = null, DateTime? to = null, string country = null, string comparison = null, string type = null, int? offset = null, int? limit = null, CancellationToken ct = default)
        {
            return this.GetJsonAsync<EconomicEvent[]>(
                "economic-events",
                ct,
                ("from", FormatDate(from)),
                ("to", FormatDate(to)),
                ("country", country),
                ("comparison", comparison),
                ("type", type),
                ("offset", FormatInt(offset)),
                ("limit", FormatInt(limit)));
        }

        /// <summary>
        /// Returns economic calendar events, optionally filtered.
        /// </summary>
        /// <param name="from">Optional inclusive start date.</param>
        /// <param name="to">Optional inclusive end date.</param>
        /// <param name="country">Optional ISO 3166 country code filter (e.g. <c>"US"</c>).</param>
        /// <param name="comparison">Optional comparison basis (<c>"mom"</c>, <c>"qoq"</c>, <c>"yoy"</c>).</param>
        /// <param name="type">Optional event-type filter (e.g. <c>"GDP Growth Rate"</c>).</param>
        /// <param name="offset">Optional pagination offset (0–1000, default 0).</param>
        /// <param name="limit">Optional number of results (0–1000, default 50).</param>
        /// <returns>The economic events.</returns>
        public EconomicEvent[] GetEconomicEvents(DateTime? from = null, DateTime? to = null, string country = null, string comparison = null, string type = null, int? offset = null, int? limit = null)
        {
            return this.GetEconomicEventsAsync(from, to, country, comparison, type, offset, limit).GetAwaiter().GetResult();
        }

        // ================================================================
        // News Word Weights
        // ================================================================

        /// <summary>
        /// Returns the weighted keywords extracted from recent news for a symbol, mapping each word to
        /// its relative weight.
        /// </summary>
        /// <param name="symbol">The symbol to query (e.g. <c>"AAPL.US"</c>).</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A dictionary mapping each keyword to its weight.</returns>
        public async Task<Dictionary<string, double>> GetNewsWordWeightsAsync(string symbol, CancellationToken ct = default)
        {
            if(string.IsNullOrWhiteSpace(symbol))
            {
                throw new ArgumentException("A symbol must be supplied.", nameof(symbol));
            }

            NewsWordWeightsResponse response = await this.GetJsonAsync<NewsWordWeightsResponse>(
                "news-word-weights",
                ct,
                ("s", symbol)).ConfigureAwait(false);

            return response?.Data ?? new Dictionary<string, double>();
        }

        /// <summary>
        /// Returns the weighted keywords extracted from recent news for a symbol, mapping each word to
        /// its relative weight.
        /// </summary>
        /// <param name="symbol">The symbol to query (e.g. <c>"AAPL.US"</c>).</param>
        /// <returns>A dictionary mapping each keyword to its weight.</returns>
        public Dictionary<string, double> GetNewsWordWeights(string symbol)
        {
            return this.GetNewsWordWeightsAsync(symbol).GetAwaiter().GetResult();
        }

        private Task<Dictionary<string, List<SentimentPoint>>> GetSentimentsAsync(string path, IReadOnlyList<string> symbols, DateTime? from, DateTime? to, CancellationToken ct)
        {
            if(symbols == null || symbols.Count == 0)
            {
                throw new ArgumentException("At least one symbol must be supplied.", nameof(symbols));
            }

            return this.GetJsonAsync<Dictionary<string, List<SentimentPoint>>>(
                path,
                ct,
                ("s", JoinSymbols(symbols)),
                ("from", FormatDate(from)),
                ("to", FormatDate(to)));
        }
    }
}
