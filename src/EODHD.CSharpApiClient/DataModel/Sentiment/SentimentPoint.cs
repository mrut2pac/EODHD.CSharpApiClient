using System;
using System.Text.Json.Serialization;

namespace EODHD.CSharpApiClient.DataModel.Sentiment
{
    /// <summary>
    /// A single daily aggregated sentiment observation for a symbol, from either the news or the
    /// social (tweets) sentiment feed.
    /// </summary>
    public sealed class SentimentPoint
    {
        /// <summary>
        /// Gets or sets the date the sentiment was aggregated over.
        /// </summary>
        [JsonPropertyName("date")]
        public DateTime? Date { get; set; }

        /// <summary>
        /// Gets or sets the number of mentions/observations aggregated for the date.
        /// </summary>
        [JsonPropertyName("count")]
        public int? Count { get; set; }

        /// <summary>
        /// Gets or sets the normalized sentiment score, from -1 (very negative) to 1 (very positive).
        /// </summary>
        [JsonPropertyName("normalized")]
        public double? Normalized { get; set; }
    }
}
