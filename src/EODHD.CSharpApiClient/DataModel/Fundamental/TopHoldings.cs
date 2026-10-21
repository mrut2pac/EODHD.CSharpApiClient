namespace EODHD.CSharpApiClient.DataModel.Fundamental
{
    /// <summary>
    /// Represents a single top-holding entry of a mutual fund.
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
        /// Gets or sets the holding weight as sent by the API, with a percent sign (e.g. <c>"4.10%"</c>).
        /// </summary>
        public string Weight { get; set; }
    }
}
