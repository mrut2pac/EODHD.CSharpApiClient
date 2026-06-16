using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace EODHD.CSharpApiClient.DataModel.Quotes
{
    // Internal envelope for the US delayed-quote endpoint: "data" is an object keyed by symbol.
    internal sealed class UsQuoteResponse
    {
        [JsonPropertyName("data")]
        public Dictionary<string, UsDelayedQuote> Data { get; set; }
    }
}
