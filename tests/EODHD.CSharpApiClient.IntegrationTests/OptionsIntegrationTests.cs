using System.Threading.Tasks;

using EODHD.CSharpApiClient.DataModel.Options;
using EODHD.CSharpApiClient.Exceptions;

using Xunit;

namespace EODHD.CSharpApiClient.IntegrationTests
{
    public class OptionsIntegrationTests : IntegrationTestBase
    {
        [SkippableFact]
        public async Task GetOptionsChain_Legacy_Apple_ReturnsChain()
        {
            this.SkipIfNoApiKey();
            using EodhdClient client = this.CreateClient();

            OptionsChain chain;
            try
            {
                chain = await client.GetOptionsChainAsync("AAPL.US");
            }
            catch(EodhdHttpException ex)
            {
                SkipIfNoLicense(ex);
                throw;
            }

            Assert.NotNull(chain);
            Skip.If(chain.Data == null || chain.Data.Length == 0, "No option expirations returned.");
            Assert.Equal("AAPL", chain.Code);
            Assert.NotNull(chain.Data[0].Options);
        }

        // The marketplace options endpoints require a separate add-on; the main key may return 403.
        // EODHD's public demo token serves AAPL for these endpoints, so we verify them live via demo —
        // these tests are NOT gated on EODHD_API_KEY because they do not use it.
        [SkippableFact]
        public async Task GetOptionsEod_Marketplace_Apple_ReturnsRecords()
        {
            using EodhdClient client = CreateDemoClient();

            OptionData[] records;
            try
            {
                records = await client.GetOptionsEodAsync("AAPL", limit: 5, sort: "-exp_date");
            }
            catch(EodhdHttpException ex)
            {
                SkipIfNoLicense(ex);
                throw;
            }

            Assert.NotNull(records);
            Skip.If(records.Length == 0, "No marketplace EOD option records returned.");
            Assert.Equal("AAPL", records[0].UnderlyingSymbol);
            Assert.False(string.IsNullOrEmpty(records[0].Contract));
        }

        // Not gated on EODHD_API_KEY: verified live via EODHD's public demo token (see note above).
        [SkippableFact]
        public async Task GetOptionContracts_Marketplace_Apple_FilteredByType_ReturnsCalls()
        {
            using EodhdClient client = CreateDemoClient();

            OptionData[] records;
            try
            {
                records = await client.GetOptionContractsAsync("AAPL", optionType: "call", limit: 5);
            }
            catch(EodhdHttpException ex)
            {
                SkipIfNoLicense(ex);
                throw;
            }

            Assert.NotNull(records);
            Skip.If(records.Length == 0, "No marketplace option contracts returned.");
            Assert.All(records, r => Assert.Equal("call", r.Type));
        }
    }
}
