using System.Text.Json.Serialization;

namespace EODHD.CSharpApiClient.DataModel
{
    // Internal envelope for endpoints that wrap their payload in a JSON object with a "data" array
    // (and a "meta" block the public API does not surface). The public methods return the Data array.
    internal sealed class DataEnvelope<T>
    {
        [JsonPropertyName("data")]
        public T[] Data { get; set; }
    }
}
