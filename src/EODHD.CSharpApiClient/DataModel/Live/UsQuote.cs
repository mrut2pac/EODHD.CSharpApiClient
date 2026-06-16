using System;
using System.Text.Json.Serialization;

namespace EODHD.CSharpApiClient.DataModel.Live
{
    /// <summary>
    /// A real-time US quote (top of book) delivered over the live (WebSocket) <c>us-quote</c> feed.
    /// </summary>
    public sealed class UsQuote
    {
        /// <summary>Gets or sets the symbol (e.g. <c>"TSLA"</c>).</summary>
        [JsonPropertyName("s")]
        public string Symbol { get; set; }

        /// <summary>Gets or sets the ask price.</summary>
        [JsonPropertyName("ap")]
        public double? AskPrice { get; set; }

        /// <summary>Gets or sets the ask size.</summary>
        [JsonPropertyName("as")]
        public long? AskSize { get; set; }

        /// <summary>Gets or sets the bid price.</summary>
        [JsonPropertyName("bp")]
        public double? BidPrice { get; set; }

        /// <summary>Gets or sets the bid size.</summary>
        [JsonPropertyName("bs")]
        public long? BidSize { get; set; }

        /// <summary>Gets or sets the quote timestamp, in milliseconds since the Unix epoch.</summary>
        [JsonPropertyName("t")]
        public long? TimestampMs { get; set; }

        /// <summary>Gets the quote timestamp as a UTC instant (derived from <see cref="TimestampMs"/>).</summary>
        [JsonIgnore]
        public DateTimeOffset? TimestampUtc => UnixTime.FromMilliseconds(this.TimestampMs);
    }
}
