using System;
using System.Text.Json.Serialization;

using EODHD.CSharpApiClient.JsonSupport;

namespace EODHD.CSharpApiClient.DataModel.ExchangeInfo
{
    /// <summary>
    /// Represents the trading hours of an exchange.
    /// </summary>
    public sealed class TradingHours
    {
        /// <summary>
        /// Gets or sets the local open time.
        /// </summary>
        [JsonPropertyName("Open")]
        [JsonConverter(typeof(NaTolerantTimeConverter))]
        public TimeOnly? Open { get; set; }

        /// <summary>
        /// Gets or sets the local close time.
        /// </summary>
        [JsonPropertyName("Close")]
        [JsonConverter(typeof(NaTolerantTimeConverter))]
        public TimeOnly? Close { get; set; }

        /// <summary>
        /// Gets or sets the open time in UTC.
        /// </summary>
        [JsonPropertyName("OpenUTC")]
        [JsonConverter(typeof(NaTolerantTimeConverter))]
        public TimeOnly? OpenUTC { get; set; }

        /// <summary>
        /// Gets or sets the close time in UTC.
        /// </summary>
        [JsonPropertyName("CloseUTC")]
        [JsonConverter(typeof(NaTolerantTimeConverter))]
        public TimeOnly? CloseUTC { get; set; }

        /// <summary>
        /// Gets or sets the working days of the exchange.
        /// </summary>
        public string WorkingDays { get; set; }
    }
}
