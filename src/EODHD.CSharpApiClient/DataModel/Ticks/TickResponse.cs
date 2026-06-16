using System.Text.Json.Serialization;

namespace EODHD.CSharpApiClient.DataModel.Ticks
{
    // Internal columnar (struct-of-arrays) envelope for the ticks endpoint. Each property is a parallel
    // column; the public API transposes them into a Tick[]. Columns may be absent (e.g. "ex"), so the
    // transpose tolerates differing / missing lengths.
    internal sealed class TickResponse
    {
        [JsonPropertyName("ex")]
        public string[] Ex { get; set; }

        [JsonPropertyName("mkt")]
        public string[] Mkt { get; set; }

        [JsonPropertyName("sub_mkt")]
        public string[] SubMkt { get; set; }

        [JsonPropertyName("price")]
        public double[] Price { get; set; }

        [JsonPropertyName("seq")]
        public long[] Seq { get; set; }

        [JsonPropertyName("shares")]
        public long[] Shares { get; set; }

        [JsonPropertyName("sl")]
        public string[] Sl { get; set; }

        [JsonPropertyName("ts")]
        public long[] Ts { get; set; }
    }
}
