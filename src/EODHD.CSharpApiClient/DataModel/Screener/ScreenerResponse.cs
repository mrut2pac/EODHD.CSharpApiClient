using System.Text.Json.Serialization;

namespace EODHD.CSharpApiClient.DataModel.Screener
{
    // Internal envelope: the screener endpoint wraps its results in a "data" array. The public API
    // surfaces the inner ScreenerResult[] directly.
    internal sealed class ScreenerResponse
    {
        [JsonPropertyName("data")]
        public ScreenerResult[] Data { get; set; }
    }
}
