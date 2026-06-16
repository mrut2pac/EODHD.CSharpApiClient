using System.Text.Json.Serialization;

namespace EODHD.CSharpApiClient.DataModel.BulkFundamental
{
    /// <summary>
    /// Represents a single cash-flow snapshot for one reporting period.
    /// </summary>
    public sealed class CashFlowData
    {
        /// <summary>
        /// Gets or sets the reporting-period end date (ISO date).
        /// </summary>
        public string Date { get; set; }

        /// <summary>
        /// Gets or sets the filing date (ISO date).
        /// </summary>
        public string FilingDate { get; set; }

        /// <summary>
        /// Gets or sets the reporting currency symbol.
        /// </summary>
        [JsonPropertyName("currency_symbol")]
        public string CurrencySymbol { get; set; }

        /// <summary>
        /// Gets or sets the investments (currency).
        /// </summary>
        public double? Investments { get; set; }

        /// <summary>
        /// Gets or sets the change to liabilities (currency).
        /// </summary>
        public double? ChangeToLiabilities { get; set; }

        /// <summary>
        /// Gets or sets the total cash flows from investing activities (currency).
        /// </summary>
        public double? TotalCashflowsFromInvestingActivities { get; set; }

        /// <summary>
        /// Gets or sets the net borrowings (currency).
        /// </summary>
        public double? NetBorrowings { get; set; }

        /// <summary>
        /// Gets or sets the total cash from financing activities (currency).
        /// </summary>
        public double? TotalCashFromFinancingActivities { get; set; }

        /// <summary>
        /// Gets or sets the change to operating activities (currency).
        /// </summary>
        public double? ChangeToOperatingActivities { get; set; }

        /// <summary>
        /// Gets or sets the net income (currency).
        /// </summary>
        public double? NetIncome { get; set; }

        /// <summary>
        /// Gets or sets the change in cash (currency).
        /// </summary>
        public double? ChangeInCash { get; set; }

        /// <summary>
        /// Gets or sets the beginning-of-period cash flow (currency).
        /// </summary>
        public double? BeginPeriodCashFlow { get; set; }

        /// <summary>
        /// Gets or sets the end-of-period cash flow (currency).
        /// </summary>
        public double? EndPeriodCashFlow { get; set; }

        /// <summary>
        /// Gets or sets the total cash from operating activities (currency).
        /// </summary>
        public double? TotalCashFromOperatingActivities { get; set; }

        /// <summary>
        /// Gets or sets the issuance of capital stock (currency).
        /// </summary>
        public double? IssuanceOfCapitalStock { get; set; }

        /// <summary>
        /// Gets or sets the depreciation (currency).
        /// </summary>
        public double? Depreciation { get; set; }

        /// <summary>
        /// Gets or sets the other cash flows from investing activities (currency).
        /// </summary>
        public double? OtherCashflowsFromInvestingActivities { get; set; }

        /// <summary>
        /// Gets or sets the dividends paid (currency).
        /// </summary>
        public double? DividendsPaid { get; set; }

        /// <summary>
        /// Gets or sets the change to inventory (currency).
        /// </summary>
        public double? ChangeToInventory { get; set; }

        /// <summary>
        /// Gets or sets the change to account receivables (currency).
        /// </summary>
        public double? ChangeToAccountReceivables { get; set; }

        /// <summary>
        /// Gets or sets the sale/purchase of stock (currency).
        /// </summary>
        public double? SalePurchaseOfStock { get; set; }

        /// <summary>
        /// Gets or sets the other cash flows from financing activities (currency).
        /// </summary>
        public double? OtherCashflowsFromFinancingActivities { get; set; }

        /// <summary>
        /// Gets or sets the change to net income (currency).
        /// </summary>
        public double? ChangeToNetincome { get; set; }

        /// <summary>
        /// Gets or sets the capital expenditures (currency).
        /// </summary>
        public double? CapitalExpenditures { get; set; }

        /// <summary>
        /// Gets or sets the change in receivables (currency).
        /// </summary>
        public double? ChangeReceivables { get; set; }

        /// <summary>
        /// Gets or sets the other operating cash flows (currency).
        /// </summary>
        public double? CashFlowsOtherOperating { get; set; }

        /// <summary>
        /// Gets or sets the exchange-rate changes (currency).
        /// </summary>
        public double? ExchangeRateChanges { get; set; }

        /// <summary>
        /// Gets or sets the changes in cash and cash equivalents (currency).
        /// </summary>
        public double? CashAndCashEquivalentsChanges { get; set; }

        /// <summary>
        /// Gets or sets the change in working capital (currency).
        /// </summary>
        public double? ChangeInWorkingCapital { get; set; }

        /// <summary>
        /// Gets or sets the stock-based compensation (currency).
        /// </summary>
        public double? StockBasedCompensation { get; set; }

        /// <summary>
        /// Gets or sets the other non-cash items (currency).
        /// </summary>
        public double? OtherNonCashItems { get; set; }

        /// <summary>
        /// Gets or sets the free cash flow (currency).
        /// </summary>
        public double? FreeCashFlow { get; set; }
    }
}
