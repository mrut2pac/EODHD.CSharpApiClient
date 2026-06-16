using System.Collections.Generic;
using System.Threading.Tasks;

using EODHD.CSharpApiClient.DataModel.TechnicalIndicators;
using EODHD.CSharpApiClient.Exceptions;

using Xunit;

namespace EODHD.CSharpApiClient.IntegrationTests
{
    public class TechnicalIndicatorIntegrationTests : IntegrationTestBase
    {
        private async Task<TechnicalIndicatorPoint[]> FetchAsync(TechnicalFunction function, int? period, IReadOnlyDictionary<string, string> extra = null)
        {
            using EodhdClient client = this.CreateClient();
            try
            {
                return await client.GetTechnicalIndicatorAsync("AAPL.US", function, period: period, extraParameters: extra);
            }
            catch(EodhdHttpException ex)
            {
                SkipIfNoLicense(ex);
                throw;
            }
        }

        [SkippableFact]
        public async Task GetTechnicalIndicator_Sma_ReturnsSingleValues()
        {
            this.SkipIfNoApiKey();
            TechnicalIndicatorPoint[] points = await this.FetchAsync(TechnicalFunction.Sma, 50);
            Assert.NotNull(points);
            Skip.If(points.Length == 0, "No SMA points returned.");
            Assert.True(points[0].Value.HasValue);
        }

        [SkippableFact]
        public async Task GetTechnicalIndicator_Rsi_ReturnsSingleValues()
        {
            this.SkipIfNoApiKey();
            TechnicalIndicatorPoint[] points = await this.FetchAsync(TechnicalFunction.Rsi, 14);
            Assert.NotNull(points);
            Skip.If(points.Length == 0, "No RSI points returned.");
            Assert.Contains("rsi", points[0].Values.Keys);
        }

        [SkippableFact]
        public async Task GetTechnicalIndicator_Macd_ReturnsMultiValues()
        {
            this.SkipIfNoApiKey();
            TechnicalIndicatorPoint[] points = await this.FetchAsync(TechnicalFunction.Macd, null);
            Assert.NotNull(points);
            Skip.If(points.Length == 0, "No MACD points returned.");
            Assert.Contains("macd", points[0].Values.Keys);
            Assert.Contains("signal", points[0].Values.Keys);
            Assert.Contains("divergence", points[0].Values.Keys);
        }

        [SkippableFact]
        public async Task GetTechnicalIndicator_BollingerBands_ReturnsBands()
        {
            this.SkipIfNoApiKey();
            TechnicalIndicatorPoint[] points = await this.FetchAsync(TechnicalFunction.BollingerBands, 20);
            Assert.NotNull(points);
            Skip.If(points.Length == 0, "No Bollinger Bands points returned.");
            Assert.Contains("uband", points[0].Values.Keys);
            Assert.Contains("mband", points[0].Values.Keys);
            Assert.Contains("lband", points[0].Values.Keys);
        }

        [SkippableFact]
        public async Task GetTechnicalIndicator_Stochastic_ReturnsKandD()
        {
            this.SkipIfNoApiKey();
            TechnicalIndicatorPoint[] points = await this.FetchAsync(TechnicalFunction.Stochastic, null);
            Assert.NotNull(points);
            Skip.If(points.Length == 0, "No stochastic points returned.");
            // The live API returns k_values / d_values (not k / d as some docs suggest).
            Assert.Contains("k_values", points[0].Values.Keys);
            Assert.Contains("d_values", points[0].Values.Keys);
        }

        [SkippableFact]
        public async Task GetTechnicalIndicator_SplitAdjusted_ReturnsOhlc()
        {
            this.SkipIfNoApiKey();
            TechnicalIndicatorPoint[] points = await this.FetchAsync(TechnicalFunction.SplitAdjusted, null);
            Assert.NotNull(points);
            Skip.If(points.Length == 0, "No split-adjusted points returned.");
            Assert.Contains("close", points[0].Values.Keys);
        }
    }
}
