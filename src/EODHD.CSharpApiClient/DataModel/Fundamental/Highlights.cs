using System;
using System.Text.Json.Serialization;

namespace EODHD.CSharpApiClient.DataModel.Fundamental
{
    /// <summary>
    /// Represents the financial highlights for an instrument.
    /// </summary>
    public sealed class Highlights
    {
        /// <summary>
        /// Gets or sets the market capitalization, as a currency amount.
        /// </summary>
        public double? MarketCapitalization { get; set; }

        /// <summary>
        /// Gets or sets the market capitalization in millions, as a currency amount.
        /// </summary>
        public double? MarketCapitalizationMln { get; set; }

        /// <summary>
        /// Gets or sets the EBITDA, as a currency amount.
        /// </summary>
        public long? EBITDA { get; set; }

        /// <summary>
        /// Gets or sets the price-to-earnings ratio.
        /// </summary>
        public double? PERatio { get; set; }

        /// <summary>
        /// Gets or sets the price/earnings-to-growth ratio.
        /// </summary>
        public double? PEGRatio { get; set; }

        /// <summary>
        /// Gets or sets the Wall Street target price.
        /// </summary>
        public double? WallStreetTargetPrice { get; set; }

        /// <summary>
        /// Gets or sets the book value per share, as a price.
        /// </summary>
        public double? BookValue { get; set; }

        /// <summary>
        /// Gets or sets the dividend per share, as a currency amount.
        /// </summary>
        public double? DividendShare { get; set; }

        /// <summary>
        /// Gets or sets the dividend yield, as a ratio.
        /// </summary>
        public double? DividendYield { get; set; }

        /// <summary>
        /// Gets or sets the earnings per share, as a currency amount.
        /// </summary>
        public double? EarningsShare { get; set; }

        /// <summary>
        /// Gets or sets the EPS estimate for the current year.
        /// </summary>
        public double? EPSEstimateCurrentYear { get; set; }

        /// <summary>
        /// Gets or sets the EPS estimate for the next year.
        /// </summary>
        public double? EPSEstimateNextYear { get; set; }

        /// <summary>
        /// Gets or sets the EPS estimate for the next quarter.
        /// </summary>
        public double? EPSEstimateNextQuarter { get; set; }

        /// <summary>
        /// Gets or sets the EPS estimate for the current quarter.
        /// </summary>
        public double? EPSEstimateCurrentQuarter { get; set; }

        /// <summary>
        /// Gets or sets the raw most-recent-quarter date string, as an ISO date.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("MostRecentQuarter")]
        private string MostRecentQuarterStr { get; set; }

        /// <summary>
        /// Gets the most-recent-quarter date, parsed from the raw string.
        /// </summary>
        [JsonIgnore]
        public DateTime? MostRecentQuarter => Utilities.ParseDate(this.MostRecentQuarterStr);

        /// <summary>
        /// Gets or sets the profit margin, as a ratio.
        /// </summary>
        public double? ProfitMargin { get; set; }

        /// <summary>
        /// Gets or sets the trailing-twelve-month operating margin, as a ratio.
        /// </summary>
        public double? OperatingMarginTTM { get; set; }

        /// <summary>
        /// Gets or sets the trailing-twelve-month return on assets, as a ratio.
        /// </summary>
        public double? ReturnOnAssetsTTM { get; set; }

        /// <summary>
        /// Gets or sets the trailing-twelve-month return on equity, as a ratio.
        /// </summary>
        public double? ReturnOnEquityTTM { get; set; }

        /// <summary>
        /// Gets or sets the trailing-twelve-month revenue, as a currency amount.
        /// </summary>
        public long? RevenueTTM { get; set; }

        /// <summary>
        /// Gets or sets the trailing-twelve-month revenue per share, as a currency amount.
        /// </summary>
        public double? RevenuePerShareTTM { get; set; }

        /// <summary>
        /// Gets or sets the year-over-year quarterly revenue growth, as a ratio.
        /// </summary>
        public double? QuarterlyRevenueGrowthYOY { get; set; }

        /// <summary>
        /// Gets or sets the trailing-twelve-month gross profit, as a currency amount.
        /// </summary>
        public long? GrossProfitTTM { get; set; }

        /// <summary>
        /// Gets or sets the trailing-twelve-month diluted EPS, as a currency amount.
        /// </summary>
        public double? DilutedEpsTTM { get; set; }

        /// <summary>
        /// Gets or sets the year-over-year quarterly earnings growth, as a ratio.
        /// </summary>
        public double? QuarterlyEarningsGrowthYOY { get; set; }
    }
}
