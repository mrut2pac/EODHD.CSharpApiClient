using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace EODHD.CSharpApiClient.JsonSupport
{
    /// <summary>
    /// Reads a nullable <see cref="DateTime"/> from an EODHD date string (<c>"2026-06-30"</c> or
    /// <c>"2026-10-19 17:00:00"</c>). The placeholders EODHD sends for a missing date (<c>"0000-00-00"</c>, <c>"NA"</c>, the
    /// empty string) and any other unparsable value map to <c>null</c>. Writes <c>yyyy-MM-dd</c>, with
    /// <c> HH:mm:ss</c> appended when the value has a time of day; sub-second precision and
    /// <see cref="DateTime.Kind"/> are not written, as no EODHD date carries them.
    /// </summary>
    public sealed class NaTolerantDateConverter : JsonConverter<DateTime?>
    {
        /// <inheritdoc/>
        public override DateTime? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if(reader.TokenType != JsonTokenType.String)
            {
                reader.Skip();
                return null;
            }

            return DateTime.TryParse(reader.GetString(), CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date)
                ? date
                : null;
        }

        /// <inheritdoc/>
        public override void Write(Utf8JsonWriter writer, DateTime? value, JsonSerializerOptions options)
        {
            if(!value.HasValue)
            {
                writer.WriteNullValue();
                return;
            }

            string format = value.Value.TimeOfDay == TimeSpan.Zero ? "yyyy-MM-dd" : "yyyy-MM-dd HH:mm:ss";
            writer.WriteStringValue(value.Value.ToString(format, CultureInfo.InvariantCulture));
        }
    }
}
