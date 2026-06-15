using System.Text.Json.Serialization;

namespace EODHD.CSharpApiClient.DataModel.Fundamental
{
    /// <summary>
    /// Represents the financial statements (balance sheet, cash flow, income statement) for an instrument.
    /// </summary>
    public sealed class Financials
    {
        /// <summary>
        /// Gets or sets the balance sheet.
        /// </summary>
        [JsonPropertyName("Balance_Sheet")]
        public BalanceSheet BalanceSheet { get; set; }

        /// <summary>
        /// Gets or sets the cash flow statement.
        /// </summary>
        [JsonPropertyName("Cash_Flow")]
        public CashFlow CashFlow { get; set; }

        /// <summary>
        /// Gets or sets the income statement.
        /// </summary>
        [JsonPropertyName("Income_Statement")]
        public IncomeStatement IncomeStatement { get; set; }
    }
}
