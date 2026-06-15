using System;
using System.Collections.Generic;
using System.Net;

using EODHD.CSharpApiClient.DataModel.TechnicalIndicators;

using Xunit;

namespace EODHD.CSharpApiClient.UnitTests
{
    public class TechnicalIndicatorTests
    {
        private static EodhdClient CreateClient(string responseJson, out FakeHttpTransport transport)
        {
            transport = new FakeHttpTransport(HttpStatusCode.OK, responseJson);
            return new EodhdClient(new EodhdClientOptions { ApiToken = "test" }, transport);
        }

        [Fact]
        public async System.Threading.Tasks.Task GetTechnicalIndicator_Sma_SingleValueAndUrlParams()
        {
            const string json = "[{\"date\":\"2024-01-02\",\"sma\":187.42},{\"date\":\"2024-01-03\",\"sma\":188.10}]";

            using EodhdClient client = CreateClient(json, out FakeHttpTransport transport);

            TechnicalIndicatorPoint[] points = await client.GetTechnicalIndicatorAsync(
                "AAPL.US", TechnicalFunction.Sma, period: 50, ascending: false);

            Assert.Equal(2, points.Length);
            Assert.Equal(new DateTime(2024, 1, 2), points[0].Date);
            Assert.Equal(187.42, points[0].Values["sma"]);
            Assert.Equal(187.42, points[0].Value);
            string uri = transport.LastRequestUri.ToString();
            Assert.Contains("function=sma", uri, StringComparison.Ordinal);
            Assert.Contains("period=50", uri, StringComparison.Ordinal);
            Assert.Contains("order=d", uri, StringComparison.Ordinal);
        }

        [Fact]
        public async System.Threading.Tasks.Task GetTechnicalIndicator_Macd_MultiValue()
        {
            const string json = "[{\"date\":\"2024-01-02\",\"macd\":1.23,\"signal\":1.10,\"divergence\":0.13}]";

            using EodhdClient client = CreateClient(json, out FakeHttpTransport _);

            TechnicalIndicatorPoint[] points = await client.GetTechnicalIndicatorAsync("AAPL.US", TechnicalFunction.Macd);

            Assert.Single(points);
            Assert.Equal(1.23, points[0].Values["macd"]);
            Assert.Equal(1.10, points[0].Values["signal"]);
            Assert.Equal(0.13, points[0].Values["divergence"]);
            Assert.Null(points[0].Value); // multi-valued → no single Value
        }

        [Fact]
        public async System.Threading.Tasks.Task GetTechnicalIndicator_Stochastic_PassesExtraParameters()
        {
            // The live API returns k_values / d_values for the stochastic function.
            const string json = "[{\"date\":\"2024-01-02\",\"k_values\":80.1,\"d_values\":75.4}]";

            using EodhdClient client = CreateClient(json, out FakeHttpTransport transport);

            Dictionary<string, string> extra = new Dictionary<string, string>
            {
                { "fast_kperiod", "14" },
                { "slow_dperiod", "3" },
            };

            TechnicalIndicatorPoint[] points = await client.GetTechnicalIndicatorAsync(
                "AAPL.US", TechnicalFunction.Stochastic, extraParameters: extra);

            Assert.Equal(80.1, points[0].Values["k_values"]);
            Assert.Equal(75.4, points[0].Values["d_values"]);
            string uri = transport.LastRequestUri.ToString();
            Assert.Contains("function=stochastic", uri, StringComparison.Ordinal);
            Assert.Contains("fast_kperiod=14", uri, StringComparison.Ordinal);
            Assert.Contains("slow_dperiod=3", uri, StringComparison.Ordinal);
        }

        [Fact]
        public async System.Threading.Tasks.Task GetTechnicalIndicator_SplitAdjusted_OhlcValues()
        {
            const string json = "[{\"date\":\"2024-01-02\",\"open\":1.0,\"high\":2.0,\"low\":0.5,\"close\":1.5}]";

            using EodhdClient client = CreateClient(json, out FakeHttpTransport transport);

            TechnicalIndicatorPoint[] points = await client.GetTechnicalIndicatorAsync("AAPL.US", TechnicalFunction.SplitAdjusted);

            Assert.Equal(4, points[0].Values.Count);
            Assert.Equal(2.0, points[0].Values["high"]);
            Assert.Contains("function=splitadjusted", transport.LastRequestUri.ToString(), StringComparison.Ordinal);
        }

        [Fact]
        public async System.Threading.Tasks.Task GetTechnicalIndicator_NaValue_MapsToNull()
        {
            const string json = "[{\"date\":\"2024-01-02\",\"rsi\":\"NA\"}]";

            using EodhdClient client = CreateClient(json, out FakeHttpTransport _);

            TechnicalIndicatorPoint[] points = await client.GetTechnicalIndicatorAsync("AAPL.US", TechnicalFunction.Rsi);

            Assert.Null(points[0].Values["rsi"]);
        }
    }
}
