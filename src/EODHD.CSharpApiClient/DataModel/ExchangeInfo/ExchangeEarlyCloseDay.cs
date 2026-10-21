using System;

namespace EODHD.CSharpApiClient.DataModel.ExchangeInfo
{
    /// <summary>
    /// Represents a scheduled early-close (half) trading day on an exchange.
    /// </summary>
    public sealed class ExchangeEarlyCloseDay
    {
        /// <summary>
        /// Gets or sets the name of the occasion (e.g. <c>"Pre-Christmas Thursday 2026"</c>).
        /// </summary>
        public string Holiday { get; set; }

        /// <summary>
        /// Gets or sets the date of the early close.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets the early-close type (e.g. <c>"official"</c>).
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the local closing time on that day (e.g. <c>"13:00"</c>).
        /// </summary>
        public string EarlyClose { get; set; }
    }
}
