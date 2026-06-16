using System.Threading.Tasks;

using EODHD.CSharpApiClient.DataModel.Treasury;
using EODHD.CSharpApiClient.DataModel.UpcomingDividends;
using EODHD.CSharpApiClient.Exceptions;

using Xunit;

namespace EODHD.CSharpApiClient.IntegrationTests
{
    public class DividendsTreasuryIntegrationTests : IntegrationTestBase
    {
        [SkippableFact]
        public async Task GetUpcomingDividends_Apple_ReturnsDates()
        {
            this.SkipIfNoApiKey();
            using EodhdClient client = this.CreateClient();

            UpcomingDividend[] dividends;
            try
            {
                dividends = await client.GetUpcomingDividendsAsync("AAPL.US");
            }
            catch(EodhdHttpException ex)
            {
                SkipIfNoLicense(ex);
                throw;
            }

            Assert.NotNull(dividends);
            Skip.If(dividends.Length == 0, "No upcoming dividends returned for the symbol.");
            Assert.All(dividends, d => Assert.Equal("AAPL.US", d.Symbol));
            Assert.Contains(dividends, d => d.Date.HasValue);
        }

        [SkippableFact]
        public async Task GetTreasuryYieldRates_ByYear_ReturnsTenorRates()
        {
            this.SkipIfNoApiKey();
            using EodhdClient client = this.CreateClient();

            TreasuryRate[] rates;
            try
            {
                rates = await client.GetTreasuryYieldRatesAsync(year: 2024);
            }
            catch(EodhdHttpException ex)
            {
                SkipIfNoLicense(ex);
                throw;
            }

            Assert.NotNull(rates);
            Skip.If(rates.Length == 0, "No Treasury yield rates returned.");
            Assert.Contains(rates, r => r.Tenor == "10Y" && r.Rate.HasValue);
        }

        [SkippableFact]
        public async Task GetTreasuryRealYieldRates_ReturnsRates()
        {
            this.SkipIfNoApiKey();
            using EodhdClient client = this.CreateClient();

            TreasuryRate[] rates;
            try
            {
                rates = await client.GetTreasuryRealYieldRatesAsync(year: 2024);
            }
            catch(EodhdHttpException ex)
            {
                SkipIfNoLicense(ex);
                throw;
            }

            Assert.NotNull(rates);
            Skip.If(rates.Length == 0, "No Treasury real yield rates returned.");
            Assert.Contains(rates, r => !string.IsNullOrEmpty(r.Tenor));
        }

        [SkippableFact]
        public async Task GetTreasuryBillRates_ReturnsBillRates()
        {
            this.SkipIfNoApiKey();
            using EodhdClient client = this.CreateClient();

            TreasuryBillRate[] rates;
            try
            {
                rates = await client.GetTreasuryBillRatesAsync(year: 2024);
            }
            catch(EodhdHttpException ex)
            {
                SkipIfNoLicense(ex);
                throw;
            }

            Assert.NotNull(rates);
            Skip.If(rates.Length == 0, "No Treasury bill rates returned.");
            Assert.Contains(rates, r => r.Discount.HasValue && !string.IsNullOrEmpty(r.Cusip));
        }

        [SkippableFact]
        public async Task GetTreasuryLongTermRates_ReturnsLongTermRates()
        {
            this.SkipIfNoApiKey();
            using EodhdClient client = this.CreateClient();

            TreasuryLongTermRate[] rates;
            try
            {
                rates = await client.GetTreasuryLongTermRatesAsync(year: 2024);
            }
            catch(EodhdHttpException ex)
            {
                SkipIfNoLicense(ex);
                throw;
            }

            Assert.NotNull(rates);
            Skip.If(rates.Length == 0, "No Treasury long-term rates returned.");
            Assert.Contains(rates, r => !string.IsNullOrEmpty(r.RateType));
        }
    }
}
