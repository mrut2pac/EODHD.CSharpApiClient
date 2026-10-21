using System;
using System.Text.Json.Serialization;

namespace EODHD.CSharpApiClient.DataModel.ExchangeInfo
{
    /// <summary>
    /// Represents a scheduled early-close (half) trading day on an exchange.
    /// </summary>
    public sealed class ExchangeEarlyCloseDay
    {
        /// <summary>
        /// Gets or sets the name of the occasion (e.g. <c>"Pre-Christmas Thursday 2026"</c>).
        /// </summary>
        public string Holiday { get; set; }

        /// <summary>
        /// Gets or sets the date of the early close.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets the early-close type (e.g. <c>"official"</c>).
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the raw closing time string (e.g. <c>"13:00"</c>).
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("EarlyClose")]
        private string EarlyCloseStr { get; set; }

        /// <summary>
        /// Gets the closing time on that day, in the exchange's local time zone (see <see cref="ExchangeDetails.Timezone"/>), parsed from the raw value.
        /// </summary>
        [JsonIgnore]
        public TimeOnly? EarlyClose => Utilities.ParseTime(this.EarlyCloseStr);
    }
}
