using System;
using System.Text.Json.Serialization;

namespace EODHD.CSharpApiClient.DataModel.Treasury
{
    /// <summary>
    /// A US Treasury long-term rate for a single date and rate type (e.g. the 20-year benchmark, the
    /// average of rates over 10 years, or the long-term real rate).
    /// </summary>
    public sealed class TreasuryLongTermRate
    {
        /// <summary>
        /// Gets or sets the observation date.
        /// </summary>
        [JsonPropertyName("date")]
        public DateTime? Date { get; set; }

        /// <summary>
        /// Gets or sets the rate type (e.g. <c>"BC_20year"</c>, <c>"Over_10_Years"</c>, <c>"Real_Rate"</c>).
        /// </summary>
        [JsonPropertyName("rate_type")]
        public string RateType { get; set; }

        /// <summary>
        /// Gets or sets the rate, percent.
        /// </summary>
        [JsonPropertyName("rate")]
        public double? Rate { get; set; }

        /// <summary>
        /// Gets or sets the extrapolation factor, when present.
        /// </summary>
        [JsonPropertyName("extrapolation_factor")]
        public double? ExtrapolationFactor { get; set; }
    }
}
