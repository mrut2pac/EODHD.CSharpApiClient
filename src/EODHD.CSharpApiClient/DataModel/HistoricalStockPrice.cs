using System;
using System.Text.Json.Serialization;

namespace EODHD.CSharpApiClient.DataModel
{
    /// <summary>
    /// Represents a single end-of-day OHLCV bar for a symbol.
    /// </summary>
    public class HistoricalStockPrice
    {
        /// <summary>
        /// Gets or sets the trading date of the bar.
        /// </summary>
        public DateTime? Date { get; set; }

        /// <summary>
        /// Gets or sets the opening price.
        /// </summary>
        public double Open { get; set; }

        /// <summary>
        /// Gets or sets the high price.
        /// </summary>
        public double High { get; set; }

        /// <summary>
        /// Gets or sets the low price.
        /// </summary>
        public double Low { get; set; }

        /// <summary>
        /// Gets or sets the closing price.
        /// </summary>
        public double Close { get; set; }

        /// <summary>
        /// Gets or sets the split- and dividend-adjusted closing price.
        /// </summary>
        [JsonPropertyName("Adjusted_close")]
        public double AdjustedClose { get; set; }

        /// <summary>
        /// Gets the adjusted low price (rounded to 4 decimals).
        /// </summary>
        [JsonIgnore]
        public double AdjustedLow => Math.Round(this.AdjustmentCoeficient * this.Low, 4);

        /// <summary>
        /// Gets the adjusted high price (rounded to 4 decimals).
        /// </summary>
        [JsonIgnore]
        public double AdjustedHigh => Math.Round(this.AdjustmentCoeficient * this.High, 4);

        /// <summary>
        /// Gets the adjusted open price (rounded to 4 decimals).
        /// </summary>
        [JsonIgnore]
        public double AdjustedOpen => Math.Round(this.AdjustmentCoeficient * this.Open, 4);

        /// <summary>
        /// Gets or sets the traded volume. Typed as <see cref="decimal"/> because high-volume penny
        /// stocks can exceed the range of <see cref="long"/>.
        /// </summary>
        public decimal? Volume { get; set; }

        /// <summary>
        /// Gets the adjusted volume, or <c>null</c> when volume is unavailable.
        /// </summary>
        [JsonIgnore]
        public double? AdjustedVolume => this.Volume.HasValue ? (double)this.Volume.Value / this.AdjustmentCoeficient : (double?)null;

        /// <summary>
        /// Gets the ratio of adjusted close to close used to derive the other adjusted values.
        /// </summary>
        [JsonIgnore]
        private double AdjustmentCoeficient => this.AdjustedClose / this.Close;
    }
}
