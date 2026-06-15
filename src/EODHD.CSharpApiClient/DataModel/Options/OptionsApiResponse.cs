using System.Text.Json.Serialization;

namespace EODHD.CSharpApiClient.DataModel.Options
{
    // Internal JSON:API envelope for the marketplace options endpoints. Each resource wraps the option
    // record under "attributes"; the public API surfaces the OptionData[] directly.
    internal sealed class OptionsApiResponse
    {
        [JsonPropertyName("data")]
        public OptionsApiResource[] Data { get; set; }
    }

    internal sealed class OptionsApiResource
    {
        [JsonPropertyName("attributes")]
        public OptionData Attributes { get; set; }
    }
}
