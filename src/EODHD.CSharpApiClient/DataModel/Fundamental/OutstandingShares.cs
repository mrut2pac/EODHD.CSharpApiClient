using System.Collections.Generic;

namespace EODHD.CSharpApiClient.DataModel.Fundamental
{
    /// <summary>
    /// Represents the annual and quarterly outstanding-shares history.
    /// </summary>
    public sealed class OutstandingShares
    {
        /// <summary>
        /// Gets or sets the annual outstanding-shares data, keyed by period.
        /// </summary>
        public Dictionary<string, OutstandingSharesData> Annual { get; set; }

        /// <summary>
        /// Gets or sets the quarterly outstanding-shares data, keyed by period.
        /// </summary>
        public Dictionary<string, OutstandingSharesData> Quarterly { get; set; }
    }
}
