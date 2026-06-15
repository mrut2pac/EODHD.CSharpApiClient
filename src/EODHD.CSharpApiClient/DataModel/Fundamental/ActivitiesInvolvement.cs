namespace EODHD.CSharpApiClient.DataModel.Fundamental
{
    /// <summary>
    /// Represents a single ESG activity and the company's level of involvement in it.
    /// </summary>
    public sealed class ActivitiesInvolvement
    {
        /// <summary>
        /// Gets or sets the activity name.
        /// </summary>
        public string Activity { get; set; }

        /// <summary>
        /// Gets or sets the company's level of involvement in the activity.
        /// </summary>
        public string Involvement { get; set; }
    }
}
