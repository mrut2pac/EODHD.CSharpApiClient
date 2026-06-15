using System.Text.Json.Serialization;

using EODHD.CSharpApiClient.JsonSupport;

namespace EODHD.CSharpApiClient.DataModel
{
    /// <summary>
    /// A live (delayed) real-time quote for a symbol. Stock prices are delayed roughly 15–20 minutes.
    /// Numeric fields come back as the literal <c>"NA"</c> when unavailable (e.g. outside trading hours)
    /// and are mapped to <c>null</c>.
    /// </summary>
    public sealed class LiveStockPrice
    {
        /// <summary>
        /// Gets or sets the full EODHD code (symbol and exchange).
        /// </summary>
        [JsonPropertyName("code")]
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets the Unix epoch timestamp (seconds) of the quote.
        /// </summary>
        [JsonPropertyName("timestamp")]
        public long? Timestamp { get; set; }

        /// <summary>
        /// Gets or sets the GMT offset applied to the quote time.
        /// </summary>
        [JsonPropertyName("gmtoffset")]
        public int? GmtOffset { get; set; }

        /// <summary>
        /// Gets or sets the opening price.
        /// </summary>
        [JsonPropertyName("open")]
        [JsonConverter(typeof(NaTolerantDoubleConverter))]
        public double? Open { get; set; }

        /// <summary>
        /// Gets or sets the high price.
        /// </summary>
        [JsonPropertyName("high")]
        [JsonConverter(typeof(NaTolerantDoubleConverter))]
        public double? High { get; set; }

        /// <summary>
        /// Gets or sets the low price.
        /// </summary>
        [JsonPropertyName("low")]
        [JsonConverter(typeof(NaTolerantDoubleConverter))]
        public double? Low { get; set; }

        /// <summary>
        /// Gets or sets the latest/closing price.
        /// </summary>
        [JsonPropertyName("close")]
        [JsonConverter(typeof(NaTolerantDoubleConverter))]
        public double? Close { get; set; }

        /// <summary>
        /// Gets or sets the traded volume.
        /// </summary>
        [JsonPropertyName("volume")]
        [JsonConverter(typeof(NaTolerantDoubleConverter))]
        public double? Volume { get; set; }

        /// <summary>
        /// Gets or sets the prior day's closing price.
        /// </summary>
        [JsonPropertyName("previousClose")]
        [JsonConverter(typeof(NaTolerantDoubleConverter))]
        public double? PreviousClose { get; set; }

        /// <summary>
        /// Gets or sets the absolute price change since the previous close.
        /// </summary>
        [JsonPropertyName("change")]
        [JsonConverter(typeof(NaTolerantDoubleConverter))]
        public double? Change { get; set; }

        /// <summary>
        /// Gets or sets the percentage price change since the previous close.
        /// </summary>
        [JsonPropertyName("change_p")]
        [JsonConverter(typeof(NaTolerantDoubleConverter))]
        public double? ChangePercent { get; set; }
    }
}
