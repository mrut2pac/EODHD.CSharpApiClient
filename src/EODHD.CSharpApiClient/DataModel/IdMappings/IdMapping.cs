using System.Text.Json.Serialization;

namespace EODHD.CSharpApiClient.DataModel.IdMappings
{
    /// <summary>
    /// A mapping between the financial identifiers of a single instrument (symbol, ISIN, FIGI, LEI,
    /// CUSIP, and CIK).
    /// </summary>
    public sealed class IdMapping
    {
        /// <summary>
        /// Gets or sets the EODHD symbol (e.g. <c>"AAPL.US"</c>).
        /// </summary>
        [JsonPropertyName("symbol")]
        public string Symbol { get; set; }

        /// <summary>
        /// Gets or sets the ISIN.
        /// </summary>
        [JsonPropertyName("isin")]
        public string Isin { get; set; }

        /// <summary>
        /// Gets or sets the Financial Instrument Global Identifier.
        /// </summary>
        [JsonPropertyName("figi")]
        public string Figi { get; set; }

        /// <summary>
        /// Gets or sets the Legal Entity Identifier.
        /// </summary>
        [JsonPropertyName("lei")]
        public string Lei { get; set; }

        /// <summary>
        /// Gets or sets the CUSIP.
        /// </summary>
        [JsonPropertyName("cusip")]
        public string Cusip { get; set; }

        /// <summary>
        /// Gets or sets the SEC Central Index Key.
        /// </summary>
        [JsonPropertyName("cik")]
        public string Cik { get; set; }
    }
}
