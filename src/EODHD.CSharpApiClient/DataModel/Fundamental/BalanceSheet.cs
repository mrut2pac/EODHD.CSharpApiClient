using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace EODHD.CSharpApiClient.DataModel.Fundamental
{
    /// <summary>
    /// Represents the balance-sheet section of a company's fundamentals.
    /// </summary>
    public sealed class BalanceSheet
    {
        /// <summary>
        /// Gets or sets the reporting currency symbol.
        /// </summary>
        [JsonPropertyName("Currency_symbol")]
        public string CurrencySymbol { get; set; }

        /// <summary>
        /// Gets or sets the quarterly balance-sheet data keyed by ISO date.
        /// </summary>
        public Dictionary<string, BalanceSheetData> Quarterly { get; set; }

        /// <summary>
        /// Gets or sets the yearly balance-sheet data keyed by ISO date.
        /// </summary>
        public Dictionary<string, BalanceSheetData> Yearly { get; set; }

        /// <summary>
        /// Gets or sets the most recent quarterly balance-sheet data.
        /// </summary>
        [JsonPropertyName("Quarterly_last_0")]
        public BalanceSheetData QuarterlyLast0 { get; set; }

        /// <summary>
        /// Gets or sets the quarterly balance-sheet data one quarter back.
        /// </summary>
        [JsonPropertyName("Quarterly_last_1")]
        public BalanceSheetData QuarterlyLast1 { get; set; }

        /// <summary>
        /// Gets or sets the quarterly balance-sheet data two quarters back.
        /// </summary>
        [JsonPropertyName("Quarterly_last_2")]
        public BalanceSheetData QuarterlyLast2 { get; set; }

        /// <summary>
        /// Gets or sets the quarterly balance-sheet data three quarters back.
        /// </summary>
        [JsonPropertyName("Quarterly_last_3")]
        public BalanceSheetData QuarterlyLast3 { get; set; }

        /// <summary>
        /// Gets or sets the most recent yearly balance-sheet data.
        /// </summary>
        [JsonPropertyName("Yearly_last_0")]
        public BalanceSheetData YearlyLast0 { get; set; }

        /// <summary>
        /// Gets or sets the yearly balance-sheet data one year back.
        /// </summary>
        [JsonPropertyName("Yearly_last_1")]
        public BalanceSheetData YearlyLast1 { get; set; }

        /// <summary>
        /// Gets or sets the yearly balance-sheet data two years back.
        /// </summary>
        [JsonPropertyName("Yearly_last_2")]
        public BalanceSheetData YearlyLast2 { get; set; }

        /// <summary>
        /// Gets or sets the yearly balance-sheet data three years back.
        /// </summary>
        [JsonPropertyName("Yearly_last_3")]
        public BalanceSheetData YearlyLast3 { get; set; }
    }
}
