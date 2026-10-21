using System;
using System.Text.Json.Serialization;

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
        /// Gets or sets the raw wire value of the date the record was last updated (<c>yyyy-MM-dd</c>).
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("UpdateDate")]
        private string UpdateDateStr { get; set; }

        /// <summary>
        /// Gets the date the record was last updated, parsed from the raw value.
        /// </summary>
        [JsonIgnore]
        public DateTime? UpdateDate => Utilities.ParseDate(this.UpdateDateStr);

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
        /// Gets or sets the raw wire value of the last trade date (<c>yyyy-MM-dd</c>).
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("LastTradeDate")]
        private string LastTradeDateStr { get; set; }

        /// <summary>
        /// Gets the last trade date, parsed from the raw value.
        /// </summary>
        [JsonIgnore]
        public DateTime? LastTradeDate => Utilities.ParseDate(this.LastTradeDateStr);

        /// <summary>
        /// Gets or sets the raw wire value of the maturity date (<c>yyyy-MM-dd</c>).
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("Maturity_Date")]
        private string MaturityDateStr { get; set; }

        /// <summary>
        /// Gets the maturity date, parsed from the raw value.
        /// </summary>
        [JsonIgnore]
        public DateTime? MaturityDate => Utilities.ParseDate(this.MaturityDateStr);

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
        /// Gets or sets the raw wire value of the next call date (<c>yyyy-MM-dd</c>), or <c>null</c>.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("NextCallDate")]
        private string NextCallDateStr { get; set; }

        /// <summary>
        /// Gets the next call date, parsed from the raw value; <c>null</c> when there is none.
        /// </summary>
        [JsonIgnore]
        public DateTime? NextCallDate => Utilities.ParseDate(this.NextCallDateStr);

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
