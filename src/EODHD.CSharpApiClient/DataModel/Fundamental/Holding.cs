using System.Text.Json.Serialization;

namespace EODHD.CSharpApiClient.DataModel.Fundamental
{
    /// <summary>
    /// Represents a single holding within a fund or ETF.
    /// </summary>
    public sealed class Holding
    {
        /// <summary>
        /// Gets or sets the holding code.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets the exchange code.
        /// </summary>
        public string Exchange { get; set; }

        /// <summary>
        /// Gets or sets the holding name.
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
        /// Gets or sets the country.
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// Gets or sets the region.
        /// </summary>
        public string Region { get; set; }

        /// <summary>
        /// Gets or sets the share of assets, as a percent.
        /// </summary>
        [JsonPropertyName("Assets_%")]
        public double? AssetsPercent { get; set; }
    }
}
