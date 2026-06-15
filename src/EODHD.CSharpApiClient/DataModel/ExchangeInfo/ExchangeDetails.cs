using System.Collections.Generic;

namespace EODHD.CSharpApiClient.DataModel.ExchangeInfo
{
    /// <summary>
    /// Represents the details and current status of a single exchange.
    /// </summary>
    public sealed class ExchangeDetails
    {
        /// <summary>
        /// Gets or sets the exchange display name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the exchange code.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets the operating market identifier code (MIC).
        /// </summary>
        public string OperatingMIC { get; set; }

        /// <summary>
        /// Gets or sets the country the exchange operates in.
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// Gets or sets the trading currency of the exchange.
        /// </summary>
        public string Currency { get; set; }

        /// <summary>
        /// Gets or sets the IANA time zone of the exchange.
        /// </summary>
        public string Timezone { get; set; }

        /// <summary>
        /// Gets or sets the exchange holidays keyed by their ordinal index.
        /// </summary>
        public Dictionary<int, ExchangeHoliday> ExchangeHolidays { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the exchange is currently open.
        /// </summary>
        public bool IsOpen { get; set; }

        /// <summary>
        /// Gets or sets the trading hours of the exchange.
        /// </summary>
        public TradingHours TradingHours { get; set; }

        /// <summary>
        /// Gets or sets the number of active tickers on the exchange.
        /// </summary>
        public long ActiveTickers { get; set; }

        /// <summary>
        /// Gets or sets the number of tickers updated on the previous day.
        /// </summary>
        public long PreviousDayUpdatedTickers { get; set; }

        /// <summary>
        /// Gets or sets the number of tickers updated.
        /// </summary>
        public long UpdatedTickers { get; set; }
    }
}
