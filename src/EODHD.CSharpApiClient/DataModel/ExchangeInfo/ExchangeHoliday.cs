using System;

namespace EODHD.CSharpApiClient.DataModel.ExchangeInfo
{
    /// <summary>
    /// Represents a single holiday observed by an exchange.
    /// </summary>
    public sealed class ExchangeHoliday
    {
        /// <summary>
        /// Gets or sets the holiday name.
        /// </summary>
        public string Holiday { get; set; }

        /// <summary>
        /// Gets or sets the date of the holiday.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets the holiday type.
        /// </summary>
        public string Type { get; set; }
    }
}
