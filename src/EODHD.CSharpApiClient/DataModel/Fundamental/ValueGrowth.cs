using System.Text.Json.Serialization;

namespace EODHD.CSharpApiClient.DataModel.Fundamental
{
    /// <summary>
    /// Represents a value/growth metric with portfolio, benchmark, and category comparisons.
    /// </summary>
    public sealed class ValueGrowth
    {
        /// <summary>
        /// Gets or sets the metric name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the category average value.
        /// </summary>
        [JsonPropertyName("Category_Average")]
        public double? CategoryAverage { get; set; }

        /// <summary>
        /// Gets or sets the benchmark value.
        /// </summary>
        public double? Benchmark { get; set; }

        /// <summary>
        /// Gets or sets the stock portfolio value.
        /// </summary>
        [JsonPropertyName("Stock_Portfolio")]
        public double? StockPortfolio { get; set; }
    }
}
