using System;
using System.Text.Json.Serialization;

namespace EODHD.CSharpApiClient.DataModel.Bonds
{
    /// <summary>
    /// Issuance details for a bond (issue dates, coupon frequency, and issuer information).
    /// </summary>
    public sealed class BondIssueData
    {
        /// <summary>
        /// Gets or sets the raw wire value of the issue date (<c>yyyy-MM-dd</c>).
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("IssueDate")]
        private string IssueDateStr { get; set; }

        /// <summary>
        /// Gets the issue date, parsed from the raw value.
        /// </summary>
        [JsonIgnore]
        public DateTime? IssueDate => Utilities.ParseDate(this.IssueDateStr);

        /// <summary>
        /// Gets or sets the raw wire value of the offering date (<c>yyyy-MM-dd</c>).
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("OfferingDate")]
        private string OfferingDateStr { get; set; }

        /// <summary>
        /// Gets the offering date, parsed from the raw value.
        /// </summary>
        [JsonIgnore]
        public DateTime? OfferingDate => Utilities.ParseDate(this.OfferingDateStr);

        /// <summary>
        /// Gets or sets the raw wire value of the first coupon date (<c>yyyy-MM-dd</c>).
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("FirstCouponDate")]
        private string FirstCouponDateStr { get; set; }

        /// <summary>
        /// Gets the first coupon date, parsed from the raw value.
        /// </summary>
        [JsonIgnore]
        public DateTime? FirstCouponDate => Utilities.ParseDate(this.FirstCouponDateStr);

        /// <summary>
        /// Gets or sets the raw wire value of the first trading day (<c>yyyy-MM-dd</c>).
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("FirstTradingDay")]
        private string FirstTradingDayStr { get; set; }

        /// <summary>
        /// Gets the first trading day, parsed from the raw value.
        /// </summary>
        [JsonIgnore]
        public DateTime? FirstTradingDay => Utilities.ParseDate(this.FirstTradingDayStr);

        /// <summary>
        /// Gets or sets the coupon payment frequency.
        /// </summary>
        [JsonPropertyName("CouponPaymentFrequency")]
        public string CouponPaymentFrequency { get; set; }

        /// <summary>
        /// Gets or sets the issuer name.
        /// </summary>
        [JsonPropertyName("Issuer")]
        public string Issuer { get; set; }

        /// <summary>
        /// Gets or sets the issuer description.
        /// </summary>
        [JsonPropertyName("IssuerDescription")]
        public string IssuerDescription { get; set; }

        /// <summary>
        /// Gets or sets the issuer's country.
        /// </summary>
        [JsonPropertyName("IssuerCountry")]
        public string IssuerCountry { get; set; }

        /// <summary>
        /// Gets or sets the issuer's website URL.
        /// </summary>
        [JsonPropertyName("IssuerURL")]
        public string IssuerUrl { get; set; }
    }
}
