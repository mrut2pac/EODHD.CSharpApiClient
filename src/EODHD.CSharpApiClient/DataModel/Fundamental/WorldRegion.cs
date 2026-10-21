using System.Text.Json.Serialization;

namespace EODHD.CSharpApiClient.DataModel.Fundamental
{
    /// <summary>
    /// Represents a world-region weighting entry of a mutual fund, within a region group (e.g. <c>"Americas"</c>).
    /// </summary>
    public sealed class WorldRegion
    {
        /// <summary>
        /// Gets or sets the region name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the category average weighting (percent).
        /// </summary>
        [JsonPropertyName("Category_Average")]
        public double? CategoryAverage { get; set; }

        /// <summary>
        /// Gets or sets the stocks weighting for the region (percent).
        /// </summary>
        [JsonPropertyName("Stocks_%")]
        public double? StocksPercent { get; set; }

        /// <summary>
        /// Gets or sets the benchmark weighting (percent).
        /// </summary>
        public double? Benchmark { get; set; }
    }
}
