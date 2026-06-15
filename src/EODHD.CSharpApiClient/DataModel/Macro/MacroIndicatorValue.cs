using System;
using System.Text.Json.Serialization;

namespace EODHD.CSharpApiClient.DataModel.Macro
{
    /// <summary>
    /// A single macroeconomic indicator observation for a country.
    /// </summary>
    public sealed class MacroIndicatorValue
    {
        /// <summary>
        /// Gets or sets the ISO 3166-1 alpha-3 country code (e.g. <c>"USA"</c>).
        /// </summary>
        [JsonPropertyName("CountryCode")]
        public string CountryCode { get; set; }

        /// <summary>
        /// Gets or sets the country name.
        /// </summary>
        [JsonPropertyName("CountryName")]
        public string CountryName { get; set; }

        /// <summary>
        /// Gets or sets the human-readable indicator name (e.g. <c>"Inflation, consumer prices (annual %)"</c>).
        /// </summary>
        [JsonPropertyName("Indicator")]
        public string Indicator { get; set; }

        /// <summary>
        /// Gets or sets the observation date.
        /// </summary>
        [JsonPropertyName("Date")]
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets the reporting period (e.g. <c>"Annual"</c>).
        /// </summary>
        [JsonPropertyName("Period")]
        public string Period { get; set; }

        /// <summary>
        /// Gets or sets the observed value, or <c>null</c> when unavailable for the period.
        /// </summary>
        [JsonPropertyName("Value")]
        public double? Value { get; set; }
    }
}
