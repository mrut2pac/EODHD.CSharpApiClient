using System.Text.Json.Serialization;

namespace EODHD.CSharpApiClient.DataModel.Fundamental
{
    /// <summary>
    /// Represents a set of valuation rates for an ETF.
    /// </summary>
    public sealed class ValuationsRatesData
    {
        /// <summary>
        /// Gets or sets the price-to-prospective-earnings ratio.
        /// </summary>
        [JsonPropertyName("Price/Prospective Earnings")]
        public string PriceProspectiveEarnings { get; set; }

        /// <summary>
        /// Gets or sets the price-to-book ratio.
        /// </summary>
        [JsonPropertyName("Price/Book")]
        public string PriceBook { get; set; }

        /// <summary>
        /// Gets or sets the price-to-sales ratio.
        /// </summary>
        [JsonPropertyName("Price/Sales")]
        public string PriceSales { get; set; }

        /// <summary>
        /// Gets or sets the price-to-cash-flow ratio.
        /// </summary>
        [JsonPropertyName("Price/Cash Flow")]
        public string PriceCashFlow { get; set; }

        /// <summary>
        /// Gets or sets the dividend-yield factor.
        /// </summary>
        [JsonPropertyName("Dividend-Yield Factor")]
        public string DividendYieldFactor { get; set; }
    }
}
