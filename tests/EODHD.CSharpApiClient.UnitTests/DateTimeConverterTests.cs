using System;
using System.Text.Json;

using EODHD.CSharpApiClient.DataModel.ExchangeInfo;
using EODHD.CSharpApiClient.DataModel.Fundamental;
using EODHD.CSharpApiClient.JsonSupport;

using Xunit;

namespace EODHD.CSharpApiClient.UnitTests
{
    public class DateTimeConverterTests
    {
        private static readonly JsonSerializerOptions DateOptions = new JsonSerializerOptions { Converters = { new NaTolerantDateConverter() } };
        private static readonly JsonSerializerOptions TimeOptions = new JsonSerializerOptions { Converters = { new NaTolerantTimeConverter() } };

        [Theory]
        [InlineData("\"2026-06-30\"", 2026, 6, 30, 0, 0, 0)]
        [InlineData("\"2026-10-19 17:00:00\"", 2026, 10, 19, 17, 0, 0)]
        public void DateConverter_Read_ParsesDateAndDateTime(string json, int year, int month, int day, int hour, int minute, int second)
        {
            DateTime? value = JsonSerializer.Deserialize<DateTime?>(json, DateOptions);

            Assert.Equal(new DateTime(year, month, day, hour, minute, second), value);
        }

        [Theory]
        [InlineData("\"0000-00-00\"")]
        [InlineData("\"NA\"")]
        [InlineData("\"\"")]
        [InlineData("null")]
        [InlineData("20260630")]
        [InlineData("{\"a\":1}")]
        public void DateConverter_Read_PlaceholdersAndNonStrings_MapToNull(string json)
        {
            Assert.Null(JsonSerializer.Deserialize<DateTime?>(json, DateOptions));
        }

        [Fact]
        public void DateConverter_Write_DateOnlyOrWithTimeOfDay()
        {
            Assert.Equal("\"2026-06-30\"", JsonSerializer.Serialize<DateTime?>(new DateTime(2026, 6, 30), DateOptions));
            Assert.Equal("\"2026-10-19 17:00:00\"", JsonSerializer.Serialize<DateTime?>(new DateTime(2026, 10, 19, 17, 0, 0), DateOptions));
            Assert.Equal("null", JsonSerializer.Serialize<DateTime?>(null, DateOptions));
        }

        [Theory]
        [InlineData("\"13:00\"", 13, 0)]
        [InlineData("\"9:30\"", 9, 30)]
        [InlineData("\"09:30:00\"", 9, 30)]
        public void TimeConverter_Read_ParsesWithOrWithoutSeconds(string json, int hour, int minute)
        {
            Assert.Equal(new TimeOnly(hour, minute), JsonSerializer.Deserialize<TimeOnly?>(json, TimeOptions));
        }

        [Theory]
        [InlineData("\"NA\"")]
        [InlineData("\"\"")]
        [InlineData("\"25:00\"")]
        [InlineData("null")]
        [InlineData("1300")]
        public void TimeConverter_Read_InvalidOrNonStrings_MapToNull(string json)
        {
            Assert.Null(JsonSerializer.Deserialize<TimeOnly?>(json, TimeOptions));
        }

        [Fact]
        public void TimeConverter_Write_UsesHoursMinutesSeconds()
        {
            Assert.Equal("\"13:00:00\"", JsonSerializer.Serialize<TimeOnly?>(new TimeOnly(13, 0), TimeOptions));
        }

        [Fact]
        public void SettableDateAndTimeProperties_RoundTripThroughJson()
        {
            // The typed properties are settable (e.g. for test fakes or caches) and survive a serialize/deserialize cycle.
            General general = new General
            {
                IPODate = new DateTime(1980, 12, 12),
                DelistedDate = null,
            };
            ExchangeEarlyCloseDay day = new ExchangeEarlyCloseDay
            {
                Date = new DateTime(2026, 12, 24),
                EarlyClose = new TimeOnly(13, 0),
            };

            string generalJson = JsonSerializer.Serialize(general);
            string dayJson = JsonSerializer.Serialize(day);
            General generalBack = JsonSerializer.Deserialize<General>(generalJson);
            ExchangeEarlyCloseDay dayBack = JsonSerializer.Deserialize<ExchangeEarlyCloseDay>(dayJson);

            Assert.Contains("\"IPODate\":\"1980-12-12\"", generalJson, StringComparison.Ordinal);
            Assert.Equal(new DateTime(1980, 12, 12), generalBack.IPODate);
            Assert.Null(generalBack.DelistedDate);
            Assert.Contains("\"EarlyClose\":\"13:00:00\"", dayJson, StringComparison.Ordinal);
            Assert.Equal(new TimeOnly(13, 0), dayBack.EarlyClose);
        }
    }
}
