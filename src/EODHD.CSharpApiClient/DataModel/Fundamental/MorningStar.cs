using System.Text.Json.Serialization;

namespace EODHD.CSharpApiClient.DataModel.Fundamental
{
    /// <summary>
    /// Represents Morningstar rating data for a fund.
    /// </summary>
    public sealed class MorningStar
    {
        /// <summary>
        /// Gets or sets the Morningstar ratio.
        /// </summary>
        public double? Ratio { get; set; }

        /// <summary>
        /// Gets or sets the category benchmark.
        /// </summary>
        [JsonPropertyName("Category_Benchmark")]
        public string CategoryBenchmark { get; set; }

        /// <summary>
        /// Gets or sets the sustainability ratio.
        /// </summary>
        [JsonPropertyName("Sustainability_Ratio")]
        public string SustainabilityRatio { get; set; }
    }
}
