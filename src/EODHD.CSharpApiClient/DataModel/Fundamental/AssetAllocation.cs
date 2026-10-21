using System.Text.Json.Serialization;

namespace EODHD.CSharpApiClient.DataModel.Fundamental
{
    /// <summary>
    /// Represents a single asset-allocation row of a mutual fund (e.g. <c>"US Stock"</c>, <c>"Bond"</c>).
    /// </summary>
    public sealed class AssetAllocation
    {
        /// <summary>
        /// Gets or sets the net allocation (percent).
        /// </summary>
        [JsonPropertyName("Net_%")]
        public double? NetPercentage { get; set; }

        /// <summary>
        /// Gets or sets the long allocation (percent).
        /// </summary>
        [JsonPropertyName("Long_%")]
        public double? LongPercentage { get; set; }

        /// <summary>
        /// Gets or sets the allocation type.
        /// </summary>
        [JsonPropertyName("Type")]
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the short allocation (percent).
        /// </summary>
        [JsonPropertyName("Short_%")]
        public double? ShortPercentage { get; set; }

        /// <summary>
        /// Gets or sets the category average allocation (percent).
        /// </summary>
        [JsonPropertyName("Category_Average")]
        public double? CategoryAverage { get; set; }

        /// <summary>
        /// Gets or sets the benchmark allocation (percent).
        /// </summary>
        [JsonPropertyName("Benchmark")]
        public double? Benchmark { get; set; }
    }
}
