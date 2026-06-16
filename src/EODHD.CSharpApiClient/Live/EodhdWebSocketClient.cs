using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

using EODHD.CSharpApiClient.Transport;

namespace EODHD.CSharpApiClient.Live
{
    /// <summary>
    /// Streaming client for an EODHD live (WebSocket) feed. Connects on demand, replays subscriptions on
    /// reconnect, and surfaces messages as an <see cref="IAsyncEnumerable{T}"/>. Construct one via the
    /// factory methods on <see cref="EodhdWebSocketClient"/> (e.g. <see cref="EodhdWebSocketClient.Forex"/>).
    /// </summary>
    /// <typeparam name="TMessage">The strongly-typed message for the feed.</typeparam>
    public sealed class EodhdWebSocketClient<TMessage> : IDisposable
    {
        /// <summary>The maximum number of symbols that may be subscribed at once.</summary>
        public const int MaxSymbols = 50;

        private static readonly JsonSerializerOptions DeserializeOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            NumberHandling = JsonNumberHandling.AllowReadingFromString,
        };

        private readonly string apiToken;
        private readonly string feed;
        private readonly EodhdWebSocketOptions options;
        private readonly Func<IWebSocketConnection> connectionFactory;
        private readonly List<string> symbols = new List<string>();
        private readonly object symbolsLock = new object();
        private readonly SemaphoreSlim sendLock = new SemaphoreSlim(1, 1);

        private volatile IWebSocketConnection connection;
        private volatile bool disposed;

        /// <summary>
        /// Initialises a new live client for the given feed.
        /// </summary>
        /// <param name="apiToken">The EODHD API token.</param>
        /// <param name="feed">The feed name (e.g. <c>"forex"</c>, <c>"crypto"</c>, <c>"us"</c>, <c>"us-quote"</c>).</param>
        /// <param name="options">Optional connection settings; defaults are used when <see langword="null"/>.</param>
        /// <param name="connectionFactory">Optional factory for the underlying connection (useful for tests).</param>
        public EodhdWebSocketClient(string apiToken, string feed, EodhdWebSocketOptions options = null, Func<IWebSocketConnection> connectionFactory = null)
        {
            if(string.IsNullOrWhiteSpace(apiToken))
            {
                throw new ArgumentException("An API token must be supplied.", nameof(apiToken));
            }

            if(string.IsNullOrWhiteSpace(feed))
            {
                throw new ArgumentException("A feed must be supplied.", nameof(feed));
            }

            this.apiToken = apiToken;
            this.feed = feed;
            this.options = options ?? new EodhdWebSocketOptions();
            this.connectionFactory = connectionFactory ?? (() => new DefaultWebSocketConnection());
        }

        /// <summary>Gets a snapshot of the currently subscribed symbols.</summary>
        public IReadOnlyCollection<string> Symbols
        {
            get
            {
                lock(this.symbolsLock)
                {
                    return new List<string>(this.symbols);
                }
            }
        }

        /// <summary>
        /// Adds symbols to the subscription. If the stream is connected, the subscription is sent
        /// immediately; otherwise it takes effect on the next connect.
        /// </summary>
        /// <param name="newSymbols">The symbols to add.</param>
        /// <param name="ct">Cancellation token.</param>
        public async Task SubscribeAsync(IReadOnlyList<string> newSymbols, CancellationToken ct = default)
        {
            List<string> added = this.AddSymbols(newSymbols);
            if(added.Count > 0 && this.connection != null)
            {
                await this.SendActionAsync("subscribe", added, ct).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Removes symbols from the subscription. If the stream is connected, the unsubscribe is sent
        /// immediately.
        /// </summary>
        /// <param name="oldSymbols">The symbols to remove.</param>
        /// <param name="ct">Cancellation token.</param>
        public async Task UnsubscribeAsync(IReadOnlyList<string> oldSymbols, CancellationToken ct = default)
        {
            if(oldSymbols == null || oldSymbols.Count == 0)
            {
                return;
            }

            List<string> removed = new List<string>();
            lock(this.symbolsLock)
            {
                foreach(string symbol in oldSymbols)
                {
                    if(string.IsNullOrWhiteSpace(symbol))
                    {
                        continue;
                    }

                    int index = this.symbols.FindIndex(existing => string.Equals(existing, symbol, StringComparison.OrdinalIgnoreCase));
                    if(index >= 0)
                    {
                        removed.Add(this.symbols[index]);
                        this.symbols.RemoveAt(index);
                    }
                }
            }

            if(removed.Count > 0 && this.connection != null)
            {
                await this.SendActionAsync("unsubscribe", removed, ct).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Subscribes to <paramref name="streamSymbols"/> and streams messages. Equivalent to calling
        /// <see cref="SubscribeAsync"/> followed by <see cref="ReadMessagesAsync(CancellationToken)"/>.
        /// </summary>
        /// <param name="streamSymbols">The symbols to subscribe to.</param>
        /// <param name="ct">Cancellation token; cancelling ends the stream.</param>
        /// <returns>An asynchronous stream of messages.</returns>
        public IAsyncEnumerable<TMessage> ReadMessagesAsync(IReadOnlyList<string> streamSymbols, CancellationToken ct = default)
        {
            this.AddSymbols(streamSymbols);
            return this.ReadMessagesAsync(ct);
        }

        /// <summary>
        /// Streams messages for the currently subscribed symbols, connecting (and, if enabled,
        /// reconnecting with backoff and replaying subscriptions) as needed.
        /// </summary>
        /// <param name="ct">Cancellation token; cancelling ends the stream.</param>
        /// <returns>An asynchronous stream of messages.</returns>
        public async IAsyncEnumerable<TMessage> ReadMessagesAsync([EnumeratorCancellation] CancellationToken ct = default)
        {
            int attempt = 0;

            while(!ct.IsCancellationRequested && !this.disposed)
            {
                bool connected = false;
                try
                {
                    await this.OpenAndSubscribeAsync(ct).ConfigureAwait(false);
                    connected = true;
                }
                catch(OperationCanceledException)
                {
                    yield break;
                }
                catch(Exception ex) when(IsTransient(ex))
                {
                    connected = false;
                }

                bool receivedAny = false;
                if(connected)
                {
                    while(true)
                    {
                        string message = null;
                        bool received = true;
                        try
                        {
                            message = await this.connection.ReceiveTextAsync(ct).ConfigureAwait(false);
                        }
                        catch(OperationCanceledException)
                        {
                            yield break;
                        }
                        catch(Exception ex) when(IsTransient(ex))
                        {
                            received = false;
                        }

                        if(!received || message == null)
                        {
                            break;
                        }

                        if(IsControlMessage(message))
                        {
                            continue;
                        }

                        if(TryDeserialize(message, out TMessage parsed))
                        {
                            receivedAny = true;
                            yield return parsed;
                        }
                    }
                }

                this.DisposeConnection();

                if(!this.options.AutoReconnect || this.disposed)
                {
                    yield break;
                }

                // Reset the budget only after a healthy connection (one that delivered data); a connection
                // that opens then immediately drops counts against MaxReconnectAttempts so it cannot loop forever.
                if(receivedAny)
                {
                    attempt = 0;
                }

                attempt++;
                if(attempt > this.options.MaxReconnectAttempts)
                {
                    yield break;
                }

                double delayMs = this.options.ReconnectBaseDelay.TotalMilliseconds * Math.Pow(2, attempt - 1);
                double capMs = this.options.MaxReconnectDelay.TotalMilliseconds;
                TimeSpan delay = TimeSpan.FromMilliseconds(Math.Min(delayMs, capMs));
                bool delayed = true;
                try
                {
                    await Task.Delay(delay, ct).ConfigureAwait(false);
                }
                catch(OperationCanceledException)
                {
                    delayed = false;
                }

                if(!delayed)
                {
                    yield break;
                }
            }
        }

        /// <summary>
        /// Ends any active stream and releases the underlying connection. The preferred way to stop a
        /// stream is to cancel the <see cref="CancellationToken"/> passed to <see cref="ReadMessagesAsync(CancellationToken)"/>.
        /// </summary>
        public void Dispose()
        {
            this.disposed = true;
            this.DisposeConnection();
        }

        // Transient connection failures that should trigger a reconnect rather than surface as a bug.
        private static bool IsTransient(Exception ex)
        {
            return ex is WebSocketException || ex is HttpRequestException || ex is IOException || ex is SocketException || ex is ObjectDisposedException;
        }

        private static bool IsControlMessage(string message)
        {
            try
            {
                using(JsonDocument document = JsonDocument.Parse(message))
                {
                    return document.RootElement.ValueKind == JsonValueKind.Object
                        && document.RootElement.TryGetProperty("status_code", out _);
                }
            }
            catch(JsonException)
            {
                return true;
            }
        }

        private static bool TryDeserialize(string message, out TMessage parsed)
        {
            try
            {
                parsed = JsonSerializer.Deserialize<TMessage>(message, DeserializeOptions);
                return parsed != null;
            }
            catch(JsonException)
            {
                parsed = default;
                return false;
            }
        }

        private List<string> AddSymbols(IReadOnlyList<string> newSymbols)
        {
            List<string> added = new List<string>();
            if(newSymbols == null || newSymbols.Count == 0)
            {
                return added;
            }

            lock(this.symbolsLock)
            {
                foreach(string symbol in newSymbols)
                {
                    if(string.IsNullOrWhiteSpace(symbol))
                    {
                        continue;
                    }

                    bool exists = this.symbols.FindIndex(existing => string.Equals(existing, symbol, StringComparison.OrdinalIgnoreCase)) >= 0;
                    if(!exists)
                    {
                        this.symbols.Add(symbol);
                        added.Add(symbol);
                    }
                }

                if(this.symbols.Count > MaxSymbols)
                {
                    throw new ArgumentException($"At most {MaxSymbols} symbols may be subscribed at once.", nameof(newSymbols));
                }
            }

            return added;
        }

        private async Task OpenAndSubscribeAsync(CancellationToken ct)
        {
            IWebSocketConnection newConnection = this.connectionFactory();
            try
            {
                await newConnection.ConnectAsync(new Uri(this.options.BaseUri, this.feed + "?api_token=" + Uri.EscapeDataString(this.apiToken)), ct).ConfigureAwait(false);
            }
            catch(Exception)
            {
                newConnection.Dispose();
                throw;
            }

            this.connection = newConnection;

            List<string> current;
            lock(this.symbolsLock)
            {
                current = new List<string>(this.symbols);
            }

            if(current.Count > 0)
            {
                await this.SendActionAsync("subscribe", current, ct).ConfigureAwait(false);
            }
        }

        private async Task SendActionAsync(string action, IReadOnlyList<string> actionSymbols, CancellationToken ct)
        {
            IWebSocketConnection target = this.connection;
            if(target == null)
            {
                return;
            }

            string payload = JsonSerializer.Serialize(
                new SubscriptionMessage { Action = action, Symbols = string.Join(",", actionSymbols) });

            await this.sendLock.WaitAsync(ct).ConfigureAwait(false);
            try
            {
                await target.SendTextAsync(payload, ct).ConfigureAwait(false);
            }
            catch(Exception ex) when(IsTransient(ex))
            {
                // The socket dropped under us (e.g. a concurrent reconnect). The symbol set is already
                // recorded, so the next connect replays the subscription — nothing more to do here.
            }
            finally
            {
                this.sendLock.Release();
            }
        }

        private void DisposeConnection()
        {
            IWebSocketConnection current = this.connection;
            this.connection = null;
            current?.Dispose();
        }
    }
}
