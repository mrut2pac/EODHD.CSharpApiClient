using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EODHD.CSharpApiClient.Transport
{
    /// <summary>
    /// Default <see cref="IWebSocketConnection"/> backed by <see cref="ClientWebSocket"/>. Not reusable
    /// across reconnects: dispose and create a new instance to reconnect.
    /// </summary>
    public sealed class DefaultWebSocketConnection : IWebSocketConnection
    {
        private const int ReceiveBufferSize = 8192;

        private readonly ClientWebSocket socket = new ClientWebSocket();

        /// <inheritdoc />
        public Task ConnectAsync(Uri uri, CancellationToken cancellationToken)
        {
            return this.socket.ConnectAsync(uri, cancellationToken);
        }

        /// <inheritdoc />
        public Task SendTextAsync(string message, CancellationToken cancellationToken)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(message);
            return this.socket.SendAsync(new ArraySegment<byte>(bytes), WebSocketMessageType.Text, true, cancellationToken);
        }

        /// <inheritdoc />
        public async Task<string> ReceiveTextAsync(CancellationToken cancellationToken)
        {
            byte[] buffer = new byte[ReceiveBufferSize];
            StringBuilder builder = new StringBuilder();

            while(true)
            {
                WebSocketReceiveResult result = await this.socket.ReceiveAsync(new ArraySegment<byte>(buffer), cancellationToken).ConfigureAwait(false);

                if(result.MessageType == WebSocketMessageType.Close)
                {
                    return null;
                }

                builder.Append(Encoding.UTF8.GetString(buffer, 0, result.Count));

                if(result.EndOfMessage)
                {
                    return builder.ToString();
                }
            }
        }

        /// <inheritdoc />
        public void Dispose()
        {
            this.socket.Dispose();
        }
    }
}
