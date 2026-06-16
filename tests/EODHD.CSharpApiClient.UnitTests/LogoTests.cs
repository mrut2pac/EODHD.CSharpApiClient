using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using EODHD.CSharpApiClient.Transport;

using Xunit;

namespace EODHD.CSharpApiClient.UnitTests
{
    public class LogoTests
    {
        // Returns queued responses in order and records every request URI, so the two-step logo flow
        // (fundamentals filter -> image download) can be asserted end to end.
        private sealed class SequencedTransport : IHttpTransport
        {
            private readonly Queue<(HttpStatusCode Status, byte[] Body)> responses;

            public SequencedTransport(params (HttpStatusCode Status, byte[] Body)[] responses)
            {
                this.responses = new Queue<(HttpStatusCode, byte[])>(responses);
            }

            public List<Uri> RequestUris { get; } = new List<Uri>();

            public Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                this.RequestUris.Add(request.RequestUri);
                (HttpStatusCode status, byte[] body) = this.responses.Dequeue();
                return Task.FromResult(new HttpResponseMessage(status) { Content = new ByteArrayContent(body ?? Array.Empty<byte>()) });
            }

            public void Dispose()
            {
            }
        }

        private static EodhdClient CreateClient(IHttpTransport transport)
        {
            return new EodhdClient(new EodhdClientOptions { ApiToken = "test" }, transport);
        }

        [Fact]
        public async Task GetLogoUrl_ResolvesRelativePathToAbsolute()
        {
            byte[] body = Encoding.UTF8.GetBytes("\"\\/img\\/logos\\/US\\/aapl.png\"");
            using EodhdClient client = CreateClient(new SequencedTransport((HttpStatusCode.OK, body)));

            string url = await client.GetLogoUrlAsync("AAPL.US");

            Assert.Equal("https://eodhd.com/img/logos/US/aapl.png", url);
        }

        [Fact]
        public async Task GetLogoUrl_NaSentinel_ReturnsNull()
        {
            byte[] body = Encoding.UTF8.GetBytes("\"NA\"");
            using EodhdClient client = CreateClient(new SequencedTransport((HttpStatusCode.OK, body)));

            string url = await client.GetLogoUrlAsync("EURUSD.FOREX");

            Assert.Null(url);
        }

        [Fact]
        public async Task GetLogoBytes_FetchesFundamentalsThenImage()
        {
            byte[] pathJson = Encoding.UTF8.GetBytes("\"\\/img\\/logos\\/US\\/aapl.png\"");
            byte[] png = { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A };
            SequencedTransport transport = new SequencedTransport((HttpStatusCode.OK, pathJson), (HttpStatusCode.OK, png));
            using EodhdClient client = CreateClient(transport);

            byte[] bytes = await client.GetLogoBytesAsync("AAPL.US");

            Assert.Equal(png, bytes);
            Assert.Equal(2, transport.RequestUris.Count);
            Assert.Contains("fundamentals/AAPL.US", transport.RequestUris[0].ToString(), StringComparison.Ordinal);
            // Full URL (not just the path): the logo CDN is tokenless, so guard against a leaked api_token query.
            Assert.Equal("https://eodhd.com/img/logos/US/aapl.png", transport.RequestUris[1].ToString());
        }

        [Fact]
        public async Task GetLogoBytes_NoLogo_ReturnsNullWithoutSecondCall()
        {
            byte[] na = Encoding.UTF8.GetBytes("\"NA\"");
            SequencedTransport transport = new SequencedTransport((HttpStatusCode.OK, na));
            using EodhdClient client = CreateClient(transport);

            byte[] bytes = await client.GetLogoBytesAsync("EURUSD.FOREX");

            Assert.Null(bytes);
            Assert.Single(transport.RequestUris);
        }

        [Fact]
        public async Task DownloadLogoBytes_RelativePath_ResolvesAgainstSiteRoot()
        {
            byte[] png = { 0x89, 0x50, 0x4E, 0x47 };
            SequencedTransport transport = new SequencedTransport((HttpStatusCode.OK, png));
            using EodhdClient client = CreateClient(transport);

            byte[] bytes = await client.DownloadLogoBytesAsync("/img/logos/LSE/TSCO.png");

            Assert.Equal(png, bytes);
            Assert.Equal("https://eodhd.com/img/logos/LSE/TSCO.png", transport.RequestUris[0].ToString());
        }

        [Fact]
        public async Task GetLogoUrl_NullSymbol_Throws()
        {
            using EodhdClient client = CreateClient(new SequencedTransport());

            await Assert.ThrowsAsync<ArgumentNullException>(() => client.GetLogoUrlAsync(null));
        }
    }
}
