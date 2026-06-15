using System.Collections.Generic;

using EODHD.CSharpApiClient.DataModel.EarningTrends;

namespace EODHD.CSharpApiClient.DataModel.BulkFundamental
{
    /// <summary>
    /// Represents a single earnings entry within a company's bulk fundamentals.
    /// </summary>
    public sealed class BulkEarnings
    {
        /// <summary>
        /// Gets or sets the earnings date (ISO date).
        /// </summary>
        public string Date { get; set; }

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
