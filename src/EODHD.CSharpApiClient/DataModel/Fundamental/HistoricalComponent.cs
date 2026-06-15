namespace EODHD.CSharpApiClient.DataModel.Fundamental
{
    /// <summary>
    /// Represents a historical index component membership record.
    /// </summary>
    public sealed class HistoricalComponent
    {
        /// <summary>
        /// Gets or sets the component code.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets the component name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the membership start date, as an ISO date.
        /// </summary>
        public string StartDate { get; set; }

        /// <summary>
        /// Gets or sets the membership end date, as an ISO date.
        /// </summary>
        public string EndDate { get; set; }

        /// <summary>
        /// Gets or sets a flag indicating whether the component is active now.
        /// </summary>
        public int? IsActiveNow { get; set; }

        /// <summary>
        /// Gets or sets a flag indicating whether the component is delisted.
        /// </summary>
        public int? IsDelisted { get; set; }
    }
}
