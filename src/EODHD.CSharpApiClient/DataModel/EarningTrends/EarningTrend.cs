using System.Collections.Generic;

namespace EODHD.CSharpApiClient.DataModel.EarningTrends
{
    /// <summary>
    /// Represents the earning-trend response for one or more symbols.
    /// </summary>
    public sealed class EarningTrend
    {
        /// <summary>
        /// Gets or sets the response type.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the response description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the symbols covered by the response.
        /// </summary>
        public string Symbols { get; set; }

        /// <summary>
        /// Gets or sets the per-symbol earning trends.
        /// </summary>
        public List<List<Trend>> Trends { get; set; }
    }
}
