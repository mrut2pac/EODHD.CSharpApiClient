using System.Text.Json.Serialization;

namespace EODHD.CSharpApiClient.DataModel.Live
{
    /// <summary>
    /// A real-time forex quote delivered over the live (WebSocket) <c>forex</c> feed.
    /// </summary>
    public sealed class ForexQuote
    {
        /// <summary>Gets or sets the symbol (e.g. <c>"EURUSD"</c>).</summary>
        [JsonPropertyName("s")]
        public string Symbol { get; set; }

        /// <summary>Gets or sets the ask price.</summary>
        [JsonPropertyName("a")]
        public double? Ask { get; set; }

        /// <summary>Gets or sets the bid price.</summary>
        [JsonPropertyName("b")]
        public double? Bid { get; set; }

        /// <summary>Gets or sets the day change (absolute).</summary>
        [JsonPropertyName("dd")]
        public double? DayChange { get; set; }

        /// <summary>Gets or sets the day change (percent).</summary>
        [JsonPropertyName("dc")]
        public double? DayChangePercent { get; set; }

        /// <summary>Gets or sets the raw <c>ppms</c> flag as sent by the API.</summary>
        [JsonPropertyName("ppms")]
        public bool? Ppms { get; set; }

        /// <summary>Gets or sets the quote timestamp, in milliseconds since the Unix epoch (UTC).</summary>
        [JsonPropertyName("t")]
        public long? Timestamp { get; set; }
    }
}
