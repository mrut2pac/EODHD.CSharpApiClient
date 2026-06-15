namespace EODHD.CSharpApiClient.DataModel.Fundamental
{
    /// <summary>
    /// Represents a single top-holding entry within an ETF or fund.
    /// </summary>
    public sealed class TopHoldings
    {
        /// <summary>
        /// Gets or sets the holding name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the amount owned.
        /// </summary>
        public string Owned { get; set; }

        /// <summary>
        /// Gets or sets the change in the holding.
        /// </summary>
        public string Change { get; set; }

        /// <summary>
        /// Gets or sets the holding weight (percent).
        /// </summary>
        public double? Weight { get; set; }
    }
}
