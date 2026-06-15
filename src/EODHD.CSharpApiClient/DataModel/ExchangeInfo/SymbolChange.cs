using System;
using System.Text.Json.Serialization;

namespace EODHD.CSharpApiClient.DataModel.ExchangeInfo
{
    /// <summary>
    /// Represents a symbol-change event for a security on an exchange.
    /// </summary>
    public sealed class SymbolChange
    {
        /// <summary>
        /// Gets or sets the exchange the change applies to.
        /// </summary>
        [JsonPropertyName("exchange")]
        public string Exchange { get; set; }

        /// <summary>
        /// Gets or sets the previous symbol.
        /// </summary>
        [JsonPropertyName("old_symbol")]
        public string OldSymbol { get; set; }

        /// <summary>
        /// Gets or sets the new symbol.
        /// </summary>
        [JsonPropertyName("new_symbol")]
        public string NewSymbol { get; set; }

        /// <summary>
        /// Gets or sets the company name.
        /// </summary>
        [JsonPropertyName("company_name")]
        public string CompanyName { get; set; }

        /// <summary>
        /// Gets or sets the date the change became effective.
        /// </summary>
        [JsonPropertyName("effective")]
        public DateTime Effective { get; set; }
    }
}
