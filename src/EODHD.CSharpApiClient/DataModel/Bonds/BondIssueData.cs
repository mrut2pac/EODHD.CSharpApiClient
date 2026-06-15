using System.Text.Json.Serialization;

namespace EODHD.CSharpApiClient.DataModel.Bonds
{
    /// <summary>
    /// Issuance details for a bond (issue dates, coupon frequency, and issuer information).
    /// </summary>
    public sealed class BondIssueData
    {
        /// <summary>
        /// Gets or sets the issue date (<c>yyyy-MM-dd</c>).
        /// </summary>
        [JsonPropertyName("IssueDate")]
        public string IssueDate { get; set; }

        /// <summary>
        /// Gets or sets the offering date (<c>yyyy-MM-dd</c>).
        /// </summary>
        [JsonPropertyName("OfferingDate")]
        public string OfferingDate { get; set; }

        /// <summary>
        /// Gets or sets the first coupon date (<c>yyyy-MM-dd</c>).
        /// </summary>
        [JsonPropertyName("FirstCouponDate")]
        public string FirstCouponDate { get; set; }

        /// <summary>
        /// Gets or sets the first trading day (<c>yyyy-MM-dd</c>).
        /// </summary>
        [JsonPropertyName("FirstTradingDay")]
        public string FirstTradingDay { get; set; }

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
