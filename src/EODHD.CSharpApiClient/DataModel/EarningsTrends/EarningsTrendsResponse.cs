using System.Text.Json.Serialization;

namespace EODHD.CSharpApiClient.DataModel.EarningsTrends
{
    // Internal envelope: the trends endpoint nests one inner array of records per requested symbol.
    // The public API flattens these into a single EarningsTrend[] (each record carries its own code).
    internal sealed class EarningsTrendsResponse
    {
        [JsonPropertyName("trends")]
        public EarningsTrend[][] Trends { get; set; }
    }
}
