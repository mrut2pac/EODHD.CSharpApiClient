using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using EODHD.CSharpApiClient.DataModel.Live;
using EODHD.CSharpApiClient.Live;

using Xunit;

namespace EODHD.CSharpApiClient.IntegrationTests
{
    public class WebSocketIntegrationTests : IntegrationTestBase
    {
        [SkippableFact]
        public async Task Forex_StreamsLiveQuotes()
        {
            this.SkipIfNoApiKey();
            using EodhdWebSocketClient<ForexQuote> client = EodhdWebSocketClient.Forex(this.ApiToken);

            ForexQuote first = await FirstOrNullAsync(client.ReadMessagesAsync(new[] { "EURUSD" }, Timeout()));

            Skip.If(first == null, "No live forex quote received within the timeout (feed may be unavailable on this plan).");
            Assert.Equal("EURUSD", first.Symbol);
            Assert.True(first.Ask.HasValue || first.Bid.HasValue);
            Assert.True(first.TimestampMs.HasValue);
        }

        [SkippableFact]
        public async Task Crypto_StreamsLiveTrades()
        {
            this.SkipIfNoApiKey();
            using EodhdWebSocketClient<CryptoTrade> client = EodhdWebSocketClient.Crypto(this.ApiToken);

            CryptoTrade first = await FirstOrNullAsync(client.ReadMessagesAsync(new[] { "BTC-USD" }, Timeout()));

            Skip.If(first == null, "No live crypto trade received within the timeout (feed may be unavailable on this plan).");
            Assert.Equal("BTC-USD", first.Symbol);
            Assert.True(first.Price.HasValue);
            Assert.True(first.TimestampMs.HasValue);
        }

        [SkippableFact]
        public async Task UsTrades_StreamsLiveTrades()
        {
            this.SkipIfNoApiKey();
            using EodhdWebSocketClient<UsTrade> client = EodhdWebSocketClient.UsTrades(this.ApiToken);

            UsTrade first = await FirstOrNullAsync(client.ReadMessagesAsync(new[] { "SPY", "TSLA", "AAPL" }, Timeout()));

            Skip.If(first == null, "No live US trade received (US market likely closed — streams during regular and extended hours only).");
            Assert.False(string.IsNullOrEmpty(first.Symbol));
            Assert.True(first.TimestampMs.HasValue);
        }

        [SkippableFact]
        public async Task UsQuotes_StreamsLiveQuotes()
        {
            this.SkipIfNoApiKey();
            using EodhdWebSocketClient<UsQuote> client = EodhdWebSocketClient.UsQuotes(this.ApiToken);

            UsQuote first = await FirstOrNullAsync(client.ReadMessagesAsync(new[] { "SPY", "TSLA", "AAPL" }, Timeout()));

            Skip.If(first == null, "No live US quote received (US market likely closed — streams during regular and extended hours only).");
            Assert.False(string.IsNullOrEmpty(first.Symbol));
            Assert.True(first.AskPrice.HasValue || first.BidPrice.HasValue);
        }

        private static CancellationToken Timeout()
        {
            return new CancellationTokenSource(TimeSpan.FromSeconds(25)).Token;
        }

        private static async Task<TMessage> FirstOrNullAsync<TMessage>(IAsyncEnumerable<TMessage> stream)
            where TMessage : class
        {
            await foreach(TMessage message in stream)
            {
                return message;
            }

            return null;
        }
    }
}
