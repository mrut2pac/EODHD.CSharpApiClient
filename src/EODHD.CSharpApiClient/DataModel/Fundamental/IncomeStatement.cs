using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace EODHD.CSharpApiClient.DataModel.Fundamental
{
    /// <summary>
    /// Represents the income statement, with quarterly and yearly breakdowns.
    /// </summary>
    public sealed class IncomeStatement
    {
        /// <summary>
        /// Gets or sets the currency symbol.
        /// </summary>
        [JsonPropertyName("Currency_symbol")]
        public string CurrencySymbol { get; set; }

        /// <summary>
        /// Gets or sets the quarterly income statements, keyed by period date.
        /// </summary>
        public Dictionary<string, IncomeStatementData> Quarterly { get; set; }

        /// <summary>
        /// Gets or sets the yearly income statements, keyed by period date.
        /// </summary>
        public Dictionary<string, IncomeStatementData> Yearly { get; set; }

        /// <summary>
        /// Gets or sets the most recent quarterly income statement.
        /// </summary>
        [JsonPropertyName("Quarterly_last_0")]
        public IncomeStatementData QuarterlyLast0 { get; set; }

        /// <summary>
        /// Gets or sets the income statement for the quarter one period back.
        /// </summary>
        [JsonPropertyName("Quarterly_last_1")]
        public IncomeStatementData QuarterlyLast1 { get; set; }

        /// <summary>
        /// Gets or sets the income statement for the quarter two periods back.
        /// </summary>
        [JsonPropertyName("Quarterly_last_2")]
        public IncomeStatementData QuarterlyLast2 { get; set; }

        /// <summary>
        /// Gets or sets the income statement for the quarter three periods back.
        /// </summary>
        [JsonPropertyName("Quarterly_last_3")]
        public IncomeStatementData QuarterlyLast3 { get; set; }

        /// <summary>
        /// Gets or sets the most recent yearly income statement.
        /// </summary>
        [JsonPropertyName("Yearly_last_0")]
        public IncomeStatementData YearlyLast0 { get; set; }

        /// <summary>
        /// Gets or sets the income statement for the year one period back.
        /// </summary>
        [JsonPropertyName("Yearly_last_1")]
        public IncomeStatementData YearlyLast1 { get; set; }

        /// <summary>
        /// Gets or sets the income statement for the year two periods back.
        /// </summary>
        [JsonPropertyName("Yearly_last_2")]
        public IncomeStatementData YearlyLast2 { get; set; }

        /// <summary>
        /// Gets or sets the income statement for the year three periods back.
        /// </summary>
        [JsonPropertyName("Yearly_last_3")]
        public IncomeStatementData YearlyLast3 { get; set; }
    }
}
