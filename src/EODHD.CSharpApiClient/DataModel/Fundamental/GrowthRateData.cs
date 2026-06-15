using System.Text.Json.Serialization;

namespace EODHD.CSharpApiClient.DataModel.Fundamental
{
    /// <summary>
    /// Represents growth-rate metrics for a fund.
    /// </summary>
    public sealed class GrowthRateData
    {
        /// <summary>
        /// Gets or sets the long-term projected earnings growth.
        /// </summary>
        [JsonPropertyName("Long-Term Projected Earnings Growth")]
        public string LongTermProjectedEarningsGrowth { get; set; }

        /// <summary>
        /// Gets or sets the historical earnings growth.
        /// </summary>
        [JsonPropertyName("Historical Earnings Growth")]
        public string HistoricalEarningsGrowth { get; set; }

        /// <summary>
        /// Gets or sets the sales growth.
        /// </summary>
        [JsonPropertyName("Sales Growth")]
        public string SalesGrowth { get; set; }

        /// <summary>
        /// Gets or sets the cash-flow growth.
        /// </summary>
        [JsonPropertyName("Cash-Flow Growth")]
        public string CashFlowGrowth { get; set; }

        /// <summary>
        /// Gets or sets the book-value growth.
        /// </summary>
        [JsonPropertyName("Book-Value Growth")]
        public string BookValueGrowth { get; set; }
    }
}
