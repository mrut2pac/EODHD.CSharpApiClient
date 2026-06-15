using System.Text.Json.Serialization;

namespace EODHD.CSharpApiClient.DataModel.BulkFundamental
{
    /// <summary>
    /// Represents the technical indicators section of a company's bulk fundamentals.
    /// </summary>
    public sealed class Technicals
    {
        /// <summary>
        /// Gets or sets the beta (ratio).
        /// </summary>
        public double? Beta { get; set; }

        /// <summary>
        /// Gets or sets the 52-week high (price).
        /// </summary>
        [JsonPropertyName("52WeekHigh")]
        public double? FiftyTwoWeekHigh { get; set; }

        /// <summary>
        /// Gets or sets the 52-week low (price).
        /// </summary>
        [JsonPropertyName("52WeekLow")]
        public double? FiftyTwoWeekLow { get; set; }

        /// <summary>
        /// Gets or sets the 50-day moving average (price).
        /// </summary>
        [JsonPropertyName("50DayMA")]
        public double? FiftyDayMA { get; set; }

        /// <summary>
        /// Gets or sets the 200-day moving average (price).
        /// </summary>
        [JsonPropertyName("200DayMA")]
        public double? TwoHundredDay200MA { get; set; }

        /// <summary>
        /// Gets or sets the number of shares short (count).
        /// </summary>
        public decimal? SharesShort { get; set; }

        /// <summary>
        /// Gets or sets the number of shares short in the prior month (count).
        /// </summary>
        public decimal? SharesShortPriorMonth { get; set; }

        /// <summary>
        /// Gets or sets the short ratio.
        /// </summary>
        public double? ShortRatio { get; set; }

        /// <summary>
        /// Gets or sets the short percent (percent).
        /// </summary>
        public double? ShortPercent { get; set; }
    }
}
