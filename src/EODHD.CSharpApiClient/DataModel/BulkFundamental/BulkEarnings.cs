using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

using EODHD.CSharpApiClient.DataModel.EarningTrends;

namespace EODHD.CSharpApiClient.DataModel.BulkFundamental
{
    /// <summary>
    /// Represents a single earnings entry within a company's bulk fundamentals.
    /// </summary>
    public sealed class BulkEarnings
    {
        /// <summary>
        /// Gets or sets the raw wire value of the earnings date.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("Date")]
        private string DateStr { get; set; }

        /// <summary>
        /// Gets the earnings date, parsed from the raw value.
        /// </summary>
        [JsonIgnore]
        public DateTime? Date => Utilities.ParseDate(this.DateStr);

        /// <summary>
        /// Gets or sets the raw report date string (ISO date).
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("reportDate")]
        private string ReportDateStr { get; set; }

        /// <summary>
        /// Gets the date the earnings were reported, parsed from the raw value.
        /// </summary>
        [JsonIgnore]
        public DateTime? ReportDate => Utilities.ParseDate(this.ReportDateStr);

        /// <summary>
        /// Gets or sets the actual reported earnings per share.
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
        /// Gets or sets the earnings surprise (percent).
        /// </summary>
        public double? SurprisePercent { get; set; }

        /// <summary>
        /// Gets or sets the earning trends keyed by period.
        /// </summary>
        public Dictionary<string, Trend> Trend { get; set; }
    }
}
