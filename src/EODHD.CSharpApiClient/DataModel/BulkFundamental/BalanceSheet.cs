using System.Text.Json.Serialization;

namespace EODHD.CSharpApiClient.DataModel.BulkFundamental
{
    /// <summary>
    /// Represents the balance-sheet section of a company's bulk fundamentals,
    /// with the most recent quarterly and yearly snapshots.
    /// </summary>
    public sealed class BalanceSheet
    {
        /// <summary>
        /// Gets or sets the reporting currency symbol.
        /// </summary>
        [JsonPropertyName("currency_symbol")]
        public string CurrencySymbol { get; set; }

        /// <summary>
        /// Gets or sets the most recent quarterly balance-sheet snapshot.
        /// </summary>
        [JsonPropertyName("Quarterly_last_0")]
        public BalanceSheetData QuarterlyLast0 { get; set; }

        /// <summary>
        /// Gets or sets the second most recent quarterly balance-sheet snapshot.
        /// </summary>
        [JsonPropertyName("Quarterly_last_1")]
        public BalanceSheetData QuarterlyLast1 { get; set; }

        /// <summary>
        /// Gets or sets the third most recent quarterly balance-sheet snapshot.
        /// </summary>
        [JsonPropertyName("Quarterly_last_2")]
        public BalanceSheetData QuarterlyLast2 { get; set; }

        /// <summary>
        /// Gets or sets the fourth most recent quarterly balance-sheet snapshot.
        /// </summary>
        [JsonPropertyName("Quarterly_last_3")]
        public BalanceSheetData QuarterlyLast3 { get; set; }

        /// <summary>
        /// Gets or sets the most recent yearly balance-sheet snapshot.
        /// </summary>
        [JsonPropertyName("yearly_last_0")]
        public BalanceSheetData YearlyLast0 { get; set; }

        /// <summary>
        /// Gets or sets the second most recent yearly balance-sheet snapshot.
        /// </summary>
        [JsonPropertyName("yearly_last_1")]
        public BalanceSheetData YearlyLast1 { get; set; }

        /// <summary>
        /// Gets or sets the third most recent yearly balance-sheet snapshot.
        /// </summary>
        [JsonPropertyName("yearly_last_2")]
        public BalanceSheetData YearlyLast2 { get; set; }

        /// <summary>
        /// Gets or sets the fourth most recent yearly balance-sheet snapshot.
        /// </summary>
        [JsonPropertyName("yearly_last_3")]
        public BalanceSheetData YearlyLast3 { get; set; }
    }
}
