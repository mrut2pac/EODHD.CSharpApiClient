using System;
using System.Net;

using EODHD.CSharpApiClient.DataModel.Bonds;
using EODHD.CSharpApiClient.DataModel.EarningsTrends;
using EODHD.CSharpApiClient.DataModel.InsiderTransactions;

using Xunit;

namespace EODHD.CSharpApiClient.UnitTests
{
    public class BondsInsiderTrendsTests
    {
        private static EodhdClient CreateClient(string responseJson, out FakeHttpTransport transport)
        {
            transport = new FakeHttpTransport(HttpStatusCode.OK, responseJson);
            return new EodhdClient(new EodhdClientOptions { ApiToken = "test" }, transport);
        }

        [Fact]
        public async System.Threading.Tasks.Task GetInsiderTransactions_DeserializesFieldsAndNulls()
        {
            const string json =
                "[{\"code\":\"RXST\",\"exchange\":\"US\",\"date\":\"2024-02-01\",\"reportDate\":\"2024-02-05\"," +
                "\"ownerCik\":null,\"ownerName\":\"William J Phd Link\",\"ownerRelationship\":null," +
                "\"ownerTitle\":\"Director\",\"transactionDate\":\"2024-02-01\",\"transactionCode\":\"S\"," +
                "\"transactionAmount\":28250,\"transactionPrice\":49.67,\"transactionAcquiredDisposed\":\"D\"," +
                "\"postTransactionAmount\":null,\"link\":\"http://www.sec.gov/x.xml\"}]";

            using EodhdClient client = CreateClient(json, out FakeHttpTransport transport);

            InsiderTransaction[] transactions = await client.GetInsiderTransactionsAsync(from: new DateTime(2024, 1, 1), limit: 3);

            Assert.Single(transactions);
            InsiderTransaction tx = transactions[0];
            Assert.Equal("RXST", tx.Code);
            Assert.Equal(new DateTime(2024, 2, 1), tx.TransactionDate);
            Assert.Equal(28250m, tx.TransactionAmount);
            Assert.Equal(49.67, tx.TransactionPrice);
            Assert.Equal("D", tx.TransactionAcquiredDisposed);
            Assert.Null(tx.OwnerCik);
            Assert.Null(tx.PostTransactionAmount);
            string uri = transport.LastRequestUri.ToString();
            Assert.Contains("from=2024-01-01", uri, StringComparison.Ordinal);
            Assert.Contains("limit=3", uri, StringComparison.Ordinal);
            Assert.DoesNotContain("code=", uri, StringComparison.Ordinal);
        }

        [Fact]
        public async System.Threading.Tasks.Task GetBondFundamentals_StringEncodedNumbersAndNestedObjects()
        {
            const string json =
                "{\"ISIN\":\"DE000CB83CF0\",\"CUSIP\":\"000CB83CF\",\"Name\":\"Commerzbank, 7.75% 16mar2021, EUR\"," +
                "\"Currency\":\"EUR\",\"Coupon\":\"7.750\",\"Price\":\"112.63\",\"Maturity_Date\":\"2021-03-15\"," +
                "\"YieldToMaturity\":\"0.442\",\"Callable\":\"No\",\"NextCallDate\":null," +
                "\"MinimumSettlementAmount\":\"100000 EUR\"," +
                "\"ClassificationData\":{\"BondType\":\"Corporate bonds Germany\",\"IndustryGroup\":\"Banks\"}," +
                "\"IssueData\":{\"IssueDate\":\"2011-03-16\",\"Issuer\":\"Commerzbank AG\",\"IssuerCountry\":\"Germany\"," +
                "\"IssuerURL\":\"https://www.commerzbank.com/\"}}";

            using EodhdClient client = CreateClient(json, out FakeHttpTransport transport);

            BondFundamentals bond = await client.GetBondFundamentalsAsync("DE000CB83CF0");

            Assert.Equal("DE000CB83CF0", bond.Isin);
            Assert.Equal(7.750, bond.Coupon);
            Assert.Equal(112.63, bond.Price);
            Assert.Equal(0.442, bond.YieldToMaturity);
            Assert.Equal("2021-03-15", bond.MaturityDate);
            Assert.Null(bond.NextCallDate);
            Assert.Equal("Banks", bond.ClassificationData.IndustryGroup);
            Assert.Equal("Commerzbank AG", bond.IssueData.Issuer);
            Assert.Equal("https://www.commerzbank.com/", bond.IssueData.IssuerUrl);
            Assert.Contains("bond-fundamentals/DE000CB83CF0", transport.LastRequestUri.ToString(), StringComparison.Ordinal);
        }

        [Fact]
        public async System.Threading.Tasks.Task GetEarningsTrends_FlattensPerSymbolArraysAndParsesStringNumbers()
        {
            const string json =
                "{\"type\":\"Trends\",\"symbols\":\"AAPL.US,MSFT.US\",\"trends\":[" +
                "[{\"code\":\"AAPL.US\",\"date\":\"2026-09-30\",\"period\":\"+1q\",\"growth\":\"0.0871\"," +
                "\"earningsEstimateAvg\":\"2.0111\",\"revenueEstimateYearAgoEps\":null," +
                "\"epsRevisionsUpLast30days\":\"31.0000\"}]," +
                "[{\"code\":\"MSFT.US\",\"date\":\"2026-06-30\",\"period\":\"0q\",\"growth\":null," +
                "\"earningsEstimateAvg\":\"3.35\"}]]}";

            using EodhdClient client = CreateClient(json, out FakeHttpTransport transport);

            EarningsTrend[] trends = await client.GetEarningsTrendsAsync(new[] { "AAPL.US", "MSFT.US" });

            Assert.Equal(2, trends.Length);
            Assert.Equal("AAPL.US", trends[0].Code);
            Assert.Equal(new DateTime(2026, 9, 30), trends[0].Date);
            Assert.Equal(0.0871, trends[0].Growth);
            Assert.Equal(2.0111, trends[0].EarningsEstimateAvg);
            Assert.Equal(31.0, trends[0].EpsRevisionsUpLast30Days);
            Assert.Null(trends[0].RevenueEstimateYearAgoEps);
            Assert.Equal("MSFT.US", trends[1].Code);
            Assert.Null(trends[1].Growth);
            Assert.Contains("symbols=AAPL.US%2CMSFT.US", transport.LastRequestUri.ToString(), StringComparison.Ordinal);
        }

        [Fact]
        public async System.Threading.Tasks.Task GetEarningsTrends_EmptyTrends_ReturnsEmptyArray()
        {
            using EodhdClient client = CreateClient("{\"type\":\"Trends\",\"trends\":[]}", out FakeHttpTransport _);

            EarningsTrend[] trends = await client.GetEarningsTrendsAsync(new[] { "AAPL.US" });

            Assert.NotNull(trends);
            Assert.Empty(trends);
        }
    }
}
