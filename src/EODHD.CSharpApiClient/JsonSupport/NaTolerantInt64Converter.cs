using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace EODHD.CSharpApiClient.JsonSupport
{
    /// <summary>
    /// Reads a nullable <see cref="long"/> that EODHD may encode as a number, a numeric string, or the
    /// literal <c>"NA"</c> (and the empty string) for an unavailable value — which map to <c>null</c>.
    /// Used on the live/real-time quote timestamp, which comes back as <c>"NA"</c> for a delisted symbol.
    /// </summary>
    public sealed class NaTolerantInt64Converter : JsonConverter<long?>
    {
        /// <inheritdoc/>
        public override long? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            switch(reader.TokenType)
            {
                case JsonTokenType.Null:
                    return null;
                case JsonTokenType.Number:
                    return reader.GetInt64();
                case JsonTokenType.String:
                    string text = reader.GetString();
                    if(string.IsNullOrWhiteSpace(text) || string.Equals(text, "NA", StringComparison.OrdinalIgnoreCase))
                    {
                        return null;
                    }

                    return long.TryParse(text, NumberStyles.Integer, CultureInfo.InvariantCulture, out long value) ? value : (long?)null;
                default:
                    return null;
            }
        }

        /// <inheritdoc/>
        public override void Write(Utf8JsonWriter writer, long? value, JsonSerializerOptions options)
        {
            if(value.HasValue)
            {
                writer.WriteNumberValue(value.Value);
            }
            else
            {
                writer.WriteNullValue();
            }
        }
    }
}
