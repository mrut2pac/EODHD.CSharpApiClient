using System.Collections.Generic;

namespace EODHD.CSharpApiClient.DataModel.Fundamental
{
    /// <summary>
    /// Represents the technical section of a crypto asset's fundamentals (<c>Tech</c>).
    /// </summary>
    public sealed class CryptoTech
    {
        /// <summary>
        /// Gets or sets the developers, keyed by ordinal index (e.g. <c>"Satoshi Nakamoto - Founder"</c>).
        /// </summary>
        public Dictionary<int, string> Developers { get; set; }
    }
}
