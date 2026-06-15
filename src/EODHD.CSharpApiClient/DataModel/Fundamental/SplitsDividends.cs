using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace EODHD.CSharpApiClient.DataModel.Fundamental
{
    /// <summary>
    /// Represents dividend and stock-split history for a security.
    /// </summary>
    public sealed class SplitsDividends
    {
        /// <summary>
        /// Gets or sets the forward annual dividend rate (currency).
        /// </summary>
        public double? ForwardAnnualDividendRate { get; set; }

        /// <summary>
        /// Gets or sets the forward annual dividend yield (percent).
        /// </summary>
        public double? ForwardAnnualDividendYield { get; set; }

        /// <summary>
        /// Gets or sets the dividend payout ratio.
        /// </summary>
        public double? PayoutRatio { get; set; }

        /// <summary>
        /// Gets or sets the raw dividend date string, as an ISO date.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("DividendDate")]
        private string DividendDateStr { get; set; }

        /// <summary>
        /// Gets the dividend date, parsed from the raw string.
        /// </summary>
        [JsonIgnore]
        public DateTime? DividendDate => Utilities.ParseDate(this.DividendDateStr);

        /// <summary>
        /// Gets or sets the raw ex-dividend date string, as an ISO date.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("ExDividendDate")]
        private string ExDividendDateStr { get; set; }

        /// <summary>
        /// Gets the ex-dividend date, parsed from the raw string.
        /// </summary>
        [JsonIgnore]
        public DateTime? ExDividendDate => Utilities.ParseDate(this.ExDividendDateStr);

        /// <summary>
        /// Gets or sets the raw last-split factor string (e.g. "2:1").
        /// </summary>
        [JsonPropertyName("LastSplitFactor")]
        public string LastSplitFactorStr { get; set; }

        /// <summary>
        /// Gets the last-split factor, parsed from the raw string, as a ratio.
        /// </summary>
        [JsonIgnore]
        public double? LastSplitFactor => Utilities.ParseSplitFactor(this.LastSplitFactorStr);

        /// <summary>
        /// Gets or sets the raw last-split date string, as an ISO date.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("LastSplitDate")]
        private string LastSplitDateStr { get; set; }

        /// <summary>
        /// Gets the last-split date, parsed from the raw string.
        /// </summary>
        [JsonIgnore]
        public DateTime? LastSplitDate => Utilities.ParseDate(this.LastSplitDateStr);

        /// <summary>
        /// Gets or sets the number of dividend payments per year, keyed by year.
        /// </summary>
        public Dictionary<string, NumberDividendsByYear> NumberDividendsByYear { get; set; }
    }
}
