using System;

namespace EODHD.CSharpApiClient.DataModel.Screener
{
    /// <summary>
    /// A single screener filter condition of the form <c>[field, operation, value]</c>, as consumed by
    /// the EODHD screener <c>filters</c> parameter. Combine several conditions to narrow the result set.
    /// </summary>
    public sealed class ScreenerFilter
    {
        /// <summary>
        /// Initialises a new filter condition.
        /// </summary>
        /// <param name="field">
        /// The field to filter on. String fields: <c>code</c>, <c>name</c>, <c>exchange</c>,
        /// <c>sector</c>, <c>industry</c>. Numeric fields: <c>market_capitalization</c>,
        /// <c>earnings_share</c>, <c>dividend_yield</c>, <c>refund_1d_p</c>, <c>refund_5d_p</c>,
        /// <c>avgvol_1d</c>, <c>avgvol_200d</c>, <c>adjusted_close</c>.
        /// </param>
        /// <param name="operation">
        /// The comparison operation. String fields support <c>"="</c> and <c>"match"</c>; numeric fields
        /// support <c>"="</c>, <c>"&gt;"</c>, <c>"&lt;"</c>, <c>"&gt;="</c>, and <c>"&lt;="</c>.
        /// </param>
        /// <param name="value">The value to compare against — a string or a number.</param>
        public ScreenerFilter(string field, string operation, object value)
        {
            if(string.IsNullOrWhiteSpace(field))
            {
                throw new ArgumentNullException(nameof(field));
            }

            if(string.IsNullOrWhiteSpace(operation))
            {
                throw new ArgumentNullException(nameof(operation));
            }

            this.Field = field;
            this.Operation = operation;
            this.Value = value;
        }

        /// <summary>
        /// Gets the field being filtered.
        /// </summary>
        public string Field { get; }

        /// <summary>
        /// Gets the comparison operation.
        /// </summary>
        public string Operation { get; }

        /// <summary>
        /// Gets the comparison value (a string or a number).
        /// </summary>
        public object Value { get; }
    }
}
