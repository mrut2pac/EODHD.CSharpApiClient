using System.Text.Json.Serialization;

namespace EODHD.CSharpApiClient.DataModel
{
    /// <summary>
    /// Represents a single historical stock-split event enriched with the bulk-feed symbol and exchange.
    /// </summary>
    public sealed class BulkHistoricalSplit : HistoricalSplit
    {
        /// <summary>
        /// Gets or sets the symbol the split applies to.
        /// </summary>
        [JsonPropertyName("code")]
        public string Symbol { get; set; }

        /// <summary>
        /// Gets or sets the exchange the symbol trades on.
        /// </summary>
        public string Exchange { get; set; }
    }
}
