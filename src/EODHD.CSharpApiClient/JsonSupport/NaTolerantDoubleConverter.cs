using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace EODHD.CSharpApiClient.JsonSupport
{
    /// <summary>
    /// Reads a nullable <see cref="double"/> that EODHD may encode as a number, a numeric string, or the
    /// literal <c>"NA"</c> (and the empty string) for an unavailable value — which map to <c>null</c>.
    /// Used on the live/real-time price fields, where closed-market values come back as <c>"NA"</c>.
    /// </summary>
    public sealed class NaTolerantDoubleConverter : JsonConverter<double?>
    {
        /// <inheritdoc/>
        public override double? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            switch(reader.TokenType)
            {
                case JsonTokenType.Null:
                    return null;
                case JsonTokenType.Number:
                    return reader.GetDouble();
                case JsonTokenType.String:
                    string text = reader.GetString();
                    if(string.IsNullOrWhiteSpace(text) || string.Equals(text, "NA", StringComparison.OrdinalIgnoreCase))
                    {
                        return null;
                    }

                    return double.TryParse(text, NumberStyles.Any, CultureInfo.InvariantCulture, out double value) ? value : (double?)null;
                default:
                    return null;
            }
        }

        /// <inheritdoc/>
        public override void Write(Utf8JsonWriter writer, double? value, JsonSerializerOptions options)
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
