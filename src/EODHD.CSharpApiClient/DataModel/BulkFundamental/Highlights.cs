namespace EODHD.CSharpApiClient.DataModel.BulkFundamental
{
    /// <summary>
    /// Represents the key financial highlights of a company's bulk fundamentals.
    /// </summary>
    public sealed class Highlights
    {
        /// <summary>
        /// Gets or sets the market capitalization (currency).
        /// </summary>
        public double? MarketCapitalization { get; set; }

        /// <summary>
        /// Gets or sets the market capitalization in millions (currency).
        /// </summary>
        public double? MarketCapitalizationMln { get; set; }

        /// <summary>
        /// Gets or sets the EBITDA (currency).
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
        /// Gets or sets the Wall Street target price (price).
        /// </summary>
        public double? WallStreetTargetPrice { get; set; }

        /// <summary>
        /// Gets or sets the book value per share (price).
        /// </summary>
        public double? BookValue { get; set; }

        /// <summary>
        /// Gets or sets the dividend per share (currency).
        /// </summary>
        public double? DividendShare { get; set; }

        /// <summary>
        /// Gets or sets the dividend yield (ratio).
        /// </summary>
        public double? DividendYield { get; set; }

        /// <summary>
        /// Gets or sets the earnings per share (currency).
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
        /// Gets or sets the most recent quarter (ISO date).
        /// </summary>
        public string MostRecentQuarter { get; set; }

        /// <summary>
        /// Gets or sets the profit margin (ratio).
        /// </summary>
        public double? ProfitMargin { get; set; }

        /// <summary>
        /// Gets or sets the trailing-twelve-month operating margin (ratio).
        /// </summary>
        public double? OperatingMarginTTM { get; set; }

        /// <summary>
        /// Gets or sets the trailing-twelve-month return on assets (ratio).
        /// </summary>
        public double? ReturnOnAssetsTTM { get; set; }

        /// <summary>
        /// Gets or sets the trailing-twelve-month return on equity (ratio).
        /// </summary>
        public double? ReturnOnEquityTTM { get; set; }

        /// <summary>
        /// Gets or sets the trailing-twelve-month revenue (currency).
        /// </summary>
        public long? RevenueTTM { get; set; }

        /// <summary>
        /// Gets or sets the trailing-twelve-month revenue per share (currency).
        /// </summary>
        public double? RevenuePerShareTTM { get; set; }

        /// <summary>
        /// Gets or sets the year-over-year quarterly revenue growth (ratio).
        /// </summary>
        public double? QuarterlyRevenueGrowthYOY { get; set; }

        /// <summary>
        /// Gets or sets the trailing-twelve-month gross profit (currency).
        /// </summary>
        public long? GrossProfitTTM { get; set; }

        /// <summary>
        /// Gets or sets the trailing-twelve-month diluted earnings per share (currency).
        /// </summary>
        public double? DilutedEpsTTM { get; set; }

        /// <summary>
        /// Gets or sets the year-over-year quarterly earnings growth (ratio).
        /// </summary>
        public double? QuarterlyEarningsGrowthYOY { get; set; }
    }
}
