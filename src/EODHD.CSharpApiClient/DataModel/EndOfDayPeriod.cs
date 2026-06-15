namespace EODHD.CSharpApiClient.DataModel
{
    /// <summary>
    /// Aggregation period for end-of-day historical price data. The enum value is the single-character
    /// code expected by the EODHD <c>period</c> query parameter.
    /// </summary>
    public enum EndOfDayPeriod
    {
        /// <summary>
        /// Daily bars.
        /// </summary>
        Daily = 'd',

        /// <summary>
        /// Weekly bars.
        /// </summary>
        Weekly = 'w',

        /// <summary>
        /// Monthly bars.
        /// </summary>
        Monthly = 'm',
    }
}
