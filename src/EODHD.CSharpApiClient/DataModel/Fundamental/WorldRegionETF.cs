using System.Text.Json.Serialization;

namespace EODHD.CSharpApiClient.DataModel.Fundamental
{
    /// <summary>
    /// Represents the per-world-region equity allocation for an ETF.
    /// </summary>
    public sealed class WorldRegionETF
    {
        /// <summary>
        /// Gets or sets the North America region allocation.
        /// </summary>
        [JsonPropertyName("North America")]
        public WorldData NorthAmerica { get; set; }

        /// <summary>
        /// Gets or sets the United Kingdom region allocation.
        /// </summary>
        [JsonPropertyName("United Kingdom")]
        public WorldData UnitedKingdom { get; set; }

        /// <summary>
        /// Gets or sets the developed Europe region allocation.
        /// </summary>
        [JsonPropertyName("Europe Developed")]
        public WorldData EuropeDeveloped { get; set; }

        /// <summary>
        /// Gets or sets the emerging Europe region allocation.
        /// </summary>
        [JsonPropertyName("Europe Emerging")]
        public WorldData EuropeEmerging { get; set; }

        /// <summary>
        /// Gets or sets the Africa / Middle East region allocation.
        /// </summary>
        [JsonPropertyName("Africa/Middle East")]
        public WorldData AfricaMiddleEast { get; set; }

        /// <summary>
        /// Gets or sets the Japan region allocation.
        /// </summary>
        public WorldData Japan { get; set; }

        /// <summary>
        /// Gets or sets the Australasia region allocation.
        /// </summary>
        public WorldData Australasia { get; set; }

        /// <summary>
        /// Gets or sets the developed Asia region allocation.
        /// </summary>
        [JsonPropertyName("Asia Developed")]
        public WorldData AsiaDeveloped { get; set; }

        /// <summary>
        /// Gets or sets the emerging Asia region allocation.
        /// </summary>
        [JsonPropertyName("Asia Emerging")]
        public WorldData AsiaEmerging { get; set; }

        /// <summary>
        /// Gets or sets the Latin America region allocation.
        /// </summary>
        [JsonPropertyName("Latin America")]
        public WorldData LatinAmerica { get; set; }
    }
}
