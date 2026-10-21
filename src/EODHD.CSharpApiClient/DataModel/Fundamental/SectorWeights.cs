using System.Text.Json.Serialization;

namespace EODHD.CSharpApiClient.DataModel.Fundamental
{
    /// <summary>
    /// Represents a sector weighting entry of a mutual fund, within a sector group (e.g. <c>"Cyclical"</c>, <c>"Bond Sector"</c>).
    /// </summary>
    public sealed class SectorWeights
    {
        /// <summary>
        /// Gets or sets the sector name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the category average weighting (percent).
        /// </summary>
        [JsonPropertyName("Category_Average")]
        public double? CategoryAverage { get; set; }

        /// <summary>
        /// Gets or sets the weighting amount (percent).
        /// </summary>
        [JsonPropertyName("Amount_%")]
        public double? AmountPercent { get; set; }

        /// <summary>
        /// Gets or sets the stocks weighting (percent); sent for bond-sector entries.
        /// </summary>
        [JsonPropertyName("Stocks_%")]
        public double? StocksPercent { get; set; }

        /// <summary>
        /// Gets or sets the benchmark weighting (percent).
        /// </summary>
        public double? Benchmark { get; set; }
    }
}
