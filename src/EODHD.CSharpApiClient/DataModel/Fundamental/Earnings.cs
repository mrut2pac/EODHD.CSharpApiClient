using System.Collections.Generic;

namespace EODHD.CSharpApiClient.DataModel.Fundamental
{
    /// <summary>
    /// Represents the earnings section of a company's fundamentals.
    /// </summary>
    public sealed class Earnings
    {
        /// <summary>
        /// Gets or sets the earnings history keyed by ISO date.
        /// </summary>
        public Dictionary<string, EarningsHistoryData> History { get; set; }

        /// <summary>
        /// Gets or sets the earnings trend keyed by ISO date.
        /// </summary>
        public Dictionary<string, EarningsTrendData> Trend { get; set; }

        /// <summary>
        /// Gets or sets the annual earnings keyed by ISO date.
        /// </summary>
        public Dictionary<string, EarningsAnnualData> Annual { get; set; }
    }
}
