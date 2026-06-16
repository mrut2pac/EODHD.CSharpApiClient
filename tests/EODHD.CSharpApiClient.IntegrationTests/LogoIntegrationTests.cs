using System;
using System.Threading.Tasks;

using EODHD.CSharpApiClient.Exceptions;

using Xunit;

namespace EODHD.CSharpApiClient.IntegrationTests
{
    public class LogoIntegrationTests : IntegrationTestBase
    {
        [SkippableFact]
        public async Task GetLogoUrl_Apple_ReturnsAbsoluteUrl()
        {
            this.SkipIfNoApiKey();
            using EodhdClient client = this.CreateClient();

            string url;
            try
            {
                url = await client.GetLogoUrlAsync("AAPL.US");
            }
            catch (EodhdHttpException ex)
            {
                SkipIfNoLicense(ex);
                throw;
            }

            Skip.If(url == null, "No logo configured for the symbol.");
            Assert.StartsWith("https://", url, StringComparison.Ordinal);
            Assert.Contains("/img/logos/", url, StringComparison.Ordinal);
        }

        [SkippableFact]
        public async Task GetLogoBytes_Apple_ReturnsPngImage()
        {
            this.SkipIfNoApiKey();
            using EodhdClient client = this.CreateClient();

            byte[] bytes;
            try
            {
                bytes = await client.GetLogoBytesAsync("AAPL.US");
            }
            catch (EodhdHttpException ex)
            {
                SkipIfNoLicense(ex);
                throw;
            }

            Skip.If(bytes == null, "No logo configured for the symbol.");
            Assert.True(bytes.Length > 8, "Logo payload is too small to be an image.");
            // PNG magic number.
            Assert.Equal(new byte[] { 0x89, 0x50, 0x4E, 0x47 }, new[] { bytes[0], bytes[1], bytes[2], bytes[3] });
        }

        [SkippableFact]
        public async Task GetLogoUrl_NonEquityWithoutLogo_ReturnsNull()
        {
            this.SkipIfNoApiKey();
            using EodhdClient client = this.CreateClient();

            string url;
            try
            {
                url = await client.GetLogoUrlAsync("EURUSD.FOREX");
            }
            catch (EodhdHttpException ex)
            {
                SkipIfNoLicense(ex);
                throw;
            }

            Assert.Null(url);
        }
    }
}
