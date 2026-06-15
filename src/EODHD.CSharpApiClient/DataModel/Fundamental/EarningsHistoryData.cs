using System;
using System.Text.Json.Serialization;

namespace EODHD.CSharpApiClient.DataModel.Fundamental
{
    /// <summary>
    /// Represents a single historical-earnings data point with actual vs. estimated results.
    /// </summary>
    public sealed class EarningsHistoryData
    {
        /// <summary>
        /// Gets or sets the raw report date string (ISO date).
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("ReportDate")]
        private string ReportDateStr { get; set; }

        /// <summary>
        /// Gets the report date (ISO date), parsed from the raw value.
        /// </summary>
        [JsonIgnore]
        public DateTime? ReportDate => Utilities.ParseDate(this.ReportDateStr);

        /// <summary>
        /// Gets or sets the raw date string (ISO date).
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("Date")]
        private string DateStr { get; set; }

        /// <summary>
        /// Gets the date (ISO date), parsed from the raw value.
        /// </summary>
        [JsonIgnore]
        public DateTime? Date => Utilities.ParseDate(this.DateStr);

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
