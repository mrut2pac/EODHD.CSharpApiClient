using System;
using System.Text.Json.Serialization;

using EODHD.CSharpApiClient.JsonSupport;

namespace EODHD.CSharpApiClient.DataModel.Fundamental
{
    /// <summary>
    /// Represents a single historical-earnings data point with actual vs. estimated results.
    /// </summary>
    public sealed class EarningsHistoryData
    {
        /// <summary>
        /// Gets or sets the report date.
        /// </summary>
        [JsonPropertyName("ReportDate")]
        [JsonConverter(typeof(NaTolerantDateConverter))]
        public DateTime? ReportDate { get; set; }

        /// <summary>
        /// Gets or sets the date.
        /// </summary>
        [JsonPropertyName("Date")]
        [JsonConverter(typeof(NaTolerantDateConverter))]
        public DateTime? Date { get; set; }

        /// <summary>
        /// Gets or sets the before/after-market indicator.
        /// </summary>
        public string BeforeAfterMarket { get; set; }

        /// <summary>
        /// Gets or sets the reporting currency.
        /// </summary>
        public string Currency { get; set; }

        /// <summary>
        /// Gets or sets the actual earnings per share.
        /// </summary>
        public double? EpsActual { get; set; }

        /// <summary>
        /// Gets or sets the estimated earnings per share.
        /// </summary>
        public double? EpsEstimate { get; set; }

        /// <summary>
        /// Gets or sets the difference between actual and estimated earnings per share.
        /// </summary>
        public double? EpsDifference { get; set; }

        /// <summary>
        /// Gets or sets the surprise (percent).
        /// </summary>
        public double? SurprisePercent { get; set; }
    }
}
