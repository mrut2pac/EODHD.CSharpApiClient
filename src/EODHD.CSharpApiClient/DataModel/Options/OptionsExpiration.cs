using System;
using System.Text.Json.Serialization;

namespace EODHD.CSharpApiClient.DataModel.Options
{
    /// <summary>
    /// The option contracts and aggregate statistics for a single expiration date, as returned by the
    /// legacy options endpoint.
    /// </summary>
    public sealed class OptionsExpiration
    {
        /// <summary>
        /// Gets or sets the expiration date.
        /// </summary>
        [JsonPropertyName("expirationDate")]
        public DateTime? ExpirationDate { get; set; }

        /// <summary>
        /// Gets or sets the aggregate implied volatility for the expiration.
        /// </summary>
        [JsonPropertyName("impliedVolatility")]
        public double? ImpliedVolatility { get; set; }

        /// <summary>
        /// Gets or sets the total put volume.
        /// </summary>
        [JsonPropertyName("putVolume")]
        public long? PutVolume { get; set; }

        /// <summary>
        /// Gets or sets the total call volume.
        /// </summary>
        [JsonPropertyName("callVolume")]
        public long? CallVolume { get; set; }

        /// <summary>
        /// Gets or sets the put/call volume ratio.
        /// </summary>
        [JsonPropertyName("putCallVolumeRatio")]
        public double? PutCallVolumeRatio { get; set; }

        /// <summary>
        /// Gets or sets the total put open interest.
        /// </summary>
        [JsonPropertyName("putOpenInterest")]
        public long? PutOpenInterest { get; set; }

        /// <summary>
        /// Gets or sets the total call open interest.
        /// </summary>
        [JsonPropertyName("callOpenInterest")]
        public long? CallOpenInterest { get; set; }

        /// <summary>
        /// Gets or sets the put/call open-interest ratio.
        /// </summary>
        [JsonPropertyName("putCallOpenInterestRatio")]
        public double? PutCallOpenInterestRatio { get; set; }

        /// <summary>
        /// Gets or sets the number of contracts for the expiration.
        /// </summary>
        [JsonPropertyName("optionsCount")]
        public int? OptionsCount { get; set; }

        /// <summary>
        /// Gets or sets the call and put contracts for the expiration.
        /// </summary>
        [JsonPropertyName("options")]
        public OptionsByType Options { get; set; }
    }
}
