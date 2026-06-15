using System.Text.Json.Serialization;

namespace EODHD.CSharpApiClient.DataModel.Fundamental
{
    /// <summary>
    /// Represents the asset allocation of an ETF broken down by asset class.
    /// </summary>
    public sealed class AssetAllocationETF
    {
        /// <summary>
        /// Gets or sets the cash allocation.
        /// </summary>
        public AssetAllocationData Cash { get; set; }

        /// <summary>
        /// Gets or sets the not-classified allocation.
        /// </summary>
        public AssetAllocationData NotClassified { get; set; }

        /// <summary>
        /// Gets or sets the non-US stock allocation.
        /// </summary>
        [JsonPropertyName("Stock non-US")]
        public AssetAllocationData StockNonUS { get; set; }

        /// <summary>
        /// Gets or sets the other allocation.
        /// </summary>
        public AssetAllocationData Other { get; set; }

        /// <summary>
        /// Gets or sets the US stock allocation.
        /// </summary>
        [JsonPropertyName("Stock US")]
        public AssetAllocationData StockUS { get; set; }

        /// <summary>
        /// Gets or sets the bond allocation.
        /// </summary>
        public AssetAllocationData Bond { get; set; }
    }
}
