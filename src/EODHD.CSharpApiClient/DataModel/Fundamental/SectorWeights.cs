using System.Text.Json.Serialization;

namespace EODHD.CSharpApiClient.DataModel.Fundamental
{
    /// <summary>
    /// Represents a sector weighting entry with its category benchmark.
    /// </summary>
    public sealed class SectorWeights
    {
        /// <summary>
        /// Gets or sets the sector type.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the category average weighting (percent).
        /// </summary>
        [JsonPropertyName("Category_Average")]
        public double? CategoryAverage { get; set; }

        /// <summary>
        /// Gets or sets the weighting amount (percent).
        /// </summary>
        public double? AmountPercent { get; set; }

        /// <summary>
        /// Gets or sets the benchmark weighting (percent).
        /// </summary>
        public double? Benchmark { get; set; }
    }
}
