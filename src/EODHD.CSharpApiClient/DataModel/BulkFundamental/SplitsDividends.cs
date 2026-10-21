using System;
using System.Text.Json.Serialization;

using EODHD.CSharpApiClient.JsonSupport;

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
        /// Gets or sets the dividend date, or <c>null</c> if unavailable.
        /// </summary>
        [JsonPropertyName("DividendDate")]
        [JsonConverter(typeof(NaTolerantDateConverter))]
        public DateTime? DividendDate { get; set; }

        /// <summary>
        /// Gets or sets the ex-dividend date, or <c>null</c> if unavailable.
        /// </summary>
        [JsonPropertyName("ExDividendDate")]
        [JsonConverter(typeof(NaTolerantDateConverter))]
        public DateTime? ExDividendDate { get; set; }

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
        /// Gets or sets the last split date, or <c>null</c> if unavailable.
        /// </summary>
        [JsonPropertyName("LastSplitDate")]
        [JsonConverter(typeof(NaTolerantDateConverter))]
        public DateTime? LastSplitDate { get; set; }
    }
}
