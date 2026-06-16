using System;
using System.Text.Json.Serialization;

namespace EODHD.CSharpApiClient.DataModel.Treasury
{
    /// <summary>
    /// A US Treasury rate for a single date and tenor. Used by the daily Treasury yield curve and the
    /// real (inflation-indexed) yield curve.
    /// </summary>
    public sealed class TreasuryRate
    {
        /// <summary>
        /// Gets or sets the observation date.
        /// </summary>
        [JsonPropertyName("date")]
        public DateTime? Date { get; set; }

        /// <summary>
        /// Gets or sets the tenor (e.g. <c>"1M"</c>, <c>"3M"</c>, <c>"2Y"</c>, <c>"10Y"</c>, <c>"30Y"</c>).
        /// </summary>
        [JsonPropertyName("tenor")]
        public string Tenor { get; set; }

        /// <summary>
        /// Gets or sets the rate, percent.
        /// </summary>
        [JsonPropertyName("rate")]
        public double? Rate { get; set; }
    }
}
