using System;

namespace EODHD.CSharpApiClient.Live
{
    /// <summary>
    /// Connection and resilience settings for <see cref="EodhdWebSocketClient{TMessage}"/>.
    /// </summary>
    public sealed class EodhdWebSocketOptions
    {
        /// <summary>
        /// Gets or sets the base URI of the live feed. Defaults to
        /// <c>wss://ws.eodhistoricaldata.com/ws/</c>; the feed name is appended to it.
        /// </summary>
        public Uri BaseUri { get; set; } = new Uri("wss://ws.eodhistoricaldata.com/ws/");

        /// <summary>
        /// Gets or sets whether the client reconnects automatically (with exponential backoff and a replay
        /// of the active subscriptions) when the connection drops. Defaults to <see langword="true"/>.
        /// </summary>
        public bool AutoReconnect { get; set; } = true;

        /// <summary>
        /// Gets or sets the maximum number of consecutive reconnect attempts before the message stream
        /// ends. Defaults to <c>5</c>.
        /// </summary>
        public int MaxReconnectAttempts { get; set; } = 5;

        /// <summary>
        /// Gets or sets the base delay for reconnect backoff; attempt <c>n</c> waits
        /// <c>BaseDelay * 2^(n-1)</c>, capped at <see cref="MaxReconnectDelay"/>. Defaults to one second.
        /// </summary>
        public TimeSpan ReconnectBaseDelay { get; set; } = TimeSpan.FromSeconds(1);

        /// <summary>
        /// Gets or sets the upper bound on a single reconnect backoff delay. Defaults to 30 seconds.
        /// </summary>
        public TimeSpan MaxReconnectDelay { get; set; } = TimeSpan.FromSeconds(30);
    }
}
