using System;
using System.Text.Json.Serialization;

using EODHD.CSharpApiClient.JsonSupport;

namespace EODHD.CSharpApiClient.DataModel.Fundamental
{
    /// <summary>
    /// Represents a single annual-earnings data point.
    /// </summary>
    public sealed class EarningsAnnualData
    {
        /// <summary>
        /// Gets or sets the date.
        /// </summary>
        [JsonPropertyName("Date")]
        [JsonConverter(typeof(NaTolerantDateConverter))]
        public DateTime? Date { get; set; }

        /// <summary>
        /// Gets or sets the actual earnings per share.
        /// </summary>
        public double? EpsActual { get; set; }
    }
}
