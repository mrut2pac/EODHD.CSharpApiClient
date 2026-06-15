using System;

namespace EODHD.CSharpApiClient.DataModel
{
    /// <summary>
    /// Represents a single historical dividend event for a symbol.
    /// </summary>
    public sealed class HistoricalDividend
    {
        /// <summary>
        /// Gets or sets the ex-dividend date.
        /// </summary>
        public DateTime? Date { get; set; }

        /// <summary>
        /// Gets or sets the date the dividend was declared.
        /// </summary>
        public DateTime? DeclarationDate { get; set; }

        /// <summary>
        /// Gets or sets the record date for dividend eligibility.
        /// </summary>
        public DateTime? RecordDate { get; set; }

        /// <summary>
        /// Gets or sets the date the dividend was paid.
        /// </summary>
        public DateTime? PaymentDate { get; set; }

        /// <summary>
        /// Gets or sets the dividend period (e.g. <c>"Quarterly"</c>).
        /// </summary>
        public string Period { get; set; }

        /// <summary>
        /// Gets or sets the adjusted dividend value per share.
        /// </summary>
        public double? Value { get; set; }

        /// <summary>
        /// Gets or sets the unadjusted dividend value per share.
        /// </summary>
        public double? UnadjustedValue { get; set; }

        /// <summary>
        /// Gets or sets the currency code the dividend is paid in.
        /// </summary>
        public string Currency { get; set; }
    }
}
