using System;
using System.Text.Json.Serialization;

using EODHD.CSharpApiClient.JsonSupport;

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
        /// Gets or sets the date the share count applies to.
        /// </summary>
        [JsonPropertyName("DateFormatted")]
        [JsonConverter(typeof(NaTolerantDateConverter))]
        public DateTime? DateFormatted { get; set; }

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
