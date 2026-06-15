using System;
using System.Text.Json.Serialization;

namespace EODHD.CSharpApiClient.DataModel.Fundamental
{
    /// <summary>
    /// Represents a single annual-earnings data point.
    /// </summary>
    public sealed class EarningsAnnualData
    {
        /// <summary>
        /// Gets or sets the raw date string (ISO date).
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("Date")]
        private string dateStr { get; set; }

        /// <summary>
        /// Gets the date (ISO date), parsed from the raw value.
        /// </summary>
        [JsonIgnore]
        public DateTime? Date => Utilities.ParseDate(this.dateStr);

        /// <summary>
        /// Gets or sets the actual earnings per share.
        /// </summary>
        public double? EpsActual { get; set; }
    }
}
