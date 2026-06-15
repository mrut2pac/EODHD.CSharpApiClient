namespace EODHD.CSharpApiClient.DataModel
{
    /// <summary>
    /// Describes a single stock exchange returned by the EODHD exchanges list API.
    /// </summary>
    public sealed class Exchange
    {
        /// <summary>
        /// Gets or sets the exchange display name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the EODHD exchange code (e.g. <c>"US"</c>).
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets the operating Market Identifier Code (MIC).
        /// </summary>
        public string OperatingMIC { get; set; }

        /// <summary>
        /// Gets or sets the country the exchange operates in.
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// Gets or sets the trading currency code.
        /// </summary>
        public string Currency { get; set; }

        /// <summary>
        /// Gets or sets the two-letter ISO country code.
        /// </summary>
        public string CountryISO2 { get; set; }

        /// <summary>
        /// Gets or sets the three-letter ISO country code.
        /// </summary>
        public string CountryISO3 { get; set; }
    }
}
