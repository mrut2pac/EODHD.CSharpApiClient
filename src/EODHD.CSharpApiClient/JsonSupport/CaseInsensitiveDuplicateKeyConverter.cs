using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace EODHD.CSharpApiClient.JsonSupport
{
    /// <summary>
    /// Reads an object whose payload repeats a key in another casing, keeping the first non-null value.
    /// EODHD's <c>MutualFund_Data</c> sends e.g. <c>Market_Capitalization</c> (the data) followed by
    /// <c>market_capitalization</c> (<c>null</c>). Property names are matched case-insensitively, so without
    /// this the trailing <c>null</c> would overwrite the data.
    /// </summary>
    /// <typeparam name="T">The model type to deserialize.</typeparam>
    public sealed class CaseInsensitiveDuplicateKeyConverter<T> : JsonConverter<T>
        where T : class
    {
        /// <inheritdoc/>
        public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            JsonNode node = JsonNode.Parse(ref reader);
            if(node is not JsonObject source)
            {
                return node.Deserialize<T>(options);
            }

            JsonObject merged = new JsonObject();
            Dictionary<string, string> keptKeys = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            foreach(KeyValuePair<string, JsonNode> property in source)
            {
                JsonNode value = property.Value?.DeepClone();
                if(!keptKeys.TryGetValue(property.Key, out string keptKey))
                {
                    keptKeys[property.Key] = property.Key;
                    merged[property.Key] = value;
                }
                else if(merged[keptKey] is null && value is not null)
                {
                    merged[keptKey] = value;
                }
            }

            return merged.Deserialize<T>(options);
        }

        /// <inheritdoc/>
        public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, value, options);
        }
    }
}
