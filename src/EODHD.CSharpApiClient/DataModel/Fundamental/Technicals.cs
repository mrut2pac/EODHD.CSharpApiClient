using System.Text.Json.Serialization;

namespace EODHD.CSharpApiClient.DataModel.Fundamental
{
    /// <summary>
    /// Represents technical indicators and short-interest figures for a security.
    /// </summary>
    public sealed class Technicals
    {
        /// <summary>
        /// Gets or sets the beta coefficient.
        /// </summary>
        public double? Beta { get; set; }

        /// <summary>
        /// Gets or sets the 52-week high price.
        /// </summary>
        [JsonPropertyName("52WeekHigh")]
        public double? FiftyTwoWeekHigh { get; set; }

        /// <summary>
        /// Gets or sets the 52-week low price.
        /// </summary>
        [JsonPropertyName("52WeekLow")]
        public double? FiftyTwoWeekLow { get; set; }

        /// <summary>
        /// Gets or sets the 50-day moving average price.
        /// </summary>
        [JsonPropertyName("50DayMA")]
        public double? FiftyDayMA { get; set; }

        /// <summary>
        /// Gets or sets the 200-day moving average price.
        /// </summary>
        [JsonPropertyName("200DayMA")]
        public double? TwoHundredDayMA { get; set; }

        /// <summary>
        /// Gets or sets the number of shares sold short, as a count.
        /// </summary>
        public long? SharesShort { get; set; }

        /// <summary>
        /// Gets or sets the number of shares sold short in the prior month, as a count.
        /// </summary>
        public long? SharesShortPriorMonth { get; set; }

        /// <summary>
        /// Gets or sets the short ratio.
        /// </summary>
        public double? ShortRatio { get; set; }

        /// <summary>
        /// Gets or sets the short interest percentage (percent).
        /// </summary>
        public double? ShortPercent { get; set; }
    }
}
