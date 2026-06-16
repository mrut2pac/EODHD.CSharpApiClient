using System.Text.Json.Serialization;

namespace EODHD.CSharpApiClient.DataModel.Live
{
    /// <summary>
    /// A real-time US trade delivered over the live (WebSocket) <c>us</c> feed.
    /// </summary>
    public sealed class UsTrade
    {
        /// <summary>Gets or sets the symbol (e.g. <c>"TSLA"</c>).</summary>
        [JsonPropertyName("s")]
        public string Symbol { get; set; }

        /// <summary>Gets or sets the trade price.</summary>
        [JsonPropertyName("p")]
        public double? Price { get; set; }

        /// <summary>Gets or sets the sale-condition codes for the trade.</summary>
        [JsonPropertyName("c")]
        public int[] Conditions { get; set; }

        /// <summary>Gets or sets the trade volume.</summary>
        [JsonPropertyName("v")]
        public long? Volume { get; set; }

        /// <summary>Gets or sets whether the trade printed on a dark pool.</summary>
        [JsonPropertyName("dp")]
        public bool? DarkPool { get; set; }

        /// <summary>Gets or sets the market status (e.g. <c>"open"</c>, <c>"extended-hours"</c>).</summary>
        [JsonPropertyName("ms")]
        public string MarketStatus { get; set; }

        /// <summary>Gets or sets the trade timestamp, in milliseconds since the Unix epoch (UTC).</summary>
        [JsonPropertyName("t")]
        public long? Timestamp { get; set; }
    }
}
