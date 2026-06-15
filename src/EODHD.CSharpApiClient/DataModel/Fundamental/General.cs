using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace EODHD.CSharpApiClient.DataModel.Fundamental
{
    /// <summary>
    /// Represents general company and instrument information.
    /// </summary>
    public sealed class General
    {
        /// <summary>
        /// Gets or sets the instrument code.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets the instrument type.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the instrument name.
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
        /// Gets or sets the OpenFIGI identifier.
        /// </summary>
        public string OpenFigi { get; set; }

        /// <summary>
        /// Gets or sets the ISIN identifier.
        /// </summary>
        public string ISIN { get; set; }

        /// <summary>
        /// Gets or sets the Legal Entity Identifier (LEI).
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
        /// Gets or sets the Central Index Key (CIK).
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
        /// Gets or sets the raw IPO date string, as an ISO date.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("IPODate")]
        private string IpoDateStr { get; set; }

        /// <summary>
        /// Gets the IPO date, parsed from the raw string.
        /// </summary>
        [JsonIgnore]
        public DateTime? IPODate => Utilities.ParseDate(this.IpoDateStr);

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
        /// Gets or sets the GIC sector.
        /// </summary>
        public string GicSector { get; set; }

        /// <summary>
        /// Gets or sets the GIC group.
        /// </summary>
        public string GicGroup { get; set; }

        /// <summary>
        /// Gets or sets the GIC industry.
        /// </summary>
        public string GicIndustry { get; set; }

        /// <summary>
        /// Gets or sets the GIC sub-industry.
        /// </summary>
        public string GicSubIndustry { get; set; }

        /// <summary>
        /// Gets or sets the home category.
        /// </summary>
        public string HomeCategory { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the instrument is delisted.
        /// </summary>
        public bool? IsDelisted { get; set; }

        /// <summary>
        /// Gets or sets the company description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the company address.
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Gets or sets the structured address data.
        /// </summary>
        public AddressData AddressData { get; set; }

        /// <summary>
        /// Gets or sets the exchange listings, keyed by listing index.
        /// </summary>
        public Dictionary<int, Listing> Listings { get; set; }

        /// <summary>
        /// Gets or sets the company officers, keyed by officer index.
        /// </summary>
        public Dictionary<int, Officer> Officers { get; set; }

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
        /// Gets or sets the number of full-time employees, as a count.
        /// </summary>
        public int? FullTimeEmployees { get; set; }

        /// <summary>
        /// Gets or sets the raw last-updated date string, as an ISO date.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("UpdatedAt")]
        private string updatedAtStr { get; set; }

        /// <summary>
        /// Gets the last-updated date, parsed from the raw string.
        /// </summary>
        [JsonIgnore]
        public DateTime? UpdatedAt => Utilities.ParseDate(this.updatedAtStr);

        /// <summary>
        /// Gets or sets the fund summary.
        /// </summary>
        [JsonPropertyName("Fund_Summary")]
        public string FundSummary { get; set; }

        /// <summary>
        /// Gets or sets the fund family.
        /// </summary>
        [JsonPropertyName("Fund_Family")]
        public string FundFamily { get; set; }

        /// <summary>
        /// Gets or sets the fund category.
        /// </summary>
        [JsonPropertyName("Fund_Category")]
        public string FundCategory { get; set; }

        /// <summary>
        /// Gets or sets the fund style.
        /// </summary>
        [JsonPropertyName("Fund_Style")]
        public string FundStyle { get; set; }

        /// <summary>
        /// Gets or sets the fund fiscal year end.
        /// </summary>
        [JsonPropertyName("Fiscal_Year_End")]
        public string FundFiscalYearEnd { get; set; }

        /// <summary>
        /// Gets or sets the market capitalization, as a currency amount.
        /// </summary>
        public double? MarketCapitalization { get; set; }

        /// <summary>
        /// Gets or sets the category.
        /// </summary>
        public string Category { get; set; }
    }
}
