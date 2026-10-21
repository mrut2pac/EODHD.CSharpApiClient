using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace EODHD.CSharpApiClient.DataModel.Fundamental
{
    /// <summary>
    /// Represents the mutual-fund-specific section of a fund's fundamentals (<c>MutualFund_Data</c>).
    /// </summary>
    public sealed class MutualFundData
    {
        /// <summary>
        /// Gets or sets the fund style.
        /// </summary>
        [JsonPropertyName("Fund_Style")]
        public string FundStyle { get; set; }

        /// <summary>
        /// Gets or sets the net asset value per share (currency).
        /// </summary>
        public double? Nav { get; set; }

        /// <summary>
        /// Gets or sets the previous closing price (currency).
        /// </summary>
        [JsonPropertyName("Prev_Close_Price")]
        public double? PrevClosePrice { get; set; }

        /// <summary>
        /// Gets or sets the date the fund data was last updated (ISO date).
        /// </summary>
        [JsonPropertyName("Update_Date")]
        public string UpdateDate { get; set; }

        /// <summary>
        /// Gets or sets the net assets of the whole portfolio, across share classes (currency).
        /// </summary>
        [JsonPropertyName("Portfolio_Net_Assets")]
        public decimal? PortfolioNetAssets { get; set; }

        /// <summary>
        /// Gets or sets the net assets of this share class (currency).
        /// </summary>
        [JsonPropertyName("Share_Class_Net_Assets")]
        public decimal? ShareClassNetAssets { get; set; }

        /// <summary>
        /// Gets or sets the inception date (ISO date).
        /// </summary>
        [JsonPropertyName("Inception_Date")]
        public string InceptionDate { get; set; }

        /// <summary>
        /// Gets or sets the fund currency.
        /// </summary>
        public string Currency { get; set; }

        /// <summary>
        /// Gets or sets the domicile.
        /// </summary>
        public string Domicile { get; set; }

        /// <summary>
        /// Gets or sets the yield.
        /// </summary>
        public double? Yield { get; set; }

        /// <summary>
        /// Gets or sets the year-to-date return (percent).
        /// </summary>
        [JsonPropertyName("Yield_YTD")]
        public double? YieldYtd { get; set; }

        /// <summary>
        /// Gets or sets the one-year return (percent).
        /// </summary>
        [JsonPropertyName("Yield_1Year_YTD")]
        public double? Yield1Year { get; set; }

        /// <summary>
        /// Gets or sets the three-year return (percent).
        /// </summary>
        [JsonPropertyName("Yield_3Year_YTD")]
        public double? Yield3Year { get; set; }

        /// <summary>
        /// Gets or sets the five-year return (percent).
        /// </summary>
        [JsonPropertyName("Yield_5Year_YTD")]
        public double? Yield5Year { get; set; }

        /// <summary>
        /// Gets or sets the expense ratio (percent).
        /// </summary>
        [JsonPropertyName("Expense_Ratio")]
        public double? ExpenseRatio { get; set; }

        /// <summary>
        /// Gets or sets the date of the expense ratio (ISO date).
        /// </summary>
        [JsonPropertyName("Expense_Ratio_Date")]
        public string ExpenseRatioDate { get; set; }

        /// <summary>
        /// Gets or sets the asset allocation rows keyed by ordinal index.
        /// </summary>
        [JsonPropertyName("Asset_Allocation")]
        public Dictionary<int, AssetAllocation> AssetAllocation { get; set; }

        /// <summary>
        /// Gets or sets the value and growth metrics keyed by ordinal index.
        /// </summary>
        [JsonPropertyName("Value_Growth")]
        public Dictionary<int, ValueGrowth> ValueGrowth { get; set; }

        /// <summary>
        /// Gets or sets the top holdings keyed by ordinal index.
        /// </summary>
        [JsonPropertyName("Top_Holdings")]
        public Dictionary<int, TopHoldings> TopHoldings { get; set; }

        /// <summary>
        /// Gets or sets the market-capitalization size buckets keyed by ordinal index.
        /// </summary>
        [JsonPropertyName("Market_Capitalization")]
        public Dictionary<int, MarketCapitalization> MarketCapitalization { get; set; }

        /// <summary>
        /// Gets or sets the sector weights keyed by sector group (e.g. <c>"Cyclical"</c>), then by ordinal index.
        /// </summary>
        [JsonPropertyName("Sector_Weights")]
        public Dictionary<string, Dictionary<int, SectorWeights>> SectorWeights { get; set; }

        /// <summary>
        /// Gets or sets the world-region weights keyed by region group (e.g. <c>"Americas"</c>), then by ordinal index.
        /// </summary>
        [JsonPropertyName("World_Regions")]
        public Dictionary<string, Dictionary<int, WorldRegion>> WorldRegions { get; set; }

        /// <summary>
        /// Gets or sets the top-country exposures keyed by ordinal index.
        /// </summary>
        [JsonPropertyName("Top_Countries")]
        public Dictionary<int, TopCountry> TopCountries { get; set; }
    }
}
