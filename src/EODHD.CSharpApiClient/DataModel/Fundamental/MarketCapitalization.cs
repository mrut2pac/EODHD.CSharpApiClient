namespace EODHD.CSharpApiClient.DataModel.Fundamental
{
    /// <summary>
    /// Represents market-capitalization allocation data for a fund.
    /// </summary>
    public sealed class MarketCapitalization
    {
        /// <summary>
        /// Gets or sets the market-capitalization size bucket.
        /// </summary>
        public string Size { get; set; }

        /// <summary>
        /// Gets or sets the category average, as a percent.
        /// </summary>
        public double? Category_Average { get; set; }

        /// <summary>
        /// Gets or sets the benchmark value, as a percent.
        /// </summary>
        public double? Benchmark { get; set; }

        /// <summary>
        /// Gets or sets the portfolio allocation, as a percent.
        /// </summary>
        public double? PortfolioPercent { get; set; }
    }
}
