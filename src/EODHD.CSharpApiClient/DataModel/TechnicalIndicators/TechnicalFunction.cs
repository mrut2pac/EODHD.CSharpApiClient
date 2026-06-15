namespace EODHD.CSharpApiClient.DataModel.TechnicalIndicators
{
    /// <summary>
    /// The technical-analysis function requested from the EODHD technical indicators endpoint.
    /// Each value maps to the API's <c>function</c> query parameter.
    /// </summary>
    public enum TechnicalFunction
    {
        /// <summary>
        /// Average traded volume (<c>avgvol</c>).
        /// </summary>
        AverageVolume,

        /// <summary>
        /// Average traded volume in currency (<c>avgvolccy</c>).
        /// </summary>
        AverageVolumeByCurrency,

        /// <summary>
        /// Simple moving average (<c>sma</c>).
        /// </summary>
        Sma,

        /// <summary>
        /// Exponential moving average (<c>ema</c>).
        /// </summary>
        Ema,

        /// <summary>
        /// Weighted moving average (<c>wma</c>).
        /// </summary>
        Wma,

        /// <summary>
        /// Volatility (<c>volatility</c>).
        /// </summary>
        Volatility,

        /// <summary>
        /// Stochastic oscillator (<c>stochastic</c>) — returns <c>k</c> and <c>d</c>.
        /// </summary>
        Stochastic,

        /// <summary>
        /// Relative strength index (<c>rsi</c>).
        /// </summary>
        Rsi,

        /// <summary>
        /// Standard deviation (<c>stddev</c>).
        /// </summary>
        StandardDeviation,

        /// <summary>
        /// Stochastic RSI (<c>stochrsi</c>) — returns <c>k</c> and <c>d</c>.
        /// </summary>
        StochasticRsi,

        /// <summary>
        /// Linear-regression slope (<c>slope</c>).
        /// </summary>
        Slope,

        /// <summary>
        /// Directional movement index (<c>dmi</c>) — returns <c>plus_di</c> and <c>minus_di</c>.
        /// </summary>
        DirectionalMovementIndex,

        /// <summary>
        /// Average directional index (<c>adx</c>).
        /// </summary>
        Adx,

        /// <summary>
        /// Moving average convergence/divergence (<c>macd</c>) — returns <c>macd</c>, <c>signal</c>, <c>divergence</c>.
        /// </summary>
        Macd,

        /// <summary>
        /// Average true range (<c>atr</c>).
        /// </summary>
        Atr,

        /// <summary>
        /// Commodity channel index (<c>cci</c>).
        /// </summary>
        Cci,

        /// <summary>
        /// Parabolic SAR (<c>sar</c>).
        /// </summary>
        ParabolicSar,

        /// <summary>
        /// Beta relative to another instrument (<c>beta</c>) — requires the <c>code2</c> parameter.
        /// </summary>
        Beta,

        /// <summary>
        /// Bollinger Bands (<c>bbands</c>) — returns <c>uband</c>, <c>mband</c>, <c>lband</c>.
        /// </summary>
        BollingerBands,

        /// <summary>
        /// Split-adjusted OHLC (<c>splitadjusted</c>) — returns <c>open</c>, <c>high</c>, <c>low</c>, <c>close</c>.
        /// </summary>
        SplitAdjusted,
    }
}
