using System;
using System.Text.Json.Serialization;

using EODHD.CSharpApiClient.JsonSupport;

namespace EODHD.CSharpApiClient.DataModel.Bonds
{
    /// <summary>
    /// Fundamentals for a single bond, identified by ISIN.
    /// </summary>
    public sealed class BondFundamentals
    {
        /// <summary>
        /// Gets or sets the International Securities Identification Number.
        /// </summary>
        [JsonPropertyName("ISIN")]
        public string Isin { get; set; }

        /// <summary>
        /// Gets or sets the CUSIP identifier.
        /// </summary>
        [JsonPropertyName("CUSIP")]
        public string Cusip { get; set; }

        /// <summary>
        /// Gets or sets the bond name.
        /// </summary>
        [JsonPropertyName("Name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the date the record was last updated.
        /// </summary>
        [JsonPropertyName("UpdateDate")]
        [JsonConverter(typeof(NaTolerantDateConverter))]
        public DateTime? UpdateDate { get; set; }

        /// <summary>
        /// Gets or sets the German securities identification number (Wertpapierkennnummer).
        /// </summary>
        [JsonPropertyName("WKN")]
        public string Wkn { get; set; }

        /// <summary>
        /// Gets or sets the SEDOL identifier.
        /// </summary>
        [JsonPropertyName("Sedol")]
        public string Sedol { get; set; }

        /// <summary>
        /// Gets or sets the Financial Instrument Global Identifier.
        /// </summary>
        [JsonPropertyName("FIGI")]
        public string Figi { get; set; }

        /// <summary>
        /// Gets or sets the trading currency.
        /// </summary>
        [JsonPropertyName("Currency")]
        public string Currency { get; set; }

        /// <summary>
        /// Gets or sets the coupon rate, percent.
        /// </summary>
        [JsonPropertyName("Coupon")]
        public double? Coupon { get; set; }

        /// <summary>
        /// Gets or sets the latest price.
        /// </summary>
        [JsonPropertyName("Price")]
        public double? Price { get; set; }

        /// <summary>
        /// Gets or sets the last trade date.
        /// </summary>
        [JsonPropertyName("LastTradeDate")]
        [JsonConverter(typeof(NaTolerantDateConverter))]
        public DateTime? LastTradeDate { get; set; }

        /// <summary>
        /// Gets or sets the maturity date.
        /// </summary>
        [JsonPropertyName("Maturity_Date")]
        [JsonConverter(typeof(NaTolerantDateConverter))]
        public DateTime? MaturityDate { get; set; }

        /// <summary>
        /// Gets or sets the yield to maturity, percent.
        /// </summary>
        [JsonPropertyName("YieldToMaturity")]
        public double? YieldToMaturity { get; set; }

        /// <summary>
        /// Gets or sets whether the bond is callable (e.g. <c>"Yes"</c>, <c>"No"</c>).
        /// </summary>
        [JsonPropertyName("Callable")]
        public string Callable { get; set; }

        /// <summary>
        /// Gets or sets the next call date; <c>null</c> when there is none.
        /// </summary>
        [JsonPropertyName("NextCallDate")]
        [JsonConverter(typeof(NaTolerantDateConverter))]
        public DateTime? NextCallDate { get; set; }

        /// <summary>
        /// Gets or sets the minimum settlement amount (e.g. <c>"100000 EUR"</c>).
        /// </summary>
        [JsonPropertyName("MinimumSettlementAmount")]
        public string MinimumSettlementAmount { get; set; }

        /// <summary>
        /// Gets or sets the par integral multiple.
        /// </summary>
        [JsonPropertyName("ParIntegralMultiple")]
        public string ParIntegralMultiple { get; set; }

        /// <summary>
        /// Gets or sets the classification metadata.
        /// </summary>
        [JsonPropertyName("ClassificationData")]
        public BondClassificationData ClassificationData { get; set; }

        /// <summary>
        /// Gets or sets the issuance details.
        /// </summary>
        [JsonPropertyName("IssueData")]
        public BondIssueData IssueData { get; set; }
    }
}
