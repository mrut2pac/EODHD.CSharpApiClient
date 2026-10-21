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

        /// <summary>
        /// Gets or sets the constituent's weight in the index, as a fraction (e.g. <c>0.0746</c> for 7.46%).
        /// </summary>
        public double? Weight { get; set; }
    }
}
