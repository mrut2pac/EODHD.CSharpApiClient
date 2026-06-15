using System;
using System.Collections.Generic;
using System.Net;

using EODHD.CSharpApiClient.DataModel;

using Xunit;

namespace EODHD.CSharpApiClient.UnitTests
{
    public class SerializationTests
    {
        private static EodhdClient CreateClient(string responseJson)
        {
            FakeHttpTransport transport = new FakeHttpTransport(HttpStatusCode.OK, responseJson);
            return new EodhdClient(new EodhdClientOptions { ApiToken = "test" }, transport);
        }

        [Fact]
        public async System.Threading.Tasks.Task GetEndOfDay_StringVolumeAndAdjustedClose_DeserializesAndComputesAdjustments()
        {
            // volume is wire-encoded as a JSON string — exercises the global AllowReadingFromString.
            const string json =
                "[{\"date\":\"2024-01-02\",\"open\":187.0,\"high\":188.44,\"low\":183.89," +
                "\"close\":185.64,\"adjusted_close\":184.20,\"volume\":\"82488700\"}]";

            using EodhdClient client = CreateClient(json);

            List<HistoricalStockPrice> bars = await client.GetEndOfDayHistoricalStockPricesAsync(
                "AAPL.US", new DateTime(2024, 1, 1), new DateTime(2024, 1, 3), EndOfDayPeriod.Daily);

            Assert.Single(bars);
            HistoricalStockPrice bar = bars[0];
            Assert.Equal(new DateTime(2024, 1, 2), bar.Date);
            Assert.Equal(187.0, bar.Open);
            Assert.Equal(184.20, bar.AdjustedClose);
            Assert.Equal(82488700L, bar.Volume);

            // AdjustedLow = round(adjusted_close/close * low, 4)
            double expectedAdjustedLow = Math.Round(184.20 / 185.64 * 183.89, 4);
            Assert.Equal(expectedAdjustedLow, bar.AdjustedLow);
        }

        [Fact]
        public async System.Threading.Tasks.Task GetHistoricalSplits_RatioString_ComputesFactor()
        {
            const string json = "[{\"date\":\"2020-08-31\",\"split\":\"4.000000/1.000000\"}]";

            using EodhdClient client = CreateClient(json);

            HistoricalSplit[] splits = await client.GetHistoricalSplitsAsync("AAPL.US");

            Assert.Single(splits);
            Assert.Equal(new DateTime(2020, 8, 31), splits[0].Date);
            // ParseSplitFactor("4/1") = oldShares / newShares = 1 / 4.
            Assert.Equal(0.25, splits[0].Factor);
        }
    }
}
