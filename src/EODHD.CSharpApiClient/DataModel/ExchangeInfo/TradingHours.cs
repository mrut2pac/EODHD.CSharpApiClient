namespace EODHD.CSharpApiClient.DataModel.ExchangeInfo
{
    /// <summary>
    /// Represents the trading hours of an exchange.
    /// </summary>
    public sealed class TradingHours
    {
        /// <summary>
        /// Gets or sets the local open time.
        /// </summary>
        public string Open { get; set; }

        /// <summary>
        /// Gets or sets the local close time.
        /// </summary>
        public string Close { get; set; }

        /// <summary>
        /// Gets or sets the open time in UTC.
        /// </summary>
        public string OpenUTC { get; set; }

        /// <summary>
        /// Gets or sets the close time in UTC.
        /// </summary>
        public string CloseUTC { get; set; }

        /// <summary>
        /// Gets or sets the working days of the exchange.
        /// </summary>
        public string WorkingDays { get; set; }
    }
}
