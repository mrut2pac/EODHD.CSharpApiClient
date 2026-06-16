using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace EODHD.CSharpApiClient.DataModel.Fundamental
{
    /// <summary>
    /// Represents the cash-flow section of a company's fundamentals.
    /// </summary>
    public sealed class CashFlow
    {
        /// <summary>
        /// Gets or sets the reporting currency symbol.
        /// </summary>
        [JsonPropertyName("Currency_symbol")]
        public string CurrencySymbol { get; set; }

        /// <summary>
        /// Gets or sets the quarterly cash-flow data keyed by ISO date.
        /// </summary>
        public Dictionary<string, CashFlowData> Quarterly { get; set; }

        /// <summary>
        /// Gets or sets the yearly cash-flow data keyed by ISO date.
        /// </summary>
        public Dictionary<string, CashFlowData> Yearly { get; set; }

        /// <summary>
        /// Gets or sets the most recent quarterly cash-flow data.
        /// </summary>
        [JsonPropertyName("Quarterly_last_0")]
        public CashFlowData QuarterlyLast0 { get; set; }

        /// <summary>
        /// Gets or sets the quarterly cash-flow data one quarter back.
        /// </summary>
        [JsonPropertyName("Quarterly_last_1")]
        public CashFlowData QuarterlyLast1 { get; set; }

        /// <summary>
        /// Gets or sets the quarterly cash-flow data two quarters back.
        /// </summary>
        [JsonPropertyName("Quarterly_last_2")]
        public CashFlowData QuarterlyLast2 { get; set; }

        /// <summary>
        /// Gets or sets the quarterly cash-flow data three quarters back.
        /// </summary>
        [JsonPropertyName("Quarterly_last_3")]
        public CashFlowData QuarterlyLast3 { get; set; }

        /// <summary>
        /// Gets or sets the most recent yearly cash-flow data.
        /// </summary>
        [JsonPropertyName("Yearly_last_0")]
        public CashFlowData YearlyLast0 { get; set; }

        /// <summary>
        /// Gets or sets the yearly cash-flow data one year back.
        /// </summary>
        [JsonPropertyName("Yearly_last_1")]
        public CashFlowData YearlyLast1 { get; set; }

        /// <summary>
        /// Gets or sets the yearly cash-flow data two years back.
        /// </summary>
        [JsonPropertyName("Yearly_last_2")]
        public CashFlowData YearlyLast2 { get; set; }

        /// <summary>
        /// Gets or sets the yearly cash-flow data three years back.
        /// </summary>
        [JsonPropertyName("Yearly_last_3")]
        public CashFlowData YearlyLast3 { get; set; }
    }
}
