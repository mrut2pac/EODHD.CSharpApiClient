using System.Text.Json.Serialization;

namespace EODHD.CSharpApiClient.DataModel.Bonds
{
    /// <summary>
    /// Classification metadata for a bond (type, debt category, and industry grouping).
    /// </summary>
    public sealed class BondClassificationData
    {
        /// <summary>
        /// Gets or sets the bond type (e.g. <c>"Corporate bonds Germany"</c>).
        /// </summary>
        [JsonPropertyName("BondType")]
        public string BondType { get; set; }

        /// <summary>
        /// Gets or sets the debt type.
        /// </summary>
        [JsonPropertyName("DebtType")]
        public string DebtType { get; set; }

        /// <summary>
        /// Gets or sets the issuer's industry group (e.g. <c>"Banks"</c>).
        /// </summary>
        [JsonPropertyName("IndustryGroup")]
        public string IndustryGroup { get; set; }

        /// <summary>
        /// Gets or sets the issuer's industry sub-group.
        /// </summary>
        [JsonPropertyName("IndustrySubGroup")]
        public string IndustrySubGroup { get; set; }

        /// <summary>
        /// Gets or sets the sub-product asset classification.
        /// </summary>
        [JsonPropertyName("SubProductAsset")]
        public string SubProductAsset { get; set; }

        /// <summary>
        /// Gets or sets the sub-product asset type.
        /// </summary>
        [JsonPropertyName("SubProductAssetType")]
        public string SubProductAssetType { get; set; }
    }
}
