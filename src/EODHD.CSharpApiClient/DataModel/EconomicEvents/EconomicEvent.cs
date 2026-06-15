using System;
using System.Text.Json.Serialization;

using EODHD.CSharpApiClient.JsonSupport;

namespace EODHD.CSharpApiClient.DataModel.EconomicEvents
{
    /// <summary>
    /// A single economic calendar event (e.g. a GDP or inflation release).
    /// </summary>
    public sealed class EconomicEvent
    {
        /// <summary>
        /// Gets or sets the event name (e.g. <c>"GDP Growth Rate"</c>).
        /// </summary>
        [JsonPropertyName("type")]
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the ISO 3166 country code the event applies to.
        /// </summary>
        [JsonPropertyName("country")]
        public string Country { get; set; }

        /// <summary>
        /// Gets or sets the event timestamp. EODHD sends this space-separated
        /// (<c>"yyyy-MM-dd HH:mm:ss"</c>), parsed via <see cref="SpaceSeparatedDateTimeConverter"/>.
        /// </summary>
        [JsonPropertyName("date")]
        [JsonConverter(typeof(SpaceSeparatedDateTimeConverter))]
        public DateTime? Date { get; set; }

        /// <summary>
        /// Gets or sets the reported (actual) value, or <c>null</c> when not yet released.
        /// </summary>
        [JsonPropertyName("actual")]
        public double? Actual { get; set; }

        /// <summary>
        /// Gets or sets the prior-period value.
        /// </summary>
        [JsonPropertyName("previous")]
        public double? Previous { get; set; }

        /// <summary>
        /// Gets or sets the forecast/estimate value.
        /// </summary>
        [JsonPropertyName("estimate")]
        public double? Estimate { get; set; }

        /// <summary>
        /// Gets or sets the absolute change from the previous value.
        /// </summary>
        [JsonPropertyName("change")]
        public double? Change { get; set; }

        /// <summary>
        /// Gets or sets the percentage change from the previous value.
        /// </summary>
        [JsonPropertyName("change_percentage")]
        public double? ChangePercentage { get; set; }

        /// <summary>
        /// Gets or sets the associated reporting period (e.g. <c>"Q4"</c>, <c>"Jan"</c>), or <c>null</c>.
        /// </summary>
        [JsonPropertyName("period")]
        public string Period { get; set; }

        /// <summary>
        /// Gets or sets the comparison basis (<c>"mom"</c>, <c>"qoq"</c>, <c>"yoy"</c>), or <c>null</c>.
        /// </summary>
        [JsonPropertyName("comparison")]
        public string Comparison { get; set; }
    }
}
