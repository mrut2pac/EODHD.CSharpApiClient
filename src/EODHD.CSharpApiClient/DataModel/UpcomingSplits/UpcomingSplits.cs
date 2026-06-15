using System;
using System.Collections.Generic;

namespace EODHD.CSharpApiClient.DataModel.UpcomingSplits
{
    /// <summary>
    /// Represents the upcoming-splits response for a date range.
    /// </summary>
    public sealed class UpcomingSplits
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
        /// Gets or sets the start of the covered date range.
        /// </summary>
        public DateTime? From { get; set; }

        /// <summary>
        /// Gets or sets the end of the covered date range.
        /// </summary>
        public DateTime? To { get; set; }

        /// <summary>
        /// Gets or sets the upcoming-split entries.
        /// </summary>
        public List<Split> Splits { get; set; }
    }
}
