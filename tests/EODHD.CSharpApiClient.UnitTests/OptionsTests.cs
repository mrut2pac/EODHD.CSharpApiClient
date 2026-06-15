using System;
using System.Net;

using EODHD.CSharpApiClient.DataModel.Options;

using Xunit;

namespace EODHD.CSharpApiClient.UnitTests
{
    public class OptionsTests
    {
        private static EodhdClient CreateClient(string responseJson, out FakeHttpTransport transport)
        {
            transport = new FakeHttpTransport(HttpStatusCode.OK, responseJson);
            return new EodhdClient(new EodhdClientOptions { ApiToken = "test" }, transport);
        }

        [Fact]
        public async System.Threading.Tasks.Task GetOptionsChain_Legacy_DeserializesNestedChainAndDateSentinels()
        {
            const string json =
                "{\"code\":\"AAPL\",\"exchange\":\"US\",\"lastTradeDate\":\"2026-06-15\",\"lastTradePrice\":296.42," +
                "\"data\":[{\"expirationDate\":\"2026-06-15\",\"impliedVolatility\":24.303,\"putVolume\":49421," +
                "\"callVolume\":90795,\"putCallVolumeRatio\":0.5443,\"optionsCount\":86,\"options\":{" +
                "\"CALL\":[{\"contractName\":\"AAPL260615C00250000\",\"contractSize\":\"REGULAR\"," +
                "\"contractPeriod\":\"WEEKLY\",\"type\":\"CALL\",\"inTheMoney\":\"TRUE\"," +
                "\"lastTradeDateTime\":\"0000-00-00 00:00:00\",\"expirationDate\":\"2026-06-15\",\"strike\":250," +
                "\"lastPrice\":41.72,\"bid\":39.65,\"ask\":42.75,\"volume\":1,\"openInterest\":1,\"delta\":0.9953," +
                "\"updatedAt\":\"2026-06-14 19:07:41\",\"daysBeforeExpiration\":0}]," +
                "\"PUT\":[{\"contractName\":\"AAPL260615P00250000\",\"type\":\"PUT\",\"inTheMoney\":\"FALSE\",\"strike\":250,\"bid\":0.01}]}}]}";

            using EodhdClient client = CreateClient(json, out FakeHttpTransport transport);

            OptionsChain chain = await client.GetOptionsChainAsync("AAPL.US");

            Assert.Equal("AAPL", chain.Code);
            Assert.Equal(new DateTime(2026, 6, 15), chain.LastTradeDate);
            Assert.Single(chain.Data);
            OptionsExpiration expiration = chain.Data[0];
            Assert.Equal(86, expiration.OptionsCount);
            Assert.Single(expiration.Options.Call);
            OptionContract call = expiration.Options.Call[0];
            Assert.Equal(250, call.Strike);
            Assert.Equal("TRUE", call.InTheMoney);
            Assert.Equal("0000-00-00 00:00:00", call.LastTradeDateTime);
            Assert.Equal("2026-06-14 19:07:41", call.UpdatedAt);
            Assert.Equal(0.9953, call.Delta);
            Assert.Equal(0, call.DaysBeforeExpiration);
            Assert.Equal("FALSE", expiration.Options.Put[0].InTheMoney);
            Assert.Contains("options/AAPL.US", transport.LastRequestUri.ToString(), StringComparison.Ordinal);
        }

        [Fact]
        public async System.Threading.Tasks.Task GetOptionsEod_Marketplace_UnwrapsAttributesAndMapsRenamedFields()
        {
            const string json =
                "{\"meta\":{\"offset\":0,\"limit\":1},\"data\":[{\"id\":\"AAPL281215P00600000-2026-06-12\"," +
                "\"type\":\"options-eod\",\"attributes\":{\"contract\":\"AAPL281215P00600000\"," +
                "\"underlying_symbol\":\"AAPL\",\"exp_date\":\"2028-12-15\",\"expiration_type\":\"monthly\"," +
                "\"type\":\"put\",\"strike\":600,\"exchange\":\"NASDAQ\",\"currency\":\"USD\",\"last_size\":0," +
                "\"pctchange\":1.5,\"bid\":306.5,\"bid_date\":\"2026-06-13T03:59:59.000000Z\",\"bid_size\":1," +
                "\"ask\":311.5,\"ask_date\":\"2026-06-12 19:59:59\",\"ask_size\":1,\"moneyness\":1.06," +
                "\"volume\":0,\"open_interest\":0,\"volatility\":0.4142,\"delta\":-0.915221,\"gamma\":0.000802," +
                "\"theta\":-0.004639,\"vega\":0.538006,\"rho\":-0.999999,\"tradetime\":null,\"vol_oi_ratio\":0," +
                "\"dte\":916,\"midpoint\":309}}],\"links\":{\"next\":\"https://eodhd.com/next\"}}";

            using EodhdClient client = CreateClient(json, out FakeHttpTransport transport);

            OptionData[] records = await client.GetOptionsEodAsync("AAPL", optionType: "put", limit: 1, sort: "-exp_date");

            Assert.Single(records);
            OptionData record = records[0];
            Assert.Equal("AAPL281215P00600000", record.Contract);
            Assert.Equal("AAPL", record.UnderlyingSymbol);
            Assert.Equal("2028-12-15", record.ExpirationDate);
            Assert.Equal(600, record.Strike);
            Assert.Equal(1.5, record.PercentChange);
            Assert.Equal("2026-06-13T03:59:59.000000Z", record.BidDate);
            Assert.Equal("2026-06-12 19:59:59", record.AskDate);
            Assert.Equal(-0.915221, record.Delta);
            Assert.Null(record.TradeTime);
            Assert.Equal(916, record.DaysToExpiration);
            Assert.Equal(309, record.Midpoint);

            string uri = Uri.UnescapeDataString(transport.LastRequestUri.ToString());
            Assert.Contains("mp/unicornbay/options/eod", uri, StringComparison.Ordinal);
            Assert.Contains("underlying_symbol", uri, StringComparison.Ordinal);
            Assert.Contains("AAPL", uri, StringComparison.Ordinal);
            Assert.Contains("type]=put", uri, StringComparison.Ordinal);
            Assert.Contains("sort=-exp_date", uri, StringComparison.Ordinal);
        }

        [Fact]
        public async System.Threading.Tasks.Task GetOptionContracts_Marketplace_HitsContractsPathAndUnwraps()
        {
            const string json =
                "{\"data\":[{\"id\":\"AAPL281215C00600000\",\"type\":\"options-contracts\"," +
                "\"attributes\":{\"contract\":\"AAPL281215C00600000\",\"underlying_symbol\":\"AAPL\"," +
                "\"type\":\"call\",\"strike\":600,\"last\":5.6,\"previous_date\":\"2026-06-11\",\"tradetime\":\"2026-06-12\"}}]}";

            using EodhdClient client = CreateClient(json, out FakeHttpTransport transport);

            OptionData[] records = await client.GetOptionContractsAsync("AAPL", strikeFrom: 100, strikeTo: 700);

            Assert.Single(records);
            Assert.Equal("call", records[0].Type);
            Assert.Equal("2026-06-11", records[0].PreviousDate);
            Assert.Equal("2026-06-12", records[0].TradeTime);
            string uri = Uri.UnescapeDataString(transport.LastRequestUri.ToString());
            Assert.Contains("mp/unicornbay/options/contracts", uri, StringComparison.Ordinal);
            Assert.Contains("strike_from]=100", uri, StringComparison.Ordinal);
            Assert.Contains("strike_to]=700", uri, StringComparison.Ordinal);
        }

        [Fact]
        public async System.Threading.Tasks.Task GetOptionsEod_NoData_ReturnsEmptyArray()
        {
            using EodhdClient client = CreateClient("{\"data\":[]}", out FakeHttpTransport _);

            OptionData[] records = await client.GetOptionsEodAsync("AAPL");

            Assert.NotNull(records);
            Assert.Empty(records);
        }
    }
}
