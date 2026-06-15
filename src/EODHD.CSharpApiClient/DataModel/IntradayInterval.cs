namespace EODHD.CSharpApiClient.DataModel
{
    /// <summary>
    /// Supported bar intervals for the intraday historical data endpoint.
    /// </summary>
    public enum IntradayInterval
    {
        /// <summary>
        /// One-minute bars (<c>1m</c>).
        /// </summary>
        OneMinute,

        /// <summary>
        /// Five-minute bars (<c>5m</c>) — the EODHD default.
        /// </summary>
        FiveMinutes,

        /// <summary>
        /// One-hour bars (<c>1h</c>).
        /// </summary>
        OneHour,
    }
}
