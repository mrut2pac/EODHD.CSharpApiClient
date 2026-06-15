namespace EODHD.CSharpApiClient.DataModel.Fundamental
{
    /// <summary>
    /// Represents a single index constituent (component) entry.
    /// </summary>
    public sealed class Component
    {
        /// <summary>
        /// Gets or sets the symbol code.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets the exchange code.
        /// </summary>
        public string Exchange { get; set; }

        /// <summary>
        /// Gets or sets the company name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the sector.
        /// </summary>
        public string Sector { get; set; }

        /// <summary>
        /// Gets or sets the industry.
        /// </summary>
        public string Industry { get; set; }
    }
}
