using System;
using System.Buffers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace EODHD.CSharpApiClient.JsonSupport
{
    /// <summary>
    /// Reads a <see cref="string"/> that EODHD may send as a JSON string, number, or boolean. EODHD switches
    /// a field between quoted and bare values from one symbol to the next, and System.Text.Json otherwise
    /// throws on a bare number for a <see cref="string"/> property, failing the whole response.
    /// A number keeps its exact wire text (e.g. <c>14459.0312</c> reads as <c>"14459.0312"</c>).
    /// </summary>
    public sealed class NumberTolerantStringConverter : JsonConverter<string>
    {
        /// <inheritdoc/>
        public override string Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            switch(reader.TokenType)
            {
                case JsonTokenType.String:
                    return reader.GetString();
                case JsonTokenType.Number:
                    return reader.HasValueSequence
                        ? Encoding.UTF8.GetString(reader.ValueSequence.ToArray())
                        : Encoding.UTF8.GetString(reader.ValueSpan);
                case JsonTokenType.True:
                    return "true";
                case JsonTokenType.False:
                    return "false";
                default:
                    // No message: System.Text.Json then reports the failing JSON path and position.
                    throw new JsonException();
            }
        }

        /// <inheritdoc/>
        public override void Write(Utf8JsonWriter writer, string value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value);
        }
    }
}
