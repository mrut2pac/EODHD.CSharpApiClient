using System.Text.Json.Serialization;

namespace EODHD.CSharpApiClient.DataModel.BulkFundamental
{
    /// <summary>
    /// Represents the cash-flow section of a company's bulk fundamentals,
    /// with the most recent quarterly and yearly snapshots.
    /// </summary>
    public sealed class CashFlow
    {
        /// <summary>
        /// Gets or sets the reporting currency symbol.
        /// </summary>
        [JsonPropertyName("currency_symbol")]
        public string CurrencySymbol { get; set; }

        /// <summary>
        /// Gets or sets the most recent quarterly cash-flow snapshot.
        /// </summary>
        [JsonPropertyName("Quarterly_last_0")]
        public CashFlowData QuarterlyLast0 { get; set; }

        /// <summary>
        /// Gets or sets the second most recent quarterly cash-flow snapshot.
        /// </summary>
        [JsonPropertyName("Quarterly_last_1")]
        public CashFlowData QuarterlyLast1 { get; set; }

        /// <summary>
        /// Gets or sets the third most recent quarterly cash-flow snapshot.
        /// </summary>
        [JsonPropertyName("Quarterly_last_2")]
        public CashFlowData QuarterlyLast2 { get; set; }

        /// <summary>
        /// Gets or sets the fourth most recent quarterly cash-flow snapshot.
        /// </summary>
        [JsonPropertyName("Quarterly_last_3")]
        public CashFlowData QuarterlyLast3 { get; set; }

        /// <summary>
        /// Gets or sets the most recent yearly cash-flow snapshot.
        /// </summary>
        [JsonPropertyName("Yearly_last_0")]
        public CashFlowData YearlyLast0 { get; set; }

        /// <summary>
        /// Gets or sets the second most recent yearly cash-flow snapshot.
        /// </summary>
        [JsonPropertyName("Yearly_last_1")]
        public CashFlowData YearlyLast1 { get; set; }

        /// <summary>
        /// Gets or sets the third most recent yearly cash-flow snapshot.
        /// </summary>
        [JsonPropertyName("Yearly_last_2")]
        public CashFlowData YearlyLast2 { get; set; }

        /// <summary>
        /// Gets or sets the fourth most recent yearly cash-flow snapshot.
        /// </summary>
        [JsonPropertyName("Yearly_last_3")]
        public CashFlowData YearlyLast3 { get; set; }
    }
}
