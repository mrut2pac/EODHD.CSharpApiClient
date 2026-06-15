using System.Text.Json.Serialization;

namespace EODHD.CSharpApiClient.DataModel.BulkFundamental
{
    /// <summary>
    /// Represents a single income-statement snapshot for one reporting period.
    /// </summary>
    public sealed class IncomeStatementData
    {
        /// <summary>
        /// Gets or sets the reporting-period end date (ISO date).
        /// </summary>
        public string Date { get; set; }

        /// <summary>
        /// Gets or sets the filing date (ISO date).
        /// </summary>
        [JsonPropertyName("filing_date")]
        public string FilingDate { get; set; }

        /// <summary>
        /// Gets or sets the reporting currency symbol.
        /// </summary>
        [JsonPropertyName("currency_symbol")]
        public string CurrencySymbol { get; set; }

        /// <summary>
        /// Gets or sets the research and development expense (currency).
        /// </summary>
        public double? ResearchDevelopment { get; set; }

        /// <summary>
        /// Gets or sets the effect of accounting charges (currency).
        /// </summary>
        public double? EffectOfAccountingCharges { get; set; }

        /// <summary>
        /// Gets or sets the income before tax (currency).
        /// </summary>
        public double? IncomeBeforeTax { get; set; }

        /// <summary>
        /// Gets or sets the minority interest (currency).
        /// </summary>
        public double? MinorityInterest { get; set; }

        /// <summary>
        /// Gets or sets the net income (currency).
        /// </summary>
        public double? NetIncome { get; set; }

        /// <summary>
        /// Gets or sets the selling, general and administrative expense (currency).
        /// </summary>
        public double? SellingGeneralAdministrative { get; set; }

        /// <summary>
        /// Gets or sets the selling and marketing expenses (currency).
        /// </summary>
        public double? SellingAndMarketingExpenses { get; set; }

        /// <summary>
        /// Gets or sets the gross profit (currency).
        /// </summary>
        public double? GrossProfit { get; set; }

        /// <summary>
        /// Gets or sets the reconciled depreciation (currency).
        /// </summary>
        public double? ReconciledDepreciation { get; set; }

        /// <summary>
        /// Gets or sets the earnings before interest and taxes (currency).
        /// </summary>
        public double? Ebit { get; set; }

        /// <summary>
        /// Gets or sets the earnings before interest, taxes, depreciation and amortization (currency).
        /// </summary>
        public double? Ebitda { get; set; }

        /// <summary>
        /// Gets or sets the depreciation and amortization (currency).
        /// </summary>
        public double? DepreciationAndAmortization { get; set; }

        /// <summary>
        /// Gets or sets the net other non-operating income (currency).
        /// </summary>
        public double? NonOperatingIncomeNetOther { get; set; }

        /// <summary>
        /// Gets or sets the operating income (currency).
        /// </summary>
        public double? OperatingIncome { get; set; }

        /// <summary>
        /// Gets or sets the other operating expenses (currency).
        /// </summary>
        public double? OtherOperatingExpenses { get; set; }

        /// <summary>
        /// Gets or sets the interest expense (currency).
        /// </summary>
        public double? InterestExpense { get; set; }

        /// <summary>
        /// Gets or sets the tax provision (currency).
        /// </summary>
        public double? TaxProvision { get; set; }

        /// <summary>
        /// Gets or sets the interest income (currency).
        /// </summary>
        public double? InterestIncome { get; set; }

        /// <summary>
        /// Gets or sets the net interest income (currency).
        /// </summary>
        public double? NetInterestIncome { get; set; }

        /// <summary>
        /// Gets or sets the extraordinary items (currency).
        /// </summary>
        public double? ExtraordinaryItems { get; set; }

        /// <summary>
        /// Gets or sets the non-recurring items (currency).
        /// </summary>
        public double? NonRecurring { get; set; }

        /// <summary>
        /// Gets or sets the other items (currency).
        /// </summary>
        public double? OtherItems { get; set; }

        /// <summary>
        /// Gets or sets the income tax expense (currency).
        /// </summary>
        public double? IncomeTaxExpense { get; set; }

        /// <summary>
        /// Gets or sets the total revenue (currency).
        /// </summary>
        public double? TotalRevenue { get; set; }

        /// <summary>
        /// Gets or sets the total operating expenses (currency).
        /// </summary>
        public double? TotalOperatingExpenses { get; set; }

        /// <summary>
        /// Gets or sets the cost of revenue (currency).
        /// </summary>
        public double? CostOfRevenue { get; set; }

        /// <summary>
        /// Gets or sets the total net other income/expense (currency).
        /// </summary>
        public double? TotalOtherIncomeExpenseNet { get; set; }

        /// <summary>
        /// Gets or sets the discontinued operations (currency).
        /// </summary>
        public double? DiscontinuedOperations { get; set; }

        /// <summary>
        /// Gets or sets the net income from continuing operations (currency).
        /// </summary>
        public double? NetIncomeFromContinuingOps { get; set; }

        /// <summary>
        /// Gets or sets the net income applicable to common shares (currency).
        /// </summary>
        public double? NetIncomeApplicableToCommonShares { get; set; }

        /// <summary>
        /// Gets or sets the preferred stock and other adjustments (currency).
        /// </summary>
        public double? PreferredStockAndOtherAdjustments { get; set; }
    }
}
