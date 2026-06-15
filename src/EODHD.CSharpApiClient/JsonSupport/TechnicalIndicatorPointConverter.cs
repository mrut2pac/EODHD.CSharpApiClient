using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

using EODHD.CSharpApiClient.DataModel.TechnicalIndicators;

namespace EODHD.CSharpApiClient.JsonSupport
{
    /// <summary>
    /// Deserializes a technical-indicator row into a <see cref="TechnicalIndicatorPoint"/>: the
    /// <c>date</c> field becomes <see cref="TechnicalIndicatorPoint.Date"/> and every other field is
    /// collected into <see cref="TechnicalIndicatorPoint.Values"/>. This keeps a single model working
    /// across all indicator functions, whose output field names differ by function.
    /// </summary>
    public sealed class TechnicalIndicatorPointConverter : JsonConverter<TechnicalIndicatorPoint>
    {
        private static readonly string[] DateFormats = new string[]
        {
            "yyyy-MM-dd",
            "yyyy-MM-dd HH:mm:ss",
            "yyyy-MM-ddTHH:mm:ss",
        };

        /// <inheritdoc/>
        public override TechnicalIndicatorPoint Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if(reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException("Expected the start of a technical-indicator object.");
            }

            TechnicalIndicatorPoint point = new TechnicalIndicatorPoint();
            Dictionary<string, double?> values = new Dictionary<string, double?>(StringComparer.Ordinal);

            while(reader.Read())
            {
                if(reader.TokenType == JsonTokenType.EndObject)
                {
                    break;
                }

                string name = reader.GetString();
                reader.Read();

                if(string.Equals(name, "date", StringComparison.OrdinalIgnoreCase))
                {
                    point.Date = ReadDate(ref reader);
                }
                else
                {
                    values[name] = ReadNullableDouble(ref reader);
                }
            }

            point.Values = values;
            return point;
        }

        /// <inheritdoc/>
        public override void Write(Utf8JsonWriter writer, TechnicalIndicatorPoint value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();

            if(value.Date.HasValue)
            {
                writer.WriteString("date", value.Date.Value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture));
            }

            if(value.Values != null)
            {
                foreach(KeyValuePair<string, double?> entry in value.Values)
                {
                    if(entry.Value.HasValue)
                    {
                        writer.WriteNumber(entry.Key, entry.Value.Value);
                    }
                    else
                    {
                        writer.WriteNull(entry.Key);
                    }
                }
            }

            writer.WriteEndObject();
        }

        private static DateTime? ReadDate(ref Utf8JsonReader reader)
        {
            if(reader.TokenType != JsonTokenType.String)
            {
                return null;
            }

            string text = reader.GetString();
            if(string.IsNullOrWhiteSpace(text))
            {
                return null;
            }

            if(DateTime.TryParseExact(text, DateFormats, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime exact))
            {
                return exact;
            }

            return DateTime.TryParse(text, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsed) ? parsed : (DateTime?)null;
        }

        private static double? ReadNullableDouble(ref Utf8JsonReader reader)
        {
            switch(reader.TokenType)
            {
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
    }
}
