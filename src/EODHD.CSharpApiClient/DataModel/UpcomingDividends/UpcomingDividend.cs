using System;
using System.Text.Json.Serialization;

namespace EODHD.CSharpApiClient.DataModel.UpcomingDividends
{
    /// <summary>
    /// An entry in the upcoming-dividends calendar: the ex-dividend date for a symbol.
    /// </summary>
    public sealed class UpcomingDividend
    {
        /// <summary>
        /// Gets or sets the ex-dividend date.
        /// </summary>
        [JsonPropertyName("date")]
        public DateTime? Date { get; set; }

        /// <summary>
        /// Gets or sets the symbol (e.g. <c>"AAPL.US"</c>).
        /// </summary>
        [JsonPropertyName("symbol")]
        public string Symbol { get; set; }
    }
}
