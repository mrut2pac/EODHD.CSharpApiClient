using System;
using System.Text.Json.Serialization;

using EODHD.CSharpApiClient.JsonSupport;

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
        /// Gets or sets the position date.
        /// </summary>
        [JsonPropertyName("Date")]
        [JsonConverter(typeof(NaTolerantDateConverter))]
        public DateTime? Date { get; set; }

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
