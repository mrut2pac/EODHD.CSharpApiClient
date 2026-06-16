using System;
using System.Linq;
using System.Threading.Tasks;

using EODHD.CSharpApiClient.DataModel.Bonds;
using EODHD.CSharpApiClient.DataModel.EarningsTrends;
using EODHD.CSharpApiClient.DataModel.InsiderTransactions;
using EODHD.CSharpApiClient.Exceptions;

using Xunit;

namespace EODHD.CSharpApiClient.IntegrationTests
{
    public class BondsInsiderTrendsIntegrationTests : IntegrationTestBase
    {
        [SkippableFact]
        public async Task GetInsiderTransactions_RecentRange_ReturnsTransactions()
        {
            this.SkipIfNoApiKey();
            using EodhdClient client = this.CreateClient();

            InsiderTransaction[] transactions;
            try
            {
                transactions = await client.GetInsiderTransactionsAsync(from: new DateTime(2024, 1, 1), to: new DateTime(2024, 2, 1), limit: 10);
            }
            catch(EodhdHttpException ex)
            {
                SkipIfNoLicense(ex);
                throw;
            }

            Assert.NotNull(transactions);
            Skip.If(transactions.Length == 0, "No insider transactions returned for the requested range.");
            Assert.False(string.IsNullOrEmpty(transactions[0].Code));
            Assert.False(string.IsNullOrEmpty(transactions[0].TransactionCode));
        }

        [SkippableFact]
        public async Task GetBondFundamentals_KnownIsin_ReturnsBond()
        {
            this.SkipIfNoApiKey();
            using EodhdClient client = this.CreateClient();

            BondFundamentals bond;
            try
            {
                bond = await client.GetBondFundamentalsAsync("DE000CB83CF0");
            }
            catch(EodhdHttpException ex)
            {
                SkipIfNoLicense(ex);
                throw;
            }

            Assert.NotNull(bond);
            Skip.If(string.IsNullOrEmpty(bond.Isin), "No bond fundamentals returned for the requested ISIN.");
            Assert.Equal("DE000CB83CF0", bond.Isin);
            Assert.NotNull(bond.IssueData);
        }

        [SkippableFact]
        public async Task GetEarningsTrends_Apple_ReturnsTrends()
        {
            this.SkipIfNoApiKey();
            using EodhdClient client = this.CreateClient();

            EarningsTrend[] trends;
            try
            {
                trends = await client.GetEarningsTrendsAsync(new[] { "AAPL.US" });
            }
            catch(EodhdHttpException ex)
            {
                SkipIfNoLicense(ex);
                throw;
            }

            Assert.NotNull(trends);
            Skip.If(trends.Length == 0, "No earnings trends returned for the requested symbol.");
            Assert.Contains(trends, t => t.Code == "AAPL.US");
            Assert.False(string.IsNullOrEmpty(trends[0].Period));
        }

        [SkippableFact]
        public async Task GetEarningsTrends_MultipleSymbols_ReturnsFlattenedAcrossSymbols()
        {
            this.SkipIfNoApiKey();
            using EodhdClient client = this.CreateClient();

            EarningsTrend[] trends;
            try
            {
                trends = await client.GetEarningsTrendsAsync(new[] { "AAPL.US", "MSFT.US" });
            }
            catch(EodhdHttpException ex)
            {
                SkipIfNoLicense(ex);
                throw;
            }

            Assert.NotNull(trends);
            Skip.If(trends.Length == 0, "No earnings trends returned for the requested symbols.");
            Assert.True(trends.Select(t => t.Code).Distinct().Count() >= 2);
        }
    }
}
