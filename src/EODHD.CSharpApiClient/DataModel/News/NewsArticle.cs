using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace EODHD.CSharpApiClient.DataModel.News
{
    /// <summary>
    /// A single financial news article returned by the EODHD news API.
    /// </summary>
    public sealed class NewsArticle
    {
        /// <summary>
        /// Gets or sets the publication timestamp.
        /// </summary>
        [JsonPropertyName("date")]
        public DateTime? Date { get; set; }

        /// <summary>
        /// Gets or sets the article headline.
        /// </summary>
        [JsonPropertyName("title")]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the full article body text.
        /// </summary>
        [JsonPropertyName("content")]
        public string Content { get; set; }

        /// <summary>
        /// Gets or sets the URL of the original article.
        /// </summary>
        [JsonPropertyName("link")]
        public string Link { get; set; }

        /// <summary>
        /// Gets or sets the ticker symbols mentioned in the article.
        /// </summary>
        [JsonPropertyName("symbols")]
        public List<string> Symbols { get; set; }

        /// <summary>
        /// Gets or sets the topic tags classifying the article.
        /// </summary>
        [JsonPropertyName("tags")]
        public List<string> Tags { get; set; }

        /// <summary>
        /// Gets or sets the sentiment breakdown of the article.
        /// </summary>
        [JsonPropertyName("sentiment")]
        public NewsSentiment Sentiment { get; set; }
    }
}
