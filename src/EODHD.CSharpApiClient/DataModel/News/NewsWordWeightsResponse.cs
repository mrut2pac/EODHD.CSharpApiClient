using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace EODHD.CSharpApiClient.DataModel.News
{
    // Internal envelope for the news-word-weights endpoint, whose "data" is an object mapping each
    // keyword to its weight. The public API surfaces the dictionary directly.
    internal sealed class NewsWordWeightsResponse
    {
        [JsonPropertyName("data")]
        public Dictionary<string, double> Data { get; set; }
    }
}
