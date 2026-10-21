using System;
using System.Text.Json.Serialization;

namespace EODHD.CSharpApiClient.DataModel.ExchangeInfo
{
    /// <summary>
    /// Represents the trading hours of an exchange.
    /// </summary>
    public sealed class TradingHours
    {
        /// <summary>
        /// Gets or sets the raw wire value of the local open time.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("Open")]
        private string OpenStr { get; set; }

        /// <summary>
        /// Gets the local open time, parsed from the raw value.
        /// </summary>
        [JsonIgnore]
        public TimeOnly? Open => Utilities.ParseTime(this.OpenStr);

        /// <summary>
        /// Gets or sets the raw wire value of the local close time.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("Close")]
        private string CloseStr { get; set; }

        /// <summary>
        /// Gets the local close time, parsed from the raw value.
        /// </summary>
        [JsonIgnore]
        public TimeOnly? Close => Utilities.ParseTime(this.CloseStr);

        /// <summary>
        /// Gets or sets the raw wire value of the open time in UTC.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("OpenUTC")]
        private string OpenUTCStr { get; set; }

        /// <summary>
        /// Gets the open time in UTC, parsed from the raw value.
        /// </summary>
        [JsonIgnore]
        public TimeOnly? OpenUTC => Utilities.ParseTime(this.OpenUTCStr);

        /// <summary>
        /// Gets or sets the raw wire value of the close time in UTC.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("CloseUTC")]
        private string CloseUTCStr { get; set; }

        /// <summary>
        /// Gets the close time in UTC, parsed from the raw value.
        /// </summary>
        [JsonIgnore]
        public TimeOnly? CloseUTC => Utilities.ParseTime(this.CloseUTCStr);

        /// <summary>
        /// Gets or sets the working days of the exchange.
        /// </summary>
        public string WorkingDays { get; set; }
    }
}
