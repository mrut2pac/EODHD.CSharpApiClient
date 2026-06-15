using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using EODHD.CSharpApiClient.Transport;

namespace EODHD.CSharpApiClient.UnitTests
{
    /// <summary>
    /// Deterministic <see cref="IHttpTransport"/> for unit tests. Returns a preset status and body and
    /// captures the last request URI so tests can assert on the composed query (path, api_token, fmt).
    /// </summary>
    internal sealed class FakeHttpTransport : IHttpTransport
    {
        private readonly HttpStatusCode statusCode;
        private readonly string responseBody;

        public FakeHttpTransport(HttpStatusCode statusCode, string responseBody)
        {
            this.statusCode = statusCode;
            this.responseBody = responseBody;
        }

        public Uri LastRequestUri { get; private set; }

        public Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            this.LastRequestUri = request.RequestUri;

            HttpResponseMessage response = new HttpResponseMessage(this.statusCode)
            {
                Content = new StringContent(this.responseBody ?? string.Empty, Encoding.UTF8, "application/json"),
            };

            return Task.FromResult(response);
        }

        public void Dispose()
        {
        }
    }
}
