using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace EODHD.CSharpApiClient.JsonSupport
{
    /// <summary>
    /// Reads a nullable <see cref="TimeOnly"/> from an EODHD time-of-day string (<c>"13:00"</c> or <c>"09:30:00"</c>), which
    /// System.Text.Json cannot parse on its own when the seconds are missing. A blank, <c>"NA"</c> or otherwise
    /// unparsable value maps to <c>null</c>. Writes <c>HH:mm:ss</c>.
    /// </summary>
    public sealed class NaTolerantTimeConverter : JsonConverter<TimeOnly?>
    {
        private static readonly string[] Formats = new string[] { "H:mm", "H:mm:ss" };

        /// <inheritdoc/>
        public override TimeOnly? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if(reader.TokenType != JsonTokenType.String)
            {
                reader.Skip();
                return null;
            }

            return TimeOnly.TryParseExact(reader.GetString(), Formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out TimeOnly time)
                ? time
                : null;
        }

        /// <inheritdoc/>
        public override void Write(Utf8JsonWriter writer, TimeOnly? value, JsonSerializerOptions options)
        {
            if(value.HasValue)
            {
                writer.WriteStringValue(value.Value.ToString("HH:mm:ss", CultureInfo.InvariantCulture));
            }
            else
            {
                writer.WriteNullValue();
            }
        }
    }
}
