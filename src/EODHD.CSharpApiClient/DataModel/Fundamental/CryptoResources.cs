using System.Collections.Generic;

namespace EODHD.CSharpApiClient.DataModel.Fundamental
{
    /// <summary>
    /// Represents the resources section of a crypto asset's fundamentals (<c>Resources</c>).
    /// </summary>
    public sealed class CryptoResources
    {
        /// <summary>
        /// Gets or sets the project links keyed by kind (e.g. <c>"website"</c>, <c>"explorer"</c>), then by ordinal
        /// index. A kind with no links may be <c>null</c>.
        /// </summary>
        public Dictionary<string, Dictionary<int, string>> Links { get; set; }

        /// <summary>
        /// Gets or sets the URL of the asset's thumbnail image.
        /// </summary>
        public string Thumbnail { get; set; }
    }
}
