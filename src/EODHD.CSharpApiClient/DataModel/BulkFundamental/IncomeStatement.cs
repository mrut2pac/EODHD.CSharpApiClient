using System.Text.Json.Serialization;

namespace EODHD.CSharpApiClient.DataModel.BulkFundamental
{
    /// <summary>
    /// Represents the income-statement section of a company's bulk fundamentals,
    /// with the most recent quarterly and yearly snapshots.
    /// </summary>
    public sealed class IncomeStatement
    {
        /// <summary>
        /// Gets or sets the reporting currency symbol.
        /// </summary>
        [JsonPropertyName("currency_symbol")]
        public string CurrencySymbol { get; set; }

        /// <summary>
        /// Gets or sets the most recent quarterly income-statement snapshot.
        /// </summary>
        [JsonPropertyName("quarterly_last_0")]
        public IncomeStatementData QuarterlyLast0 { get; set; }

        /// <summary>
        /// Gets or sets the second most recent quarterly income-statement snapshot.
        /// </summary>
        [JsonPropertyName("quarterly_last_1")]
        public IncomeStatementData QuarterlyLast1 { get; set; }

        /// <summary>
        /// Gets or sets the third most recent quarterly income-statement snapshot.
        /// </summary>
        [JsonPropertyName("quarterly_last_2")]
        public IncomeStatementData QuarterlyLast2 { get; set; }

        /// <summary>
        /// Gets or sets the fourth most recent quarterly income-statement snapshot.
        /// </summary>
        [JsonPropertyName("quarterly_last_3")]
        public IncomeStatementData QuarterlyLast3 { get; set; }

        /// <summary>
        /// Gets or sets the most recent yearly income-statement snapshot.
        /// </summary>
        [JsonPropertyName("yearly_last_0")]
        public IncomeStatementData YearlyLast0 { get; set; }

        /// <summary>
        /// Gets or sets the second most recent yearly income-statement snapshot.
        /// </summary>
        [JsonPropertyName("yearly_last_1")]
        public IncomeStatementData YearlyLast1 { get; set; }

        /// <summary>
        /// Gets or sets the third most recent yearly income-statement snapshot.
        /// </summary>
        [JsonPropertyName("yearly_last_2")]
        public IncomeStatementData YearlyLast2 { get; set; }

        /// <summary>
        /// Gets or sets the fourth most recent yearly income-statement snapshot.
        /// </summary>
        [JsonPropertyName("yearly_last_3")]
        public IncomeStatementData YearlyLast3 { get; set; }
    }
}
