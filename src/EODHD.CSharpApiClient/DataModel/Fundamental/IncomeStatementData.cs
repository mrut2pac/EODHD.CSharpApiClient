using System;
using System.Text.Json.Serialization;

namespace EODHD.CSharpApiClient.DataModel.Fundamental
{
    /// <summary>
    /// Represents a single income statement period.
    /// </summary>
    public sealed class IncomeStatementData
    {
        /// <summary>
        /// Gets or sets the raw period date string, as an ISO date.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("Date")]
        private string DateStr { get; set; }

        /// <summary>
        /// Gets the period date, parsed from the raw string.
        /// </summary>
        [JsonIgnore]
        public DateTime? Date => Utilities.ParseDate(this.DateStr);

        /// <summary>
        /// Gets or sets the raw filing date string, as an ISO date.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("Filing_date")]
        private string filingDateStr { get; set; }

        /// <summary>
        /// Gets the filing date, parsed from the raw string.
        /// </summary>
        [JsonIgnore]
        public DateTime? FilingDate => Utilities.ParseDate(this.filingDateStr);

        /// <summary>
        /// Gets or sets the currency symbol.
        /// </summary>
        [JsonPropertyName("Currency_symbol")]
        public string CurrencySymbol { get; set; }

        /// <summary>
        /// Gets or sets the research and development expense, as a currency amount.
        /// </summary>
        public double? ResearchDevelopment { get; set; }

        /// <summary>
        /// Gets or sets the effect of accounting charges, as a currency amount.
        /// </summary>
        public double? EffectOfAccountingCharges { get; set; }

        /// <summary>
        /// Gets or sets the income before tax, as a currency amount.
        /// </summary>
        public double? IncomeBeforeTax { get; set; }

        /// <summary>
        /// Gets or sets the minority interest, as a currency amount.
        /// </summary>
        public double? MinorityInterest { get; set; }

        /// <summary>
        /// Gets or sets the net income, as a currency amount.
        /// </summary>
        public double? NetIncome { get; set; }

        /// <summary>
        /// Gets or sets the selling, general and administrative expense, as a currency amount.
        /// </summary>
        public double? SellingGeneralAdministrative { get; set; }

        /// <summary>
        /// Gets or sets the selling and marketing expenses, as a currency amount.
        /// </summary>
        public double? SellingAndMarketingExpenses { get; set; }

        /// <summary>
        /// Gets or sets the gross profit, as a currency amount.
        /// </summary>
        public double? GrossProfit { get; set; }

        /// <summary>
        /// Gets or sets the reconciled depreciation, as a currency amount.
        /// </summary>
        public double? ReconciledDepreciation { get; set; }

        /// <summary>
        /// Gets or sets the earnings before interest and taxes (EBIT), as a currency amount.
        /// </summary>
        public double? Ebit { get; set; }

        /// <summary>
        /// Gets or sets the earnings before interest, taxes, depreciation and amortization (EBITDA), as a currency amount.
        /// </summary>
        public double? Ebitda { get; set; }

        /// <summary>
        /// Gets or sets the depreciation and amortization, as a currency amount.
        /// </summary>
        public double? DepreciationAndAmortization { get; set; }

        /// <summary>
        /// Gets or sets the non-operating income, net other, as a currency amount.
        /// </summary>
        public double? NonOperatingIncomeNetOther { get; set; }

        /// <summary>
        /// Gets or sets the operating income, as a currency amount.
        /// </summary>
        public double? OperatingIncome { get; set; }

        /// <summary>
        /// Gets or sets the other operating expenses, as a currency amount.
        /// </summary>
        public double? OtherOperatingExpenses { get; set; }

        /// <summary>
        /// Gets or sets the interest expense, as a currency amount.
        /// </summary>
        public double? InterestExpense { get; set; }

        /// <summary>
        /// Gets or sets the tax provision, as a currency amount.
        /// </summary>
        public double? TaxProvision { get; set; }

        /// <summary>
        /// Gets or sets the interest income, as a currency amount.
        /// </summary>
        public double? InterestIncome { get; set; }

        /// <summary>
        /// Gets or sets the net interest income, as a currency amount.
        /// </summary>
        public double? NetInterestIncome { get; set; }

        /// <summary>
        /// Gets or sets the extraordinary items, as a currency amount.
        /// </summary>
        public double? ExtraordinaryItems { get; set; }

        /// <summary>
        /// Gets or sets the non-recurring items, as a currency amount.
        /// </summary>
        public double? NonRecurring { get; set; }

        /// <summary>
        /// Gets or sets the other items, as a currency amount.
        /// </summary>
        public double? OtherItems { get; set; }

        /// <summary>
        /// Gets or sets the income tax expense, as a currency amount.
        /// </summary>
        public double? IncomeTaxExpense { get; set; }

        /// <summary>
        /// Gets or sets the total revenue, as a currency amount.
        /// </summary>
        public double? TotalRevenue { get; set; }

        /// <summary>
        /// Gets or sets the total operating expenses, as a currency amount.
        /// </summary>
        public double? TotalOperatingExpenses { get; set; }

        /// <summary>
        /// Gets or sets the cost of revenue, as a currency amount.
        /// </summary>
        public double? CostOfRevenue { get; set; }

        /// <summary>
        /// Gets or sets the total other income/expense, net, as a currency amount.
        /// </summary>
        public double? TotalOtherIncomeExpenseNet { get; set; }

        /// <summary>
        /// Gets or sets the discontinued operations, as a currency amount.
        /// </summary>
        public double? DiscontinuedOperations { get; set; }

        /// <summary>
        /// Gets or sets the net income from continuing operations, as a currency amount.
        /// </summary>
        public double? NetIncomeFromContinuingOps { get; set; }

        /// <summary>
        /// Gets or sets the net income applicable to common shares, as a currency amount.
        /// </summary>
        public double? NetIncomeApplicableToCommonShares { get; set; }

        /// <summary>
        /// Gets or sets the preferred stock and other adjustments, as a currency amount.
        /// </summary>
        public double? PreferredStockAndOtherAdjustments { get; set; }
    }
}
