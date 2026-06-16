using System;

namespace EODHD.CSharpApiClient.DataModel
{
    // Converts EODHD's Unix-epoch fields (which appear in both seconds and milliseconds depending on the
    // endpoint) into offset-aware instants. The result always carries a +00:00 offset (UTC).
    internal static class UnixTime
    {
        public static DateTimeOffset? FromMilliseconds(long? milliseconds)
        {
            return milliseconds.HasValue ? DateTimeOffset.FromUnixTimeMilliseconds(milliseconds.Value) : (DateTimeOffset?)null;
        }

        public static DateTimeOffset? FromSeconds(long? seconds)
        {
            return seconds.HasValue ? DateTimeOffset.FromUnixTimeSeconds(seconds.Value) : (DateTimeOffset?)null;
        }
    }
}
