using System.Text.Json.Serialization;

namespace EODHD.CSharpApiClient.DataModel.Fundamental
{
    /// <summary>
    /// Represents fixed-income allocation data for a fund.
    /// </summary>
    public sealed class FixedIncomeData
    {
        /// <summary>
        /// Gets or sets the fund allocation, as a percent.
        /// </summary>
        [JsonPropertyName("Fund_%")]
        public double? FundPercent { get; set; }

        /// <summary>
        /// Gets or sets the value relative to the category, as a ratio.
        /// </summary>
        [JsonPropertyName("Relative_to_Category")]
        public double? RelativeToCategory { get; set; }
    }
}
