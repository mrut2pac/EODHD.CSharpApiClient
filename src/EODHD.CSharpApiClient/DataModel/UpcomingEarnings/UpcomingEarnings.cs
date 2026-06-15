using System.Collections.Generic;

namespace EODHD.CSharpApiClient.DataModel.UpcomingEarnings
{
    /// <summary>
    /// Represents the upcoming-earnings response for one or more symbols.
    /// </summary>
    public sealed class UpcomingEarnings
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
        /// Gets or sets the upcoming-earnings entries.
        /// </summary>
        public List<Earning> Earnings { get; set; }
    }
}
