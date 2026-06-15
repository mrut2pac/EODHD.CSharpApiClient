using System.Text.Json.Serialization;

namespace EODHD.CSharpApiClient.DataModel.Options
{
    /// <summary>
    /// The call and put contracts for a single expiration, as returned by the legacy options endpoint.
    /// </summary>
    public sealed class OptionsByType
    {
        /// <summary>
        /// Gets or sets the call contracts.
        /// </summary>
        [JsonPropertyName("CALL")]
        public OptionContract[] Call { get; set; }

        /// <summary>
        /// Gets or sets the put contracts.
        /// </summary>
        [JsonPropertyName("PUT")]
        public OptionContract[] Put { get; set; }
    }
}
