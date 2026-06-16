using System;
using System.Threading;
using System.Threading.Tasks;

namespace EODHD.CSharpApiClient.Transport
{
    /// <summary>
    /// Abstraction over a single client WebSocket connection used by the live (streaming) client.
    /// Implement this interface to substitute a deterministic mock connection in unit tests. A single
    /// instance represents one connection lifecycle; reconnection creates a fresh instance.
    /// </summary>
    public interface IWebSocketConnection : IDisposable
    {
        /// <summary>Opens the connection to the supplied endpoint.</summary>
        /// <param name="uri">The WebSocket endpoint URI.</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        Task ConnectAsync(Uri uri, CancellationToken cancellationToken);

        /// <summary>Sends a text message.</summary>
        /// <param name="message">The UTF-8 text payload.</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        Task SendTextAsync(string message, CancellationToken cancellationToken);

        /// <summary>
        /// Receives the next complete text message, or <see langword="null"/> when the remote endpoint
        /// has closed the connection.
        /// </summary>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        /// <returns>The received text message, or <see langword="null"/> on close.</returns>
        Task<string> ReceiveTextAsync(CancellationToken cancellationToken);
    }
}
