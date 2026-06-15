using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

using EODHD.CSharpApiClient.JsonSupport;

namespace EODHD.CSharpApiClient.DataModel.TechnicalIndicators
{
    /// <summary>
    /// A single technical-indicator data point: a date plus the indicator's output values keyed by the
    /// field name the API returns for the requested function (e.g. <c>"sma"</c>; <c>"macd"</c>,
    /// <c>"signal"</c>, <c>"divergence"</c>; <c>"k"</c>, <c>"d"</c>; <c>"uband"</c>, <c>"mband"</c>,
    /// <c>"lband"</c>; or OHLC for split-adjusted). Deserialized by
    /// <see cref="TechnicalIndicatorPointConverter"/>, which keeps the model independent of the
    /// per-function response shape.
    /// </summary>
    [JsonConverter(typeof(TechnicalIndicatorPointConverter))]
    public sealed class TechnicalIndicatorPoint
    {
        /// <summary>
        /// Gets or sets the data point date.
        /// </summary>
        public DateTime? Date { get; set; }

        /// <summary>
        /// Gets or sets the indicator output values for this date, keyed by the API field name.
        /// </summary>
        public IReadOnlyDictionary<string, double?> Values { get; set; }

        /// <summary>
        /// Gets the single output value for single-valued indicators (e.g. SMA, RSI), or <c>null</c>
        /// when the function returns multiple values or none.
        /// </summary>
        [JsonIgnore]
        public double? Value => this.Values != null && this.Values.Count == 1 ? this.Values.Values.First() : null;
    }
}
