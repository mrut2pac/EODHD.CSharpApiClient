using System;
using System.Text.Json.Serialization;

namespace EODHD.CSharpApiClient.DataModel.MarketCap
{
    /// <summary>
    /// A single weekly historical market-capitalisation data point for a symbol.
    /// </summary>
    public sealed class HistoricalMarketCap
    {
        /// <summary>
        /// Gets or sets the date of the data point.
        /// </summary>
        [JsonPropertyName("date")]
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets the market capitalisation (in the symbol's reporting currency).
        /// </summary>
        [JsonPropertyName("value")]
        public decimal? Value { get; set; }
    }
}
