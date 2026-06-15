using System;
using System.Text.Json.Serialization;

using EODHD.CSharpApiClient.JsonSupport;

namespace EODHD.CSharpApiClient.DataModel
{
    /// <summary>
    /// Represents a single intraday OHLCV bar for a symbol.
    /// </summary>
    public sealed class IntradayHistoricalStockPrice
    {
        /// <summary>
        /// Gets or sets the Unix epoch timestamp (seconds) of the bar.
        /// </summary>
        public long? Timestamp { get; set; }

        /// <summary>
        /// Gets or sets the GMT offset in seconds applied to the bar time.
        /// </summary>
        public double? GmtOffset { get; set; }

        /// <summary>
        /// Gets or sets the bar date and time (UTC). EODHD sends this as a space-separated
        /// <c>"yyyy-MM-dd HH:mm:ss"</c> string, parsed via <see cref="SpaceSeparatedDateTimeConverter"/>.
        /// </summary>
        [JsonConverter(typeof(SpaceSeparatedDateTimeConverter))]
        public DateTime? DateTime { get; set; }

        /// <summary>
        /// Gets or sets the opening price.
        /// </summary>
        public double? Open { get; set; }

        /// <summary>
        /// Gets or sets the high price.
        /// </summary>
        public double? High { get; set; }

        /// <summary>
        /// Gets or sets the low price.
        /// </summary>
        public double? Low { get; set; }

        /// <summary>
        /// Gets or sets the closing price.
        /// </summary>
        public double? Close { get; set; }

        /// <summary>
        /// Gets or sets the traded volume.
        /// </summary>
        public decimal? Volume { get; set; }
    }
}
