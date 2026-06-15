using System;

using EODHD.CSharpApiClient.Exceptions;

using Xunit;

namespace EODHD.CSharpApiClient.IntegrationTests
{
    /// <summary>
    /// Base class for all integration tests. Reads the API token from the <c>EODHD_API_KEY</c>
    /// environment variable. Individual tests must call <see cref="SkipIfNoApiKey"/> at their start so
    /// they are cleanly skipped in environments without credentials.
    /// </summary>
    public abstract class IntegrationTestBase : IDisposable
    {
        protected readonly string ApiToken;

        protected IntegrationTestBase()
        {
            this.ApiToken = Environment.GetEnvironmentVariable("EODHD_API_KEY") ?? string.Empty;
        }

        /// <summary>
        /// Skips the test when <c>EODHD_API_KEY</c> is not set. Call at the very start of every
        /// <c>[SkippableFact]</c> test body.
        /// </summary>
        protected void SkipIfNoApiKey()
        {
            Skip.If(string.IsNullOrWhiteSpace(this.ApiToken), "EODHD_API_KEY environment variable not set.");
        }

        /// <summary>
        /// Skips the test when the exception indicates the endpoint or data is not available on the
        /// current subscription tier (HTTP 402/403), rather than failing. Call in a catch block around
        /// calls that may hit subscription-gated endpoints.
        /// </summary>
        /// <param name="ex">The exception thrown by the client call.</param>
        protected static void SkipIfNoLicense(EodhdHttpException ex)
        {
            Skip.If(
                ex.Code == EodhdErrorCase.PaymentRequired || ex.Code == EodhdErrorCase.Forbidden,
                "Skipped — endpoint/data not available on this subscription: " + ex.Message);
        }

        /// <summary>
        /// Creates an <see cref="EodhdClient"/> bound to the configured API token.
        /// </summary>
        /// <returns>A new client instance.</returns>
        protected EodhdClient CreateClient()
        {
            return new EodhdClient(new EodhdClientOptions { ApiToken = this.ApiToken });
        }

        /// <summary>
        /// Creates an <see cref="EodhdClient"/> bound to EODHD's public <c>demo</c> token. Some endpoints
        /// (e.g. the marketplace options API) require a separate add-on the main key may not carry, but
        /// the documented <c>demo</c> token serves a fixed set of symbols (notably <c>AAPL</c>) for them,
        /// so the integration test can still exercise the endpoint live.
        /// </summary>
        /// <returns>A new client instance bound to the demo token.</returns>
        protected static EodhdClient CreateDemoClient()
        {
            return new EodhdClient(new EodhdClientOptions { ApiToken = "demo" });
        }

        public void Dispose()
        {
        }
    }
}
