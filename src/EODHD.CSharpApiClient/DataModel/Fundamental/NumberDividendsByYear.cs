namespace EODHD.CSharpApiClient.DataModel.Fundamental
{
    /// <summary>
    /// Represents the number of dividend payments for a single year.
    /// </summary>
    public sealed class NumberDividendsByYear
    {
        /// <summary>
        /// Gets or sets the year, as a count.
        /// </summary>
        public int? Year { get; set; }

        /// <summary>
        /// Gets or sets the number of dividend payments in the year, as a count.
        /// </summary>
        public int? Count { get; set; }
    }
}
