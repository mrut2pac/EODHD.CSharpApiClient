using System;
using System.Text.Json.Serialization;

namespace EODHD.CSharpApiClient.DataModel.Fundamental
{
    /// <summary>
    /// Represents a single outstanding-shares observation for a period.
    /// </summary>
    public sealed class OutstandingSharesData
    {
        /// <summary>
        /// Gets or sets the period label.
        /// </summary>
        public string Date { get; set; }

        /// <summary>
        /// Gets or sets the raw formatted date string, as an ISO date.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("DateFormatted")]
        private string DateFormattedStr { get; set; }

        /// <summary>
        /// Gets the formatted date, parsed from the raw string.
        /// </summary>
        [JsonIgnore]
        public DateTime? DateFormatted => Utilities.ParseDate(this.DateFormattedStr);

        /// <summary>
        /// Gets or sets the number of shares in millions, as a count.
        /// </summary>
        public string SharesMln { get; set; }

        /// <summary>
        /// Gets or sets the number of outstanding shares, as a count.
        /// </summary>
        public decimal? Shares { get; set; }
    }
}
