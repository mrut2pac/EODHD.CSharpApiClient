namespace EODHD.CSharpApiClient.DataModel.Fundamental
{
    /// <summary>
    /// Represents valuation ratios for a security.
    /// </summary>
    public sealed class Valuation
    {
        /// <summary>
        /// Gets or sets the trailing price-to-earnings ratio.
        /// </summary>
        public double? TrailingPE { get; set; }

        /// <summary>
        /// Gets or sets the forward price-to-earnings ratio.
        /// </summary>
        public double? ForwardPE { get; set; }

        /// <summary>
        /// Gets or sets the trailing-twelve-month price-to-sales ratio.
        /// </summary>
        public double? PriceSalesTTM { get; set; }

        /// <summary>
        /// Gets or sets the most-recent-quarter price-to-book ratio.
        /// </summary>
        public double? PriceBookMRQ { get; set; }

        /// <summary>
        /// Gets or sets the enterprise value (currency).
        /// </summary>
        public decimal? EnterpriseValue { get; set; }

        /// <summary>
        /// Gets or sets the enterprise-value-to-revenue ratio.
        /// </summary>
        public double? EnterpriseValueRevenue { get; set; }

        /// <summary>
        /// Gets or sets the enterprise-value-to-EBITDA ratio.
        /// </summary>
        public double? EnterpriseValueEbitda { get; set; }
    }
}
