using System.Text.Json.Serialization;

namespace EODHD.CSharpApiClient.DataModel
{
    /// <summary>
    /// A single match returned by the EODHD search API (active tickers only).
    /// </summary>
    public sealed class SearchResult
    {
        /// <summary>
        /// Gets or sets the ticker symbol.
        /// </summary>
        [JsonPropertyName("Code")]
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets the exchange listing code.
        /// </summary>
        [JsonPropertyName("Exchange")]
        public string Exchange { get; set; }

        /// <summary>
        /// Gets or sets the full instrument name.
        /// </summary>
        [JsonPropertyName("Name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the asset type (e.g. <c>"Common Stock"</c>, <c>"ETF"</c>).
        /// </summary>
        [JsonPropertyName("Type")]
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the exchange country.
        /// </summary>
        [JsonPropertyName("Country")]
        public string Country { get; set; }

        /// <summary>
        /// Gets or sets the trading currency.
        /// </summary>
        [JsonPropertyName("Currency")]
        public string Currency { get; set; }

        /// <summary>
        /// Gets or sets the International Securities Identification Number.
        /// </summary>
        [JsonPropertyName("ISIN")]
        public string Isin { get; set; }

        /// <summary>
        /// Gets or sets the prior closing price.
        /// </summary>
        [JsonPropertyName("previousClose")]
        public double? PreviousClose { get; set; }

        /// <summary>
        /// Gets or sets the date of the previous close.
        /// </summary>
        [JsonPropertyName("previousCloseDate")]
        public string PreviousCloseDate { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this is the instrument's primary exchange listing.
        /// </summary>
        [JsonPropertyName("isPrimary")]
        public bool? IsPrimary { get; set; }
    }
}
