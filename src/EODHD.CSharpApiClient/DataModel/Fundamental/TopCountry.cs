using System.Text.Json.Serialization;

namespace EODHD.CSharpApiClient.DataModel.Fundamental
{
    /// <summary>
    /// Represents a single top-country exposure entry of a mutual fund.
    /// </summary>
    public sealed class TopCountry
    {
        /// <summary>
        /// Gets or sets the country name. Placeholder rows carry <c>"0.00"</c> here.
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// Gets or sets the exposure to the country (percent).
        /// </summary>
        [JsonPropertyName("Amount_%")]
        public double? AmountPercent { get; set; }

        /// <summary>
        /// Gets or sets the category average exposure (percent).
        /// </summary>
        [JsonPropertyName("Category_Average")]
        public double? CategoryAverage { get; set; }

        /// <summary>
        /// Gets or sets the benchmark exposure (percent).
        /// </summary>
        public double? Benchmark { get; set; }
    }
}
