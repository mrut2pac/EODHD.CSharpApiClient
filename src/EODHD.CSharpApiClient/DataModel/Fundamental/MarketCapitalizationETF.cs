namespace EODHD.CSharpApiClient.DataModel.Fundamental
{
    /// <summary>
    /// Represents market-capitalization bucket allocations for an ETF.
    /// </summary>
    public sealed class MarketCapitalizationETF
    {
        /// <summary>
        /// Gets or sets the mega-cap allocation, as a percent.
        /// </summary>
        public string Mega { get; set; }

        /// <summary>
        /// Gets or sets the big-cap allocation, as a percent.
        /// </summary>
        public string Big { get; set; }

        /// <summary>
        /// Gets or sets the medium-cap allocation, as a percent.
        /// </summary>
        public string Medium { get; set; }

        /// <summary>
        /// Gets or sets the small-cap allocation, as a percent.
        /// </summary>
        public string Small { get; set; }

        /// <summary>
        /// Gets or sets the micro-cap allocation, as a percent.
        /// </summary>
        public string Micro { get; set; }
    }
}
