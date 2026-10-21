using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

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
        /// Gets or sets the raw start of the requested date range string (ISO date).
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("from")]
        private string FromStr { get; set; }

        /// <summary>
        /// Gets the start of the requested date range, parsed from the raw value.
        /// </summary>
        [JsonIgnore]
        public DateTime? From => Utilities.ParseDate(this.FromStr);

        /// <summary>
        /// Gets or sets the raw end of the requested date range string (ISO date).
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("to")]
        private string ToStr { get; set; }

        /// <summary>
        /// Gets the end of the requested date range, parsed from the raw value.
        /// </summary>
        [JsonIgnore]
        public DateTime? To => Utilities.ParseDate(this.ToStr);

        /// <summary>
        /// Gets or sets the upcoming-earnings entries.
        /// </summary>
        public List<Earning> Earnings { get; set; }
    }
}
