using System.Collections.Generic;

using EODHD.CSharpApiClient.DataModel.Fundamental;

namespace EODHD.CSharpApiClient.DataModel.BulkFundamental
{
    /// <summary>
    /// Represents the complete bulk-fundamentals payload for a single company.
    /// </summary>
    public sealed class BulkFundamentalData
    {
        /// <summary>
        /// Gets or sets the general company information.
        /// </summary>
        public General General { get; set; }

        /// <summary>
        /// Gets or sets the key financial highlights.
        /// </summary>
        public Highlights Highlights { get; set; }

        /// <summary>
        /// Gets or sets the valuation metrics.
        /// </summary>
        public Valuation Valuation { get; set; }

        /// <summary>
        /// Gets or sets the share statistics.
        /// </summary>
        public SharesStats SharesStats { get; set; }

        /// <summary>
        /// Gets or sets the technical indicators.
        /// </summary>
        public Technicals Technicals { get; set; }

        /// <summary>
        /// Gets or sets the splits and dividends information.
        /// </summary>
        public SplitsDividends SplitsDividends { get; set; }

        /// <summary>
        /// Gets or sets the earnings entries keyed by date.
        /// </summary>
        public Dictionary<string, BulkEarnings> Earnings { get; set; }

        /// <summary>
        /// Gets or sets the financial statements.
        /// </summary>
        public Financials Financials { get; set; }

        /// <summary>
        /// Gets or sets the analyst ratings.
        /// </summary>
        public AnalystRatings AnalystRatings { get; set; }
    }
}
