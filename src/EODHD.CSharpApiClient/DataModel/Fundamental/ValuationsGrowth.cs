using System.Text.Json.Serialization;

namespace EODHD.CSharpApiClient.DataModel.Fundamental
{
    /// <summary>
    /// Represents ETF valuation and growth rates relative to the portfolio and category.
    /// </summary>
    public sealed class ValuationsGrowth
    {
        /// <summary>
        /// Gets or sets the portfolio valuation rates.
        /// </summary>
        [JsonPropertyName("Valuations_Rates_Portfolio")]
        public ValuationsRatesData ValuationsRatesPortfolio { get; set; }

        /// <summary>
        /// Gets or sets the valuation rates relative to the category.
        /// </summary>
        [JsonPropertyName("Valuations_Rates_To_Category")]
        public ValuationsRatesData ValuationsRatesToCategory { get; set; }

        /// <summary>
        /// Gets or sets the portfolio growth rates.
        /// </summary>
        [JsonPropertyName("Growth_Rates_Portfolio")]
        public GrowthRateData GrowthRatesPortfolio { get; set; }

        /// <summary>
        /// Gets or sets the growth rates relative to the category.
        /// </summary>
        [JsonPropertyName("Growth_Rates_To_Category")]
        public GrowthRateData GrowthRatesToCategory { get; set; }
    }
}
