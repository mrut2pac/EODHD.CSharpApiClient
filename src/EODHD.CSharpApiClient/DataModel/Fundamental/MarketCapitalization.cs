using System.Text.Json.Serialization;

namespace EODHD.CSharpApiClient.DataModel.Fundamental
{
    /// <summary>
    /// Represents a market-capitalization size bucket of a mutual fund.
    /// </summary>
    public sealed class MarketCapitalization
    {
        /// <summary>
        /// Gets or sets the market-capitalization size bucket.
        /// </summary>
        public string Size { get; set; }

        /// <summary>
        /// Gets or sets the category average, as a percent.
        /// </summary>
        [JsonPropertyName("Category_Average")]
        public double? CategoryAverage { get; set; }

        /// <summary>
        /// Gets or sets the benchmark value, as a percent.
        /// </summary>
        public double? Benchmark { get; set; }

        /// <summary>
        /// Gets or sets the portfolio allocation, as a percent.
        /// </summary>
        [JsonPropertyName("Portfolio_%")]
        public double? PortfolioPercent { get; set; }
    }
}
