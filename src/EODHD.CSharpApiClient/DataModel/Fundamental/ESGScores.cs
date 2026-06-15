using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace EODHD.CSharpApiClient.DataModel.Fundamental
{
    /// <summary>
    /// Represents the ESG (environmental, social, governance) scores for a company.
    /// </summary>
    public sealed class ESGScores
    {
        /// <summary>
        /// Gets or sets the disclaimer text.
        /// </summary>
        public string Disclaimer { get; set; }

        /// <summary>
        /// Gets or sets the raw rating date string (ISO date).
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("RatingDate")]
        private string RatingDateStr { get; set; }

        /// <summary>
        /// Gets the rating date (ISO date), parsed from the raw value.
        /// </summary>
        [JsonIgnore]
        public DateTime? RatingDate => Utilities.ParseDate(this.RatingDateStr);

        /// <summary>
        /// Gets or sets the total ESG score.
        /// </summary>
        public double? TotalEsg { get; set; }

        /// <summary>
        /// Gets or sets the total ESG percentile.
        /// </summary>
        public double? TotalEsgPercentile { get; set; }

        /// <summary>
        /// Gets or sets the environment score.
        /// </summary>
        public double? EnvironmentScore { get; set; }

        /// <summary>
        /// Gets or sets the environment score percentile.
        /// </summary>
        public double? EnvironmentScorePercentile { get; set; }

        /// <summary>
        /// Gets or sets the social score.
        /// </summary>
        public double? SocialScore { get; set; }

        /// <summary>
        /// Gets or sets the social score percentile.
        /// </summary>
        public double? SocialScorePercentile { get; set; }

        /// <summary>
        /// Gets or sets the governance score.
        /// </summary>
        public double? GovernanceScore { get; set; }

        /// <summary>
        /// Gets or sets the governance score percentile.
        /// </summary>
        public double? GovernanceScorePercentile { get; set; }

        /// <summary>
        /// Gets or sets the controversy level.
        /// </summary>
        public double? ControversyLevel { get; set; }

        /// <summary>
        /// Gets or sets the activities involvement keyed by activity name.
        /// </summary>
        public Dictionary<string, ActivitiesInvolvement> ActivitiesInvolvement { get; set; }
    }
}
