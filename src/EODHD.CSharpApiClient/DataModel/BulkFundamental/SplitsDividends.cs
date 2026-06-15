using System;
using System.Text.Json.Serialization;

namespace EODHD.CSharpApiClient.DataModel.BulkFundamental
{
    /// <summary>
    /// Represents the splits and dividends section of a company's bulk fundamentals.
    /// </summary>
    public sealed class SplitsDividends
    {
        /// <summary>
        /// Gets or sets the forward annual dividend rate (currency).
        /// </summary>
        public double? ForwardAnnualDividendRate { get; set; }

        /// <summary>
        /// Gets or sets the forward annual dividend yield (ratio).
        /// </summary>
        public double? ForwardAnnualDividendYield { get; set; }

        /// <summary>
        /// Gets or sets the payout ratio.
        /// </summary>
        public double? PayoutRatio { get; set; }

        /// <summary>
        /// Gets or sets the raw dividend date string as returned by the API.
        /// Marked <see cref="JsonIncludeAttribute"/> so the private setter is populated during deserialization.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("DividendDate")]
        private string DividendDateStr { get; set; }

        /// <summary>
        /// Gets the parsed dividend date (ISO date), or <c>null</c> if unavailable.
        /// </summary>
        [JsonIgnore]
        public DateTime? DividendDate => Utilities.ParseDate(this.DividendDateStr);

        /// <summary>
        /// Gets or sets the raw ex-dividend date string as returned by the API.
        /// Marked <see cref="JsonIncludeAttribute"/> so the private setter is populated during deserialization.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("ExDividendDate")]
        private string ExDividendDateStr { get; set; }

        /// <summary>
        /// Gets the parsed ex-dividend date (ISO date), or <c>null</c> if unavailable.
        /// </summary>
        [JsonIgnore]
        public DateTime? ExDividendDate => Utilities.ParseDate(this.ExDividendDateStr);

        /// <summary>
        /// Gets or sets the raw last-split-factor string as returned by the API (e.g. <c>"2:1"</c>).
        /// </summary>
        [JsonPropertyName("LastSplitFactor")]
        public string LastSplitFactorStr { get; set; }

        /// <summary>
        /// Gets the parsed last split factor (ratio), or <c>null</c> if unavailable.
        /// </summary>
        [JsonIgnore]
        public double? LastSplitFactor => Utilities.ParseSplitFactor(this.LastSplitFactorStr);

        /// <summary>
        /// Gets or sets the raw last-split-date string as returned by the API.
        /// Marked <see cref="JsonIncludeAttribute"/> so the private setter is populated during deserialization.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("LastSplitDate")]
        private string LastSplitDateStr { get; set; }

        /// <summary>
        /// Gets the parsed last split date (ISO date), or <c>null</c> if unavailable.
        /// </summary>
        [JsonIgnore]
        public DateTime? LastSplitDate => Utilities.ParseDate(this.LastSplitDateStr);
    }
}
