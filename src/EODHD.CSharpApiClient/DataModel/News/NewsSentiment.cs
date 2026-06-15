using System.Text.Json.Serialization;

namespace EODHD.CSharpApiClient.DataModel.News
{
    /// <summary>
    /// The sentiment breakdown attached to a financial news article.
    /// </summary>
    public sealed class NewsSentiment
    {
        /// <summary>
        /// Gets or sets the overall sentiment polarity (negative to positive).
        /// </summary>
        [JsonPropertyName("polarity")]
        public double? Polarity { get; set; }

        /// <summary>
        /// Gets or sets the negative sentiment component (0–1).
        /// </summary>
        [JsonPropertyName("neg")]
        public double? Negative { get; set; }

        /// <summary>
        /// Gets or sets the neutral sentiment component (0–1).
        /// </summary>
        [JsonPropertyName("neu")]
        public double? Neutral { get; set; }

        /// <summary>
        /// Gets or sets the positive sentiment component (0–1).
        /// </summary>
        [JsonPropertyName("pos")]
        public double? Positive { get; set; }
    }
}
