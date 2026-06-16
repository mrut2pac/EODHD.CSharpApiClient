using System;
using System.Text.Json.Serialization;

namespace EODHD.CSharpApiClient.DataModel.Options
{
    /// <summary>
    /// An option chain for a symbol from the legacy options endpoint: the underlying's latest trade and
    /// one entry per expiration date.
    /// </summary>
    public sealed class OptionsChain
    {
        /// <summary>
        /// Gets or sets the underlying symbol code.
        /// </summary>
        [JsonPropertyName("code")]
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets the underlying exchange code.
        /// </summary>
        [JsonPropertyName("exchange")]
        public string Exchange { get; set; }

        /// <summary>
        /// Gets or sets the underlying's last trade date.
        /// </summary>
        [JsonPropertyName("lastTradeDate")]
        public DateTime? LastTradeDate { get; set; }

        /// <summary>
        /// Gets or sets the underlying's last trade price.
        /// </summary>
        [JsonPropertyName("lastTradePrice")]
        public double? LastTradePrice { get; set; }

        /// <summary>
        /// Gets or sets the per-expiration option data.
        /// </summary>
        [JsonPropertyName("data")]
        public OptionsExpiration[] Data { get; set; }
    }
}
