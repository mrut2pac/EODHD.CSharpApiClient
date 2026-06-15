namespace EODHD.CSharpApiClient.DataModel.Fundamental
{
    /// <summary>
    /// Represents the aggregated analyst ratings for a security.
    /// </summary>
    public sealed class AnalystRatings
    {
        /// <summary>
        /// Gets or sets the consensus rating (numeric scale).
        /// </summary>
        public double? Rating { get; set; }

        /// <summary>
        /// Gets or sets the consensus target price.
        /// </summary>
        public decimal? TargetPrice { get; set; }

        /// <summary>
        /// Gets or sets the number of strong-buy ratings (count).
        /// </summary>
        public int? StrongBuy { get; set; }

        /// <summary>
        /// Gets or sets the number of buy ratings (count).
        /// </summary>
        public int? Buy { get; set; }

        /// <summary>
        /// Gets or sets the number of hold ratings (count).
        /// </summary>
        public int? Hold { get; set; }

        /// <summary>
        /// Gets or sets the number of sell ratings (count).
        /// </summary>
        public int? Sell { get; set; }

        /// <summary>
        /// Gets or sets the number of strong-sell ratings (count).
        /// </summary>
        public int? StrongSell { get; set; }
    }
}
