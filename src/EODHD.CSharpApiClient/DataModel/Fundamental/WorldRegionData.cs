using System.Text.Json.Serialization;

namespace EODHD.CSharpApiClient.DataModel.Fundamental
{
    /// <summary>
    /// Represents the equity allocation for a world region relative to its category.
    /// </summary>
    public sealed class WorldData
    {
        /// <summary>
        /// Gets or sets the equity allocation (percent).
        /// </summary>
        [JsonPropertyName("Equity_%")]
        public double? EquityPercent { get; set; }

        /// <summary>
        /// Gets or sets the allocation relative to the category (percent).
        /// </summary>
        [JsonPropertyName("Relative_to_Category")]
        public double? RelativeToCategory { get; set; }
    }
}
