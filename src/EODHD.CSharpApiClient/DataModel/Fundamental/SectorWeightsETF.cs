using System.Text.Json.Serialization;

namespace EODHD.CSharpApiClient.DataModel.Fundamental
{
    /// <summary>
    /// Represents the per-sector equity allocation for an ETF.
    /// </summary>
    public sealed class SectorWeightETF
    {
        /// <summary>
        /// Gets or sets the basic materials sector allocation.
        /// </summary>
        [JsonPropertyName("Basic Materials")]
        public WorldData BasicMaterials { get; set; }

        /// <summary>
        /// Gets or sets the consumer cyclicals sector allocation.
        /// </summary>
        [JsonPropertyName("Consumer Cyclicals")]
        public WorldData ConsumerCyclicals { get; set; }

        /// <summary>
        /// Gets or sets the financial services sector allocation.
        /// </summary>
        [JsonPropertyName("Financial Services")]
        public WorldData FinancialServices { get; set; }

        /// <summary>
        /// Gets or sets the energy sector allocation.
        /// </summary>
        public WorldData Energy { get; set; }

        /// <summary>
        /// Gets or sets the industrials sector allocation.
        /// </summary>
        public WorldData Industrials { get; set; }

        /// <summary>
        /// Gets or sets the technology sector allocation.
        /// </summary>
        public WorldData Technology { get; set; }

        /// <summary>
        /// Gets or sets the consumer defensive sector allocation.
        /// </summary>
        [JsonPropertyName("Consumer Defensive")]
        public WorldData ConsumerDefensive { get; set; }

        /// <summary>
        /// Gets or sets the health care sector allocation.
        /// </summary>
        public WorldData HealthCare { get; set; }

        /// <summary>
        /// Gets or sets the utilities sector allocation.
        /// </summary>
        public WorldData Utilities { get; set; }
    }
}
