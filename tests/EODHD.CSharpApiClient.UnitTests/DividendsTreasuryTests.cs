using System;
using System.Net;

using EODHD.CSharpApiClient.DataModel.Treasury;
using EODHD.CSharpApiClient.DataModel.UpcomingDividends;

using Xunit;

namespace EODHD.CSharpApiClient.UnitTests
{
    public class DividendsTreasuryTests
    {
        private static EodhdClient CreateClient(string responseJson, out FakeHttpTransport transport)
        {
            transport = new FakeHttpTransport(HttpStatusCode.OK, responseJson);
            return new EodhdClient(new EodhdClientOptions { ApiToken = "test" }, transport);
        }

        [Fact]
        public async System.Threading.Tasks.Task GetUpcomingDividends_UnwrapsDataAndSendsSymbolFilter()
        {
            const string json =
                "{\"meta\":{\"total\":2,\"symbol\":\"AAPL.US\"},\"data\":[" +
                "{\"date\":\"2026-05-11\",\"symbol\":\"AAPL.US\"},{\"date\":\"2026-02-09\",\"symbol\":\"AAPL.US\"}]}";

            using EodhdClient client = CreateClient(json, out FakeHttpTransport transport);

            UpcomingDividend[] dividends = await client.GetUpcomingDividendsAsync("AAPL.US");

            Assert.Equal(2, dividends.Length);
            Assert.Equal(new DateTime(2026, 5, 11), dividends[0].Date);
            Assert.Equal("AAPL.US", dividends[0].Symbol);
            string uri = Uri.UnescapeDataString(transport.LastRequestUri.ToString());
            Assert.Contains("calendar/dividends", uri, StringComparison.Ordinal);
            Assert.Contains("filter[symbol]=AAPL.US", uri, StringComparison.Ordinal);
        }

        [Fact]
        public async System.Threading.Tasks.Task GetUpcomingDividends_NeitherSymbolNorDate_Throws()
        {
            using EodhdClient client = CreateClient("{\"data\":[]}", out FakeHttpTransport _);

            await Assert.ThrowsAsync<ArgumentException>(() => client.GetUpcomingDividendsAsync());
        }

        [Fact]
        public async System.Threading.Tasks.Task GetUpcomingDividends_DateOnly_IsAllowedAndSendsDateEq()
        {
            using EodhdClient client = CreateClient("{\"data\":[]}", out FakeHttpTransport transport);

            await client.GetUpcomingDividendsAsync(dateEqual: new DateTime(2026, 5, 11));

            string uri = Uri.UnescapeDataString(transport.LastRequestUri.ToString());
            Assert.Contains("filter[date_eq]=2026-05-11", uri, StringComparison.Ordinal);
        }

        [Fact]
        public async System.Threading.Tasks.Task GetTreasuryYieldRates_DeserializesTenorRowsAndSendsYearFilter()
        {
            const string json =
                "{\"meta\":{\"total\":2},\"data\":[" +
                "{\"date\":\"2025-01-02\",\"tenor\":\"1M\",\"rate\":4.45}," +
                "{\"date\":\"2025-01-02\",\"tenor\":\"10Y\",\"rate\":4.57}]}";

            using EodhdClient client = CreateClient(json, out FakeHttpTransport transport);

            TreasuryRate[] rates = await client.GetTreasuryYieldRatesAsync(year: 2025);

            Assert.Equal(2, rates.Length);
            Assert.Equal("1M", rates[0].Tenor);
            Assert.Equal(4.45, rates[0].Rate);
            Assert.Equal(new DateTime(2025, 1, 2), rates[1].Date);
            string uri = Uri.UnescapeDataString(transport.LastRequestUri.ToString());
            Assert.Contains("ust/yield-rates", uri, StringComparison.Ordinal);
            Assert.Contains("filter[year]=2025", uri, StringComparison.Ordinal);
        }

        [Fact]
        public async System.Threading.Tasks.Task GetTreasuryBillRates_DeserializesDiscountCouponAndCusip()
        {
            const string json =
                "{\"data\":[{\"date\":\"2026-01-02\",\"tenor\":\"4WK\",\"discount\":3.58,\"coupon\":3.64," +
                "\"avg_discount\":3.58,\"avg_coupon\":3.64,\"maturity_date\":\"2026-02-03\",\"cusip\":\"912797SJ7\"}]}";

            using EodhdClient client = CreateClient(json, out FakeHttpTransport _);

            TreasuryBillRate[] rates = await client.GetTreasuryBillRatesAsync();

            Assert.Single(rates);
            Assert.Equal(3.58, rates[0].Discount);
            Assert.Equal(3.64, rates[0].AverageCoupon);
            Assert.Equal(new DateTime(2026, 2, 3), rates[0].MaturityDate);
            Assert.Equal("912797SJ7", rates[0].Cusip);
        }

        [Fact]
        public async System.Threading.Tasks.Task GetTreasuryLongTermRates_DeserializesRateTypeAndNullFactor()
        {
            const string json =
                "{\"data\":[{\"date\":\"2026-01-02\",\"rate_type\":\"BC_20year\",\"rate\":4.81,\"extrapolation_factor\":null}]}";

            using EodhdClient client = CreateClient(json, out FakeHttpTransport _);

            TreasuryLongTermRate[] rates = await client.GetTreasuryLongTermRatesAsync();

            Assert.Single(rates);
            Assert.Equal("BC_20year", rates[0].RateType);
            Assert.Equal(4.81, rates[0].Rate);
            Assert.Null(rates[0].ExtrapolationFactor);
        }
    }
}
