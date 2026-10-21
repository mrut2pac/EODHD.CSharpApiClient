using System;
using System.Text.Json.Serialization;

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
        /// Gets or sets the raw wire value of the membership start date.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("StartDate")]
        private string StartDateStr { get; set; }

        /// <summary>
        /// Gets the membership start date, parsed from the raw value.
        /// </summary>
        [JsonIgnore]
        public DateTime? StartDate => Utilities.ParseDate(this.StartDateStr);

        /// <summary>
        /// Gets or sets the raw wire value of the membership end date.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("EndDate")]
        private string EndDateStr { get; set; }

        /// <summary>
        /// Gets the membership end date, parsed from the raw value.
        /// </summary>
        [JsonIgnore]
        public DateTime? EndDate => Utilities.ParseDate(this.EndDateStr);

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
