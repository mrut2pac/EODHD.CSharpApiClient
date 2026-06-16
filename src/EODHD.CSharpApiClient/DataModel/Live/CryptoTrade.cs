using System.Text.Json.Serialization;

namespace EODHD.CSharpApiClient.DataModel.Live
{
    /// <summary>
    /// A real-time crypto trade delivered over the live (WebSocket) <c>crypto</c> feed.
    /// </summary>
    public sealed class CryptoTrade
    {
        /// <summary>Gets or sets the symbol (e.g. <c>"BTC-USD"</c>).</summary>
        [JsonPropertyName("s")]
        public string Symbol { get; set; }

        /// <summary>Gets or sets the trade price.</summary>
        [JsonPropertyName("p")]
        public double? Price { get; set; }

        /// <summary>Gets or sets the traded quantity.</summary>
        [JsonPropertyName("q")]
        public double? Quantity { get; set; }

        /// <summary>Gets or sets the day change (absolute).</summary>
        [JsonPropertyName("dd")]
        public double? DayChange { get; set; }

        /// <summary>Gets or sets the day change (percent).</summary>
        [JsonPropertyName("dc")]
        public double? DayChangePercent { get; set; }

        /// <summary>Gets or sets the trade timestamp, in milliseconds since the Unix epoch (UTC).</summary>
        [JsonPropertyName("t")]
        public long? Timestamp { get; set; }
    }
}
