using System.Text.Json.Serialization;

namespace EODHD.CSharpApiClient.DataModel
{
    /// <summary>
    /// Represents a single historical stock-price record enriched with the bulk-feed symbol and exchange.
    /// </summary>
    public sealed class BulkHistoricalStockPrice : HistoricalStockPrice
    {
        /// <summary>
        /// Gets or sets the symbol the price applies to.
        /// </summary>
        [JsonPropertyName("code")]
        public string Symbol { get; set; }

        /// <summary>
        /// Gets or sets the exchange the symbol trades on.
        /// </summary>
        [JsonPropertyName("exchange_short_name")]
        public string Exchange { get; set; }
    }
}
