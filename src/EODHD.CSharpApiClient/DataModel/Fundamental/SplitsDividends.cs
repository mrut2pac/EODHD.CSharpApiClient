using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

using EODHD.CSharpApiClient.JsonSupport;

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
        /// Gets or sets the dividend date.
        /// </summary>
        [JsonPropertyName("DividendDate")]
        [JsonConverter(typeof(NaTolerantDateConverter))]
        public DateTime? DividendDate { get; set; }

        /// <summary>
        /// Gets or sets the ex-dividend date.
        /// </summary>
        [JsonPropertyName("ExDividendDate")]
        [JsonConverter(typeof(NaTolerantDateConverter))]
        public DateTime? ExDividendDate { get; set; }

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
        /// Gets or sets the last-split date.
        /// </summary>
        [JsonPropertyName("LastSplitDate")]
        [JsonConverter(typeof(NaTolerantDateConverter))]
        public DateTime? LastSplitDate { get; set; }

        /// <summary>
        /// Gets or sets the number of dividend payments per year, keyed by year.
        /// </summary>
        public Dictionary<string, NumberDividendsByYear> NumberDividendsByYear { get; set; }
    }
}
