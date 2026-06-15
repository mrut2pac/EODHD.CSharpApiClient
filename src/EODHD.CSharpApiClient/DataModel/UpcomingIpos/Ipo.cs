using System;
using System.Text.Json.Serialization;

namespace EODHD.CSharpApiClient.DataModel.UpcomingIpos
{
    /// <summary>
    /// Represents a single upcoming or recent IPO entry.
    /// </summary>
    public sealed class Ipo
    {
        /// <summary>
        /// Gets or sets the full EODHD code (symbol and exchange).
        /// </summary>
        [JsonPropertyName("code")]
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets the company name.
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the listing exchange.
        /// </summary>
        [JsonPropertyName("exchange")]
        public string Exchange { get; set; }

        /// <summary>
        /// Gets or sets the trading currency.
        /// </summary>
        [JsonPropertyName("currency")]
        public string Currency { get; set; }

        /// <summary>
        /// Gets or sets the expected or effective first trading date.
        /// </summary>
        [JsonPropertyName("start_date")]
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// Gets or sets the initial filing date.
        /// </summary>
        [JsonPropertyName("filing_date")]
        public DateTime? FilingDate { get; set; }

        /// <summary>
        /// Gets or sets the latest amended filing date.
        /// </summary>
        [JsonPropertyName("amended_date")]
        public DateTime? AmendedDate { get; set; }

        /// <summary>
        /// Gets or sets the lower end of the offering price range.
        /// </summary>
        [JsonPropertyName("price_from")]
        public double? PriceFrom { get; set; }

        /// <summary>
        /// Gets or sets the upper end of the offering price range.
        /// </summary>
        [JsonPropertyName("price_to")]
        public double? PriceTo { get; set; }

        /// <summary>
        /// Gets or sets the final priced offer price.
        /// </summary>
        [JsonPropertyName("offer_price")]
        public double? OfferPrice { get; set; }

        /// <summary>
        /// Gets or sets the number of shares offered.
        /// </summary>
        [JsonPropertyName("shares")]
        public double? Shares { get; set; }

        /// <summary>
        /// Gets or sets the deal lifecycle state (e.g. <c>"Filed"</c>, <c>"Expected"</c>, <c>"Amended"</c>, <c>"Priced"</c>).
        /// </summary>
        [JsonPropertyName("deal_type")]
        public string DealType { get; set; }

        /// <summary>
        /// Gets or sets the ticker symbol.
        /// </summary>
        [JsonPropertyName("symbol")]
        public string Symbol { get; set; }
    }
}
