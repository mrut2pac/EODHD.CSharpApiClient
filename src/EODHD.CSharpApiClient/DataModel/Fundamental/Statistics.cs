namespace EODHD.CSharpApiClient.DataModel.Fundamental
{
    /// <summary>
    /// Represents market-capitalization and supply statistics (primarily for crypto assets).
    /// </summary>
    public sealed class Statistics
    {
        /// <summary>
        /// Gets or sets the market capitalization (currency).
        /// </summary>
        public decimal? MarketCapitalization { get; set; }

        /// <summary>
        /// Gets or sets the diluted market capitalization (currency).
        /// </summary>
        public decimal? MarketCapitalizationDiluted { get; set; }

        /// <summary>
        /// Gets or sets the circulating supply, as a count.
        /// </summary>
        public long? CirculatingSupply { get; set; }

        /// <summary>
        /// Gets or sets the total supply, as a count.
        /// </summary>
        public long? TotalSupply { get; set; }

        /// <summary>
        /// Gets or sets the market-cap dominance (percent).
        /// </summary>
        public double? MarketCapDominance { get; set; }

        /// <summary>
        /// Gets or sets the technical documentation URL.
        /// </summary>
        public string TechnicalDoc { get; set; }

        /// <summary>
        /// Gets or sets the block explorer URL.
        /// </summary>
        public string Explorer { get; set; }

        /// <summary>
        /// Gets or sets the source-code URL.
        /// </summary>
        public string SourceCode { get; set; }

        /// <summary>
        /// Gets or sets the message-board URL.
        /// </summary>
        public string MessageBoard { get; set; }

        /// <summary>
        /// Gets or sets the all-time low price.
        /// </summary>
        public decimal LowAllTime { get; set; }

        /// <summary>
        /// Gets or sets the all-time high price.
        /// </summary>
        public decimal HighAllTime { get; set; }
    }
}
