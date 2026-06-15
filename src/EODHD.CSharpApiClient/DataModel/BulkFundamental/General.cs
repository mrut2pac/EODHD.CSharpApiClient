using System;
using System.Text.Json.Serialization;

namespace EODHD.CSharpApiClient.DataModel.BulkFundamental
{
    /// <summary>
    /// Represents the general company information section of a company's bulk fundamentals.
    /// </summary>
    public sealed class General
    {
        /// <summary>
        /// Gets or sets the symbol code.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets the instrument type.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the company name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the exchange code.
        /// </summary>
        public string Exchange { get; set; }

        /// <summary>
        /// Gets or sets the currency code.
        /// </summary>
        public string CurrencyCode { get; set; }

        /// <summary>
        /// Gets or sets the currency name.
        /// </summary>
        public string CurrencyName { get; set; }

        /// <summary>
        /// Gets or sets the currency symbol.
        /// </summary>
        public string CurrencySymbol { get; set; }

        /// <summary>
        /// Gets or sets the country name.
        /// </summary>
        public string CountryName { get; set; }

        /// <summary>
        /// Gets or sets the ISO country code.
        /// </summary>
        public string CountryISO { get; set; }

        /// <summary>
        /// Gets or sets the International Securities Identification Number.
        /// </summary>
        public string ISIN { get; set; }

        /// <summary>
        /// Gets or sets the Legal Entity Identifier.
        /// </summary>
        public string LEI { get; set; }

        /// <summary>
        /// Gets or sets the primary ticker.
        /// </summary>
        public string PrimaryTicker { get; set; }

        /// <summary>
        /// Gets or sets the CUSIP identifier.
        /// </summary>
        public string CUSIP { get; set; }

        /// <summary>
        /// Gets or sets the SEC Central Index Key.
        /// </summary>
        public string CIK { get; set; }

        /// <summary>
        /// Gets or sets the employer identification number.
        /// </summary>
        public string EmployerIdNumber { get; set; }

        /// <summary>
        /// Gets or sets the fiscal year end.
        /// </summary>
        public string FiscalYearEnd { get; set; }

        /// <summary>
        /// Gets or sets the raw IPO date string as returned by the API.
        /// Marked <see cref="JsonIncludeAttribute"/> so the private setter is populated during deserialization.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("IPODate")]
        private string ipoDateStr { get; set; }

        /// <summary>
        /// Gets the parsed IPO date (ISO date), or <c>null</c> if unavailable.
        /// </summary>
        [JsonIgnore]
        public DateTime? IPODate => Utilities.ParseDate(this.ipoDateStr);

        /// <summary>
        /// Gets or sets the international/domestic classification.
        /// </summary>
        public string InternationalDomestic { get; set; }

        /// <summary>
        /// Gets or sets the sector.
        /// </summary>
        public string Sector { get; set; }

        /// <summary>
        /// Gets or sets the industry.
        /// </summary>
        public string Industry { get; set; }

        /// <summary>
        /// Gets or sets the GICS sector.
        /// </summary>
        public string GicSector { get; set; }

        /// <summary>
        /// Gets or sets the GICS group.
        /// </summary>
        public string GicGroup { get; set; }

        /// <summary>
        /// Gets or sets the GICS industry.
        /// </summary>
        public string GicIndustry { get; set; }

        /// <summary>
        /// Gets or sets the GICS sub-industry.
        /// </summary>
        public string GicSubIndustry { get; set; }

        /// <summary>
        /// Gets or sets the home category.
        /// </summary>
        public string HomeCategory { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the security is delisted.
        /// </summary>
        public bool IsDelisted { get; set; }

        /// <summary>
        /// Gets or sets the company description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the company address.
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Gets or sets the company phone number.
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// Gets or sets the company website URL.
        /// </summary>
        public string WebURL { get; set; }

        /// <summary>
        /// Gets or sets the company logo URL.
        /// </summary>
        public string LogoURL { get; set; }

        /// <summary>
        /// Gets or sets the number of full-time employees (count).
        /// </summary>
        public int? FullTimeEmployees { get; set; }

        /// <summary>
        /// Gets or sets the timestamp of the last update.
        /// </summary>
        public DateTime? UpdatedAt { get; set; }
    }
}
