using System.Text.Json.Serialization;

namespace EODHD.CSharpApiClient.DataModel.Fundamental
{
    /// <summary>
    /// Represents the long/short/net allocation percentages for an ETF asset class.
    /// </summary>
    public sealed class AssetAllocationData
    {
        /// <summary>
        /// Gets or sets the long allocation (percent).
        /// </summary>
        [JsonPropertyName("Long_%")]
        public double? LongPercent { get; set; }

        /// <summary>
        /// Gets or sets the short allocation (percent).
        /// </summary>
        [JsonPropertyName("Short_%")]
        public double? ShortPercent { get; set; }

        /// <summary>
        /// Gets or sets the net assets allocation (percent).
        /// </summary>
        [JsonPropertyName("Net_Assets_%")]
        public double? NetAssetsPercent { get; set; }
    }
}
