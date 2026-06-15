using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace EODHD.CSharpApiClient.JsonSupport
{
    /// <summary>
    /// Reads a nullable <see cref="DateTime"/> from the space-separated format EODHD uses for intraday
    /// timestamps (<c>"yyyy-MM-dd HH:mm:ss"</c>), which the default System.Text.Json parser rejects
    /// because it is not ISO-8601. Also accepts an ISO-8601 string, a date-only string, a Unix-seconds
    /// number, and <c>null</c>/<c>"NA"</c> (mapped to <c>null</c>).
    /// </summary>
    public sealed class SpaceSeparatedDateTimeConverter : JsonConverter<DateTime?>
    {
        private static readonly string[] AcceptedFormats = new string[]
        {
            "yyyy-MM-dd HH:mm:ss",
            "yyyy-MM-ddTHH:mm:ss",
            "yyyy-MM-dd",
        };

        /// <inheritdoc/>
        public override DateTime? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if(reader.TokenType == JsonTokenType.Null)
            {
                return null;
            }

            if(reader.TokenType == JsonTokenType.Number)
            {
                return DateTimeOffset.FromUnixTimeSeconds(reader.GetInt64()).UtcDateTime;
            }

            if(reader.TokenType == JsonTokenType.String)
            {
                string text = reader.GetString();
                if(string.IsNullOrWhiteSpace(text) || string.Equals(text, "NA", StringComparison.OrdinalIgnoreCase))
                {
                    return null;
                }

                if(DateTime.TryParseExact(text, AcceptedFormats, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime exact))
                {
                    return exact;
                }

                if(DateTime.TryParse(text, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsed))
                {
                    return parsed;
                }
            }

            return null;
        }

        /// <inheritdoc/>
        public override void Write(Utf8JsonWriter writer, DateTime? value, JsonSerializerOptions options)
        {
            if(value.HasValue)
            {
                writer.WriteStringValue(value.Value.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture));
            }
            else
            {
                writer.WriteNullValue();
            }
        }
    }
}
