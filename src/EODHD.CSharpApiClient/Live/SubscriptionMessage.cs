using System.Text.Json.Serialization;

namespace EODHD.CSharpApiClient.Live
{
    // Outgoing control message for (un)subscribing to symbols, e.g.
    // {"action":"subscribe","symbols":"EURUSD,GBPUSD"}.
    internal sealed class SubscriptionMessage
    {
        [JsonPropertyName("action")]
        public string Action { get; set; }

        [JsonPropertyName("symbols")]
        public string Symbols { get; set; }
    }
}
