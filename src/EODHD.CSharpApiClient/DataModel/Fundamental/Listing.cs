namespace EODHD.CSharpApiClient.DataModel.Fundamental
{
    /// <summary>
    /// Represents an exchange listing for an instrument.
    /// </summary>
    public sealed class Listing
    {
        /// <summary>
        /// Gets or sets the listing code.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets the exchange code.
        /// </summary>
        public string Exchange { get; set; }

        /// <summary>
        /// Gets or sets the listing name.
        /// </summary>
        public string Name { get; set; }
    }
}
