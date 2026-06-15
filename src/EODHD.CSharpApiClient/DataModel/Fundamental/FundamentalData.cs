using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace EODHD.CSharpApiClient.DataModel.Fundamental
{
    /// <summary>
    /// Represents the full fundamental data payload for an instrument.
    /// </summary>
    public sealed class FundamentalData
    {
        /// <summary>
        /// Gets or sets the general company information.
        /// </summary>
        public General General { get; set; }

        /// <summary>
        /// Gets or sets the financial highlights.
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
        /// Gets or sets the splits and dividends data.
        /// </summary>
        public SplitsDividends SplitsDividends { get; set; }

        /// <summary>
        /// Gets or sets the analyst ratings.
        /// </summary>
        public AnalystRatings AnalystRatings { get; set; }

        /// <summary>
        /// Gets or sets the institutional and fund holders.
        /// </summary>
        public Holders Holders { get; set; }

        /// <summary>
        /// Gets or sets the insider transactions, keyed by transaction identifier.
        /// </summary>
        public Dictionary<string, InsiderTransactions> InsiderTransactions { get; set; }

        /// <summary>
        /// Gets or sets the ESG scores.
        /// </summary>
        public ESGScores ESGScores { get; set; }

        /// <summary>
        /// Gets or sets the outstanding shares data.
        /// </summary>
        public OutstandingShares OutstandingShares { get; set; }

        /// <summary>
        /// Gets or sets the earnings data.
        /// </summary>
        public Earnings Earnings { get; set; }

        /// <summary>
        /// Gets or sets the financial statements.
        /// </summary>
        public Financials Financials { get; set; }

        /// <summary>
        /// Gets or sets the technical indicators.
        /// </summary>
        public Technicals Technicals { get; set; }

        /// <summary>
        /// Gets or sets the ETF-specific data.
        /// </summary>
        [JsonPropertyName("ETF_Data")]
        public ETFData ETFData { get; set; }

        /// <summary>
        /// Gets or sets the index components, keyed by component index.
        /// </summary>
        public Dictionary<int, Component> Components { get; set; }

        /// <summary>
        /// Gets or sets the historical ticker components, keyed by component index.
        /// </summary>
        public Dictionary<int, HistoricalComponent> HistoricalTickerComponents { get; set; }

        /// <summary>
        /// Gets or sets the index statistics.
        /// </summary>
        public Statistics Statistics { get; set; }
    }
}
