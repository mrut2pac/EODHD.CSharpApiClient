using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace EODHD.CSharpApiClient.DataModel.Fundamental
{
    /// <summary>
    /// Represents the ETF-specific section of a fund's fundamentals.
    /// </summary>
    public sealed class ETFData
    {
        /// <summary>
        /// Gets or sets the ISIN identifier.
        /// </summary>
        public string ISIN { get; set; }

        /// <summary>
        /// Gets or sets the company name.
        /// </summary>
        [JsonPropertyName("Company_Name")]
        public string CompanyName { get; set; }

        /// <summary>
        /// Gets or sets the company URL.
        /// </summary>
        [JsonPropertyName("Company_URL")]
        public string CompanyUrl { get; set; }

        /// <summary>
        /// Gets or sets the ETF URL.
        /// </summary>
        [JsonPropertyName("ETF_URL")]
        public string ETFUrl { get; set; }

        /// <summary>
        /// Gets or sets the domicile.
        /// </summary>
        public string Domicile { get; set; }

        /// <summary>
        /// Gets or sets the tracked index name.
        /// </summary>
        public string Index_Name { get; set; }

        /// <summary>
        /// Gets or sets the yield (percent).
        /// </summary>
        public double? Yield { get; set; }

        /// <summary>
        /// Gets or sets the dividend-paying frequency.
        /// </summary>
        [JsonPropertyName("Dividend_Paying_Frequency")]
        public string DividendPayingFrequency { get; set; }

        /// <summary>
        /// Gets or sets the inception date (ISO date).
        /// </summary>
        [JsonPropertyName("Inception_Date")]
        public string InceptionDate { get; set; }

        /// <summary>
        /// Gets or sets the maximum annual management charge (percent).
        /// </summary>
        [JsonPropertyName("Max_Annual_Mgmt_Charge")]
        public double? MaxAnnualManagementCharge { get; set; }

        /// <summary>
        /// Gets or sets the ongoing charge (percent).
        /// </summary>
        [JsonPropertyName("Ongoing_Charge")]
        public double? OngoingCharge { get; set; }

        /// <summary>
        /// Gets or sets the ongoing-charge date (ISO date).
        /// </summary>
        [JsonPropertyName("Date_Ongoing_Charge")]
        public string DateOngoingCharge { get; set; }

        /// <summary>
        /// Gets or sets the net expense ratio (ratio).
        /// </summary>
        public double? NetExpenseRatio { get; set; }

        /// <summary>
        /// Gets or sets the annual holdings turnover (ratio).
        /// </summary>
        public double? AnnualHoldingsTurnover { get; set; }

        /// <summary>
        /// Gets or sets the total assets (currency).
        /// </summary>
        public decimal? TotalAssets { get; set; }

        /// <summary>
        /// Gets or sets the average market capitalization in millions (currency).
        /// </summary>
        [JsonPropertyName("Average_Mkt_Cap_Mil")]
        public string AverageMarketCapitalizationMillions { get; set; }

        /// <summary>
        /// Gets or sets the market capitalization breakdown.
        /// </summary>
        [JsonPropertyName("Market_Capitalisation")]
        public MarketCapitalizationETF MarketCapitalization { get; set; }

        /// <summary>
        /// Gets or sets the asset allocation breakdown.
        /// </summary>
        [JsonPropertyName("Asset_Allocation")]
        public AssetAllocationETF AssetAllocation { get; set; }

        /// <summary>
        /// Gets or sets the world-region breakdown.
        /// </summary>
        [JsonPropertyName("World_Regions")]
        public WorldRegionETF WorldRegions { get; set; }

        /// <summary>
        /// Gets or sets the sector-weight breakdown.
        /// </summary>
        [JsonPropertyName("Sector_Weights")]
        public SectorWeightETF SectorWeights { get; set; }

        /// <summary>
        /// Gets or sets the fixed-income breakdown keyed by metric name.
        /// </summary>
        [JsonPropertyName("Fixed_Income")]
        public Dictionary<string, FixedIncomeData> FixedIncome { get; set; }

        /// <summary>
        /// Gets or sets the number of holdings (count).
        /// </summary>
        [JsonPropertyName("Holdings_Count")]
        public decimal? HoldingsCount { get; set; }

        /// <summary>
        /// Gets or sets the top 10 holdings keyed by symbol.
        /// </summary>
        [JsonPropertyName("Top_10_Holdings")]
        public Dictionary<string, Holding> Top10Holdings { get; set; }

        /// <summary>
        /// Gets or sets all holdings keyed by symbol.
        /// </summary>
        public Dictionary<string, Holding> Holdings { get; set; }

        /// <summary>
        /// Gets or sets the valuations and growth metrics.
        /// </summary>
        [JsonPropertyName("Valuations_Growth")]
        public ValuationsGrowth ValuationsGrowth { get; set; }

        /// <summary>
        /// Gets or sets the Morningstar ratings.
        /// </summary>
        public MorningStar MorningStar { get; set; }

        /// <summary>
        /// Gets or sets the performance metrics.
        /// </summary>
        public Performance Performance { get; set; }
    }
}
