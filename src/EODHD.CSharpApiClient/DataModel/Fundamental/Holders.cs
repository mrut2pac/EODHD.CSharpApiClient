using System.Collections.Generic;

namespace EODHD.CSharpApiClient.DataModel.Fundamental
{
    /// <summary>
    /// Represents the institutional and fund holders of an instrument.
    /// </summary>
    public sealed class Holders
    {
        /// <summary>
        /// Gets or sets the institutional holders, keyed by holder identifier.
        /// </summary>
        public Dictionary<string, HoldersData> Institutions { get; set; }

        /// <summary>
        /// Gets or sets the fund holders, keyed by holder identifier.
        /// </summary>
        public Dictionary<string, HoldersData> Funds { get; set; }
    }
}
