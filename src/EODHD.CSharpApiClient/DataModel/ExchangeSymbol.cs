namespace EODHD.CSharpApiClient.DataModel
{
    /// <summary>
    /// Represents a single tradable symbol listed on an exchange.
    /// </summary>
    public sealed class ExchangeSymbol
    {
        /// <summary>
        /// Gets or sets the ticker code.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets the instrument display name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the country of listing.
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// Gets or sets the exchange code the symbol is listed on.
        /// </summary>
        public string Exchange { get; set; }

        /// <summary>
        /// Gets or sets the trading currency code.
        /// </summary>
        public string Currency { get; set; }

        /// <summary>
        /// Gets or sets the instrument type (e.g. <c>"Common Stock"</c>).
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the International Securities Identification Number (ISIN).
        /// </summary>
        public string Isin { get; set; }
    }
}
