using System;
using System.Text.Json.Serialization;

namespace EODHD.CSharpApiClient.DataModel.Fundamental
{
    /// <summary>
    /// Represents a single holder's position in an instrument.
    /// </summary>
    public sealed class HoldersData
    {
        /// <summary>
        /// Gets or sets the holder name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the raw position date string, as an ISO date.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("Date")]
        private string DateStr { get; set; }

        /// <summary>
        /// Gets the position date, parsed from the raw string.
        /// </summary>
        [JsonIgnore]
        public DateTime? Date => Utilities.ParseDate(this.DateStr);

        /// <summary>
        /// Gets or sets the total shares held, as a count.
        /// </summary>
        public double? TotalShares { get; set; }

        /// <summary>
        /// Gets or sets the total assets held, as a currency amount.
        /// </summary>
        public double? TotalAssets { get; set; }

        /// <summary>
        /// Gets or sets the current shares held, as a count.
        /// </summary>
        public decimal? CurrentShares { get; set; }

        /// <summary>
        /// Gets or sets the change in shares, as a count.
        /// </summary>
        public decimal? Change { get; set; }

        /// <summary>
        /// Gets or sets the change in shares, as a percent.
        /// </summary>
        [JsonPropertyName("Change_p")]
        public double? ChangePercent { get; set; }
    }
}
