using System.Text.Json.Serialization;

namespace EODHD.CSharpApiClient.DataModel.Fundamental
{
    /// <summary>
    /// Represents a single asset-allocation row for an ETF holding category.
    /// </summary>
    public sealed class AssetAllocation
    {
        /// <summary>
        /// Gets or sets the net allocation (percent).
        /// </summary>
        [JsonPropertyName("Net_%")]
        public string NetPercentage { get; set; }

        /// <summary>
        /// Gets or sets the long allocation (percent).
        /// </summary>
        [JsonPropertyName("Long_%")]
        public string LongPercentage { get; set; }

        /// <summary>
        /// Gets or sets the allocation type.
        /// </summary>
        [JsonPropertyName("Type")]
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the short allocation (percent).
        /// </summary>
        [JsonPropertyName("Short_%")]
        public string ShortPercentage { get; set; }

        /// <summary>
        /// Gets or sets the category average allocation (percent).
        /// </summary>
        [JsonPropertyName("Category_Average")]
        public string CategoryAverage { get; set; }

        /// <summary>
        /// Gets or sets the benchmark allocation (percent).
        /// </summary>
        [JsonPropertyName("Benchmark")]
        public string Benchmark { get; set; }
    }
}
