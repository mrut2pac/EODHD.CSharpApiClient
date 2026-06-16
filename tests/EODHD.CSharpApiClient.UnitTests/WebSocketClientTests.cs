using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using EODHD.CSharpApiClient.DataModel.Live;
using EODHD.CSharpApiClient.Live;
using EODHD.CSharpApiClient.Transport;

using Xunit;

namespace EODHD.CSharpApiClient.UnitTests
{
    public class WebSocketClientTests
    {
        // Hands back a preset sequence of received messages, then signals close (null), and records sends.
        private sealed class FakeConnection : IWebSocketConnection
        {
            private readonly Queue<string> incoming;

            public FakeConnection(params string[] messages)
            {
                this.incoming = new Queue<string>(messages);
            }

            public List<string> Sent { get; } = new List<string>();

            public Uri LastUri { get; private set; }

            public Task ConnectAsync(Uri uri, CancellationToken cancellationToken)
            {
                this.LastUri = uri;
                return Task.CompletedTask;
            }

            public Task SendTextAsync(string message, CancellationToken cancellationToken)
            {
                this.Sent.Add(message);
                return Task.CompletedTask;
            }

            public Task<string> ReceiveTextAsync(CancellationToken cancellationToken)
            {
                return Task.FromResult(this.incoming.Count > 0 ? this.incoming.Dequeue() : null);
            }

            public void Dispose()
            {
            }
        }

        private static EodhdWebSocketOptions NoReconnect()
        {
            return new EodhdWebSocketOptions { AutoReconnect = false };
        }

        private static async Task<List<TMessage>> DrainAsync<TMessage>(EodhdWebSocketClient<TMessage> client, IReadOnlyList<string> symbols)
        {
            List<TMessage> received = new List<TMessage>();
            await foreach(TMessage message in client.ReadMessagesAsync(symbols))
            {
                received.Add(message);
            }

            return received;
        }

        [Fact]
        public async Task ReadMessages_ParsesData_SkipsControl_AndSubscribes()
        {
            FakeConnection connection = new FakeConnection(
                "{\"status_code\":200,\"message\":\"Authorized\"}",
                "{\"s\":\"EURUSD\",\"a\":1.1589,\"b\":1.1586,\"dc\":\"-0.139\",\"dd\":\"-0.0016\",\"ppms\":true,\"t\":1781593336816}");

            using EodhdWebSocketClient<ForexQuote> client = new EodhdWebSocketClient<ForexQuote>(
                "test", "forex", NoReconnect(), () => connection);

            List<ForexQuote> quotes = await DrainAsync(client, new[] { "EURUSD" });

            Assert.Single(quotes);
            Assert.Equal("EURUSD", quotes[0].Symbol);
            Assert.Equal(1.1589, quotes[0].Ask);
            Assert.Equal(1.1586, quotes[0].Bid);
            Assert.Equal(-0.0016, quotes[0].DayChange);
            Assert.True(quotes[0].Ppms);
            Assert.Equal(1781593336816, quotes[0].Timestamp);
            Assert.Contains("{\"action\":\"subscribe\",\"symbols\":\"EURUSD\"}", connection.Sent);
            Assert.Contains("forex?api_token=test", connection.LastUri.ToString(), StringComparison.Ordinal);
        }

        [Fact]
        public async Task ReadMessages_MultipleSymbols_SubscribesCommaJoinedInOrder()
        {
            FakeConnection connection = new FakeConnection();
            using EodhdWebSocketClient<CryptoTrade> client = new EodhdWebSocketClient<CryptoTrade>(
                "test", "crypto", NoReconnect(), () => connection);

            await DrainAsync(client, new[] { "BTC-USD", "ETH-USD" });

            Assert.Contains("{\"action\":\"subscribe\",\"symbols\":\"BTC-USD,ETH-USD\"}", connection.Sent);
        }

        [Fact]
        public async Task ReadMessages_Reconnects_AndResubscribes_OnDrop()
        {
            FakeConnection first = new FakeConnection(
                "{\"s\":\"BTC-USD\",\"p\":\"66408.48\",\"q\":\"1\",\"dc\":\"0.99\",\"dd\":\"660.2\",\"t\":1781593338284}");
            FakeConnection second = new FakeConnection(
                "{\"s\":\"BTC-USD\",\"p\":\"66410.10\",\"q\":\"2\",\"dc\":\"1.01\",\"dd\":\"662.0\",\"t\":1781593339000}");
            Queue<IWebSocketConnection> connections = new Queue<IWebSocketConnection>(new IWebSocketConnection[] { first, second });

            EodhdWebSocketOptions options = new EodhdWebSocketOptions
            {
                AutoReconnect = true,
                MaxReconnectAttempts = 1,
                ReconnectBaseDelay = TimeSpan.Zero,
            };

            // After the two healthy connections drop, hand out empty connections that connect then close
            // immediately; the bounded reconnect logic must terminate after MaxReconnectAttempts.
            using EodhdWebSocketClient<CryptoTrade> client = new EodhdWebSocketClient<CryptoTrade>(
                "test", "crypto", options, () => connections.Count > 0 ? connections.Dequeue() : new FakeConnection());

            List<CryptoTrade> trades = await DrainAsync(client, new[] { "BTC-USD" });

            Assert.Equal(2, trades.Count);
            Assert.Equal(66408.48, trades[0].Price);
            Assert.Equal(66410.10, trades[1].Price);
            // The reconnect replayed the subscription on the second connection.
            Assert.Contains("{\"action\":\"subscribe\",\"symbols\":\"BTC-USD\"}", second.Sent);
        }

        [Fact]
        public async Task Subscribe_OverFiftySymbols_Throws()
        {
            FakeConnection connection = new FakeConnection();
            using EodhdWebSocketClient<ForexQuote> client = new EodhdWebSocketClient<ForexQuote>(
                "test", "forex", NoReconnect(), () => connection);

            string[] symbols = new string[51];
            for(int i = 0; i < symbols.Length; i++)
            {
                symbols[i] = "SYM" + i.ToString(System.Globalization.CultureInfo.InvariantCulture);
            }

            await Assert.ThrowsAsync<ArgumentException>(() => client.SubscribeAsync(symbols));
        }

        [Fact]
        public async Task ReadMessages_ParsesUsTrade_WithConditionCodes()
        {
            FakeConnection connection = new FakeConnection(
                "{\"s\":\"TSLA\",\"p\":405.3462,\"c\":[14,37,41],\"v\":7,\"dp\":false,\"ms\":\"extended-hours\",\"t\":1781596800421}");
            using EodhdWebSocketClient<UsTrade> client = new EodhdWebSocketClient<UsTrade>(
                "test", "us", NoReconnect(), () => connection);

            List<UsTrade> trades = await DrainAsync(client, new[] { "TSLA" });

            Assert.Single(trades);
            Assert.Equal("TSLA", trades[0].Symbol);
            Assert.Equal(405.3462, trades[0].Price);
            Assert.Equal(new[] { 14, 37, 41 }, trades[0].Conditions);
            Assert.Equal(7, trades[0].Volume);
            Assert.False(trades[0].DarkPool);
            Assert.Equal("extended-hours", trades[0].MarketStatus);
            Assert.Equal(1781596800421, trades[0].Timestamp);
        }

        [Fact]
        public async Task ReadMessages_ParsesUsQuote_TopOfBook()
        {
            FakeConnection connection = new FakeConnection(
                "{\"s\":\"TSLA\",\"ap\":405.8,\"as\":200,\"bp\":405.05,\"bs\":40,\"t\":1781596800421}");
            using EodhdWebSocketClient<UsQuote> client = new EodhdWebSocketClient<UsQuote>(
                "test", "us-quote", NoReconnect(), () => connection);

            List<UsQuote> quotes = await DrainAsync(client, new[] { "TSLA" });

            Assert.Single(quotes);
            Assert.Equal("TSLA", quotes[0].Symbol);
            Assert.Equal(405.8, quotes[0].AskPrice);
            Assert.Equal(200, quotes[0].AskSize);
            Assert.Equal(405.05, quotes[0].BidPrice);
            Assert.Equal(40, quotes[0].BidSize);
            Assert.Equal(1781596800421, quotes[0].Timestamp);
        }

        [Fact]
        public void Constructor_NullToken_Throws()
        {
            Assert.Throws<ArgumentException>(() => new EodhdWebSocketClient<ForexQuote>(null, "forex"));
        }

        [Fact]
        public void Factory_Forex_CreatesClientWithNoSymbols()
        {
            using EodhdWebSocketClient<ForexQuote> client = EodhdWebSocketClient.Forex("test");

            Assert.Empty(client.Symbols);
        }
    }
}
