using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

using EODHD.CSharpApiClient.DataModel.Cboe;
using EODHD.CSharpApiClient.Exceptions;

using Xunit;

namespace EODHD.CSharpApiClient.IntegrationTests
{
    public class CboeNewsWordWeightsIntegrationTests : IntegrationTestBase
    {
        [SkippableFact]
        public async Task GetCboeIndices_ReturnsSnapshots()
        {
            this.SkipIfNoApiKey();
            using EodhdClient client = this.CreateClient();

            CboeIndex[] indices;
            try
            {
                indices = await client.GetCboeIndicesAsync(limit: 5);
            }
            catch (EodhdHttpException ex)
            {
                SkipIfNoLicense(ex);
                throw;
            }

            Assert.NotNull(indices);
            Skip.If(indices.Length == 0, "No Cboe indices returned.");
            Assert.All(indices, i => Assert.False(string.IsNullOrEmpty(i.IndexCode)));
        }

        [SkippableFact]
        public async Task GetCboeIndex_ReturnsComponents()
        {
            this.SkipIfNoApiKey();
            using EodhdClient client = this.CreateClient();

            CboeIndex[] indices;
            try
            {
                indices = await client.GetCboeIndicesAsync(limit: 1);
            }
            catch (EodhdHttpException ex)
            {
                SkipIfNoLicense(ex);
                throw;
            }

            Skip.If(indices.Length == 0, "No Cboe indices to resolve a detail request from.");
            CboeIndex listed = indices[0];
            DateTime date = DateTime.ParseExact(listed.Date, "yyyy-MM-dd", CultureInfo.InvariantCulture);

            CboeIndex index = await client.GetCboeIndexAsync(listed.IndexCode, listed.FeedType, date);

            Assert.NotNull(index);
            Assert.Equal(listed.IndexCode, index.IndexCode);
            Assert.NotNull(index.Components);
            Skip.If(index.Components.Length == 0, "Index returned no components.");
            Assert.All(index.Components, c => Assert.False(string.IsNullOrEmpty(c.Symbol)));
        }

        [SkippableFact]
        public async Task GetNewsWordWeights_Apple_ReturnsWeights()
        {
            this.SkipIfNoApiKey();
            using EodhdClient client = this.CreateClient();

            Dictionary<string, double> weights;
            try
            {
                weights = await client.GetNewsWordWeightsAsync("AAPL.US");
            }
            catch (EodhdHttpException ex)
            {
                SkipIfNoLicense(ex);
                throw;
            }

            Assert.NotNull(weights);
            Skip.If(weights.Count == 0, "No news word weights returned.");
            Assert.All(weights.Values, w => Assert.True(w >= 0));
        }
    }
}
