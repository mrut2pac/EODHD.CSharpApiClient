using System.Text.Json.Serialization;

namespace EODHD.CSharpApiClient.DataModel.Cboe
{
    // Internal JSON:API envelope for the Cboe index endpoints. Each resource wraps the index fields
    // under "attributes" and, for the single-index endpoint, the constituents under "components"
    // (themselves "attributes"-wrapped). The public API surfaces the flattened CboeIndex.
    internal sealed class CboeIndexResponse
    {
        [JsonPropertyName("data")]
        public CboeIndexResource[] Data { get; set; }
    }

    internal sealed class CboeIndexResource
    {
        [JsonPropertyName("attributes")]
        public CboeIndex Attributes { get; set; }

        [JsonPropertyName("components")]
        public CboeIndexComponentResource[] Components { get; set; }
    }

    internal sealed class CboeIndexComponentResource
    {
        [JsonPropertyName("attributes")]
        public CboeIndexComponent Attributes { get; set; }
    }
}
