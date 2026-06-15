namespace EODHD.CSharpApiClient.DataModel.Fundamental
{
    /// <summary>
    /// Represents a company officer.
    /// </summary>
    public sealed class Officer
    {
        /// <summary>
        /// Gets or sets the officer's name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the officer's title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the officer's year of birth.
        /// </summary>
        public string YearBorn { get; set; }
    }
}
