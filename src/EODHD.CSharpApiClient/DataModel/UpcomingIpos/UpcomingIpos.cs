using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace EODHD.CSharpApiClient.DataModel.UpcomingIpos
{
    /// <summary>
    /// Represents the IPO calendar response for a date range.
    /// </summary>
    public sealed class UpcomingIpos
    {
        /// <summary>
        /// Gets or sets the response type.
        /// </summary>
        [JsonPropertyName("type")]
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the response description.
        /// </summary>
        [JsonPropertyName("description")]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the start of the covered date range.
        /// </summary>
        [JsonPropertyName("from")]
        public DateTime? From { get; set; }

        /// <summary>
        /// Gets or sets the end of the covered date range.
        /// </summary>
        [JsonPropertyName("to")]
        public DateTime? To { get; set; }

        /// <summary>
        /// Gets or sets the IPO entries.
        /// </summary>
        [JsonPropertyName("ipos")]
        public List<Ipo> Ipos { get; set; }
    }
}
