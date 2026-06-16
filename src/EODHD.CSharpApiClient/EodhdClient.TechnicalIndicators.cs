using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using EODHD.CSharpApiClient.DataModel.TechnicalIndicators;

namespace EODHD.CSharpApiClient
{
    public sealed partial class EodhdClient
    {
        // ================================================================
        // Technical Indicators API
        // ================================================================

        /// <summary>
        /// Returns a technical-indicator series for a symbol. The output values vary by
        /// <paramref name="function"/> and are exposed per data point in
        /// <see cref="TechnicalIndicatorPoint.Values"/>.
        /// </summary>
        /// <param name="symbol">The full EODHD symbol (e.g. <c>"AAPL.US"</c>).</param>
        /// <param name="function">The indicator function to compute.</param>
        /// <param name="period">Optional look-back period (default 50; range 2–100000).</param>
        /// <param name="from">Optional inclusive start date.</param>
        /// <param name="to">Optional inclusive end date.</param>
        /// <param name="ascending">When <see langword="true"/> (default), points are oldest-first.</param>
        /// <param name="extraParameters">
        /// Optional function-specific parameters (e.g. <c>fast_kperiod</c>, <c>slow_kperiod</c>,
        /// <c>signal_period</c>, <c>acceleration</c>, <c>maximum</c>, <c>code2</c>, <c>agg_period</c>),
        /// passed through verbatim as query parameters.
        /// </param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>The indicator data points.</returns>
        public Task<TechnicalIndicatorPoint[]> GetTechnicalIndicatorAsync(string symbol, TechnicalFunction function, int? period = null, DateTime? from = null, DateTime? to = null, bool ascending = true, IReadOnlyDictionary<string, string> extraParameters = null, CancellationToken ct = default)
        {
            if(string.IsNullOrWhiteSpace(symbol))
            {
                throw new ArgumentNullException(nameof(symbol));
            }

            List<(string Name, string Value)> query = new List<(string Name, string Value)>
            {
                ("function", TechnicalFunctionToString(function)),
                ("period", FormatInt(period)),
                ("from", FormatDate(from)),
                ("to", FormatDate(to)),
                ("order", ascending ? null : "d"),
            };

            if(extraParameters != null)
            {
                foreach(KeyValuePair<string, string> parameter in extraParameters)
                {
                    query.Add((parameter.Key, parameter.Value));
                }
            }

            return this.GetJsonAsync<TechnicalIndicatorPoint[]>(
                "technical/" + Uri.EscapeDataString(symbol),
                ct,
                query.ToArray());
        }

        /// <summary>
        /// Returns a technical-indicator series for a symbol. The output values vary by
        /// <paramref name="function"/> and are exposed per data point in
        /// <see cref="TechnicalIndicatorPoint.Values"/>.
        /// </summary>
        /// <param name="symbol">The full EODHD symbol (e.g. <c>"AAPL.US"</c>).</param>
        /// <param name="function">The indicator function to compute.</param>
        /// <param name="period">Optional look-back period (default 50; range 2–100000).</param>
        /// <param name="from">Optional inclusive start date.</param>
        /// <param name="to">Optional inclusive end date.</param>
        /// <param name="ascending">When <see langword="true"/> (default), points are oldest-first.</param>
        /// <param name="extraParameters">Optional function-specific parameters passed through verbatim.</param>
        /// <returns>The indicator data points.</returns>
        public TechnicalIndicatorPoint[] GetTechnicalIndicator(string symbol, TechnicalFunction function, int? period = null, DateTime? from = null, DateTime? to = null, bool ascending = true, IReadOnlyDictionary<string, string> extraParameters = null)
        {
            return this.GetTechnicalIndicatorAsync(symbol, function, period, from, to, ascending, extraParameters).GetAwaiter().GetResult();
        }

        private static string TechnicalFunctionToString(TechnicalFunction function)
        {
            switch(function)
            {
                case TechnicalFunction.AverageVolume:
                    return "avgvol";
                case TechnicalFunction.AverageVolumeByCurrency:
                    return "avgvolccy";
                case TechnicalFunction.Sma:
                    return "sma";
                case TechnicalFunction.Ema:
                    return "ema";
                case TechnicalFunction.Wma:
                    return "wma";
                case TechnicalFunction.Volatility:
                    return "volatility";
                case TechnicalFunction.Stochastic:
                    return "stochastic";
                case TechnicalFunction.Rsi:
                    return "rsi";
                case TechnicalFunction.StandardDeviation:
                    return "stddev";
                case TechnicalFunction.StochasticRsi:
                    return "stochrsi";
                case TechnicalFunction.Slope:
                    return "slope";
                case TechnicalFunction.DirectionalMovementIndex:
                    return "dmi";
                case TechnicalFunction.Adx:
                    return "adx";
                case TechnicalFunction.Macd:
                    return "macd";
                case TechnicalFunction.Atr:
                    return "atr";
                case TechnicalFunction.Cci:
                    return "cci";
                case TechnicalFunction.ParabolicSar:
                    return "sar";
                case TechnicalFunction.Beta:
                    return "beta";
                case TechnicalFunction.BollingerBands:
                    return "bbands";
                case TechnicalFunction.SplitAdjusted:
                    return "splitadjusted";
                default:
                    throw new ArgumentOutOfRangeException(nameof(function), function, "Unknown technical function.");
            }
        }
    }
}
