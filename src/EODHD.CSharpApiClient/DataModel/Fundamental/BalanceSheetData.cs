using System;
using System.Text.Json.Serialization;

namespace EODHD.CSharpApiClient.DataModel.Fundamental
{
    /// <summary>
    /// Represents a single balance-sheet statement for a reporting period.
    /// </summary>
    public sealed class BalanceSheetData
    {
        /// <summary>
        /// Gets or sets the raw report date string (ISO date).
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("Date")]
        private string dateStr { get; set; }

        /// <summary>
        /// Gets the report date (ISO date), parsed from the raw value.
        /// </summary>
        [JsonIgnore]
        public DateTime? Date { get => Utilities.ParseDate(this.dateStr); }

        /// <summary>
        /// Gets or sets the raw filing date string (ISO date).
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("Filing_date")]
        private string filingDateStr { get; set; }

        /// <summary>
        /// Gets the filing date (ISO date), parsed from the raw value.
        /// </summary>
        [JsonIgnore]
        public DateTime? FilingDate { get => Utilities.ParseDate(this.filingDateStr); }

        /// <summary>
        /// Gets or sets the reporting currency symbol.
        /// </summary>
        [JsonPropertyName("currency_symbol")]
        public string CurrencySymbol { get; set; }

        /// <summary>
        /// Gets or sets the total assets (currency).
        /// </summary>
        public double? TotalAssets { get; set; }

        /// <summary>
        /// Gets or sets the intangible assets (currency).
        /// </summary>
        public double? IntangibleAssets { get; set; }

        /// <summary>
        /// Gets or sets the earning assets (currency).
        /// </summary>
        public double? EarningAssets { get; set; }

        /// <summary>
        /// Gets or sets the other current assets (currency).
        /// </summary>
        public double? OtherCurrentAssets { get; set; }

        /// <summary>
        /// Gets or sets the total liabilities (currency).
        /// </summary>
        [JsonPropertyName("TotalLiab")]
        public double? TotalLiabilities { get; set; }

        /// <summary>
        /// Gets or sets the total stockholder equity (currency).
        /// </summary>
        public double? TotalStockholderEquity { get; set; }

        /// <summary>
        /// Gets or sets the deferred long-term liabilities (currency).
        /// </summary>
        [JsonPropertyName("DeferredLongTermLiab")]
        public double? DeferredLongTermLiabilities { get; set; }

        /// <summary>
        /// Gets or sets the other current liabilities (currency).
        /// </summary>
        [JsonPropertyName("OtherCurrentLiab")]
        public double? OtherCurrentLiabilities { get; set; }

        /// <summary>
        /// Gets or sets the common stock (currency).
        /// </summary>
        public double? CommonStock { get; set; }

        /// <summary>
        /// Gets or sets the capital stock (currency).
        /// </summary>
        public double? CapitalStock { get; set; }

        /// <summary>
        /// Gets or sets the retained earnings (currency).
        /// </summary>
        public double? RetainedEarnings { get; set; }

        /// <summary>
        /// Gets or sets the other liabilities (currency).
        /// </summary>
        [JsonPropertyName("OtherLiab")]
        public double? OtherLiabilities { get; set; }

        /// <summary>
        /// Gets or sets the goodwill (currency).
        /// </summary>
        public double? GoodWill { get; set; }

        /// <summary>
        /// Gets or sets the other assets (currency).
        /// </summary>
        public double? OtherAssets { get; set; }

        /// <summary>
        /// Gets or sets the cash (currency).
        /// </summary>
        public double? Cash { get; set; }

        /// <summary>
        /// Gets or sets the cash and equivalents (currency).
        /// </summary>
        public double? CashAndEquivalents { get; set; }

        /// <summary>
        /// Gets or sets the total current liabilities (currency).
        /// </summary>
        public double? TotalCurrentLiabilities { get; set; }

        /// <summary>
        /// Gets or sets the current deferred revenue (currency).
        /// </summary>
        public double? CurrentDeferredRevenue { get; set; }

        /// <summary>
        /// Gets or sets the net debt (currency).
        /// </summary>
        public double? NetDebt { get; set; }

        /// <summary>
        /// Gets or sets the short-term debt (currency).
        /// </summary>
        public double? ShortTermDebt { get; set; }

        /// <summary>
        /// Gets or sets the short/long-term debt (currency).
        /// </summary>
        public double? ShortLongTermDebt { get; set; }

        /// <summary>
        /// Gets or sets the total short/long-term debt (currency).
        /// </summary>
        public double? ShortLongTermDebtTotal { get; set; }

        /// <summary>
        /// Gets or sets the other stockholder equity (currency).
        /// </summary>
        public double? OtherStockholderEquity { get; set; }

        /// <summary>
        /// Gets or sets the property, plant and equipment (currency).
        /// </summary>
        public double? PropertyPlantEquipment { get; set; }

        /// <summary>
        /// Gets or sets the total current assets (currency).
        /// </summary>
        public double? TotalCurrentAssets { get; set; }

        /// <summary>
        /// Gets or sets the long-term investments (currency).
        /// </summary>
        public double? LongTermInvestments { get; set; }

        /// <summary>
        /// Gets or sets the net tangible assets (currency).
        /// </summary>
        public double? NetTangibleAssets { get; set; }

        /// <summary>
        /// Gets or sets the short-term investments (currency).
        /// </summary>
        public double? ShortTermInvestments { get; set; }

        /// <summary>
        /// Gets or sets the net receivables (currency).
        /// </summary>
        public double? NetReceivables { get; set; }

        /// <summary>
        /// Gets or sets the long-term debt (currency).
        /// </summary>
        public double? LongTermDebt { get; set; }

        /// <summary>
        /// Gets or sets the inventory (currency).
        /// </summary>
        public double? Inventory { get; set; }

        /// <summary>
        /// Gets or sets the accounts payable (currency).
        /// </summary>
        public double? AccountsPayable { get; set; }

        /// <summary>
        /// Gets or sets the total permanent equity (currency).
        /// </summary>
        public double? TotalPermanentEquity { get; set; }

        /// <summary>
        /// Gets or sets the non-controlling interest in the consolidated entity (currency).
        /// </summary>
        public double? NonControllingInterestInConsolidatedEntity { get; set; }

        /// <summary>
        /// Gets or sets the temporary equity redeemable non-controlling interests (currency).
        /// </summary>
        public double? TemporaryEquityRedeemableNonControllingInterests { get; set; }

        /// <summary>
        /// Gets or sets the accumulated other comprehensive income (currency).
        /// </summary>
        public double? AccumulatedOtherComprehensiveIncome { get; set; }

        /// <summary>
        /// Gets or sets the additional paid-in capital (currency).
        /// </summary>
        public double? AdditionalPaidInCapital { get; set; }

        /// <summary>
        /// Gets or sets the common stock total equity (currency).
        /// </summary>
        public double? CommonStockTotalEquity { get; set; }

        /// <summary>
        /// Gets or sets the preferred stock total equity (currency).
        /// </summary>
        public double? PreferredStockTotalEquity { get; set; }

        /// <summary>
        /// Gets or sets the retained earnings total equity (currency).
        /// </summary>
        public double? RetainedEarningsTotalEquity { get; set; }

        /// <summary>
        /// Gets or sets the treasury stock (currency).
        /// </summary>
        public double? TreasuryStock { get; set; }

        /// <summary>
        /// Gets or sets the accumulated amortization (currency).
        /// </summary>
        public double? AccumulatedAmortization { get; set; }

        /// <summary>
        /// Gets or sets the other non-current assets (currency).
        /// </summary>
        [JsonPropertyName("NonCurrrentAssetsOther")]
        public double? NonCurrentAssetsOther { get; set; }

        /// <summary>
        /// Gets or sets the deferred long-term asset charges (currency).
        /// </summary>
        public double? DeferredLongTermAssetCharges { get; set; }

        /// <summary>
        /// Gets or sets the total non-current assets (currency).
        /// </summary>
        public double? NonCurrentAssetsTotal { get; set; }

        /// <summary>
        /// Gets or sets the capital lease obligations (currency).
        /// </summary>
        public double? CapitalLeaseObligations { get; set; }

        /// <summary>
        /// Gets or sets the total long-term debt (currency).
        /// </summary>
        public double? LongTermDebtTotal { get; set; }

        /// <summary>
        /// Gets or sets the other non-current liabilities (currency).
        /// </summary>
        public double? NonCurrentLiabilitiesOther { get; set; }

        /// <summary>
        /// Gets or sets the total non-current liabilities (currency).
        /// </summary>
        public double? NonCurrentLiabilitiesTotal { get; set; }

        /// <summary>
        /// Gets or sets the negative goodwill (currency).
        /// </summary>
        public double? NegativeGoodwill { get; set; }

        /// <summary>
        /// Gets or sets the warrants (currency).
        /// </summary>
        public double? Warrants { get; set; }

        /// <summary>
        /// Gets or sets the preferred stock redeemable (currency).
        /// </summary>
        public double? PreferredStockRedeemable { get; set; }

        /// <summary>
        /// Gets or sets the capital surplus (currency).
        /// </summary>
        public double? CapitalSurpluse { get; set; }

        /// <summary>
        /// Gets or sets the liabilities and stockholders equity (currency).
        /// </summary>
        public double? LiabilitiesAndStockholdersEquity { get; set; }

        /// <summary>
        /// Gets or sets the cash and short-term investments (currency).
        /// </summary>
        public double? CashAndShortTermInvestments { get; set; }

        /// <summary>
        /// Gets or sets the gross property, plant and equipment (currency).
        /// </summary>
        public double? PropertyPlantAndEquipmentGross { get; set; }

        /// <summary>
        /// Gets or sets the net property, plant and equipment (currency).
        /// </summary>
        public double? PropertyPlantAndEquipmentNet { get; set; }

        /// <summary>
        /// Gets or sets the accumulated depreciation (currency).
        /// </summary>
        public double? AccumulatedDepreciation { get; set; }

        /// <summary>
        /// Gets or sets the net working capital (currency).
        /// </summary>
        public double? NetWorkingCapital { get; set; }

        /// <summary>
        /// Gets or sets the net invested capital (currency).
        /// </summary>
        public double? NetInvestedCapital { get; set; }

        /// <summary>
        /// Gets or sets the common stock shares outstanding (count).
        /// </summary>
        public double? CommonStockSharesOutstanding { get; set; }
    }
}
