using System;
using System.Text.Json.Serialization;

using EODHD.CSharpApiClient.JsonSupport;

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
        /// Gets or sets the closing time on that day, in the exchange's local time zone (see <see cref="ExchangeDetails.Timezone"/>).
        /// </summary>
        [JsonPropertyName("EarlyClose")]
        [JsonConverter(typeof(NaTolerantTimeConverter))]
        public TimeOnly? EarlyClose { get; set; }
    }
}
