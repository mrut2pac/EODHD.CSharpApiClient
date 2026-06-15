namespace EODHD.CSharpApiClient.DataModel.Fundamental
{
    /// <summary>
    /// Represents share-ownership and short-interest statistics.
    /// </summary>
    public sealed class SharesStats
    {
        /// <summary>
        /// Gets or sets the number of shares outstanding, as a count.
        /// </summary>
        public long? SharesOutstanding { get; set; }

        /// <summary>
        /// Gets or sets the number of shares in the public float, as a count.
        /// </summary>
        public long? SharesFloat { get; set; }

        /// <summary>
        /// Gets or sets the percentage of shares held by insiders (percent).
        /// </summary>
        public double? PercentInsiders { get; set; }

        /// <summary>
        /// Gets or sets the percentage of shares held by institutions (percent).
        /// </summary>
        public double? PercentInstitutions { get; set; }

        /// <summary>
        /// Gets or sets the number of shares sold short, as a count.
        /// </summary>
        public double? SharesShort { get; set; }

        /// <summary>
        /// Gets or sets the number of shares sold short in the prior month, as a count.
        /// </summary>
        public double? SharesShortPriorMonth { get; set; }

        /// <summary>
        /// Gets or sets the short ratio.
        /// </summary>
        public double? ShortRatio { get; set; }

        /// <summary>
        /// Gets or sets the short interest as a percentage of shares outstanding (percent).
        /// </summary>
        public double? ShortPercentOutstanding { get; set; }

        /// <summary>
        /// Gets or sets the short interest as a percentage of the float (percent).
        /// </summary>
        public double? ShortPercentFloat { get; set; }
    }
}
