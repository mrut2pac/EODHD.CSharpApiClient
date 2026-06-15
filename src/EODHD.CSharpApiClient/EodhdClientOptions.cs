using System;

namespace EODHD.CSharpApiClient
{
    /// <summary>
    /// Configuration options passed to <see cref="EodhdClient"/> at construction time.
    /// </summary>
    public sealed class EodhdClientOptions
    {
        /// <summary>
        /// Gets or sets the API base URL. Defaults to <c>https://eodhd.com/api/</c>.
        /// </summary>
        public Uri BaseUri { get; set; } = new Uri("https://eodhd.com/api/");

        /// <summary>
        /// Gets or sets the EODHD API token, sent as the <c>api_token</c> query parameter. Required.
        /// </summary>
        public string ApiToken { get; set; }

        /// <summary>
        /// Gets or sets the value sent in the <c>User-Agent</c> header.
        /// Defaults to <c>EODHD.CSharpApiClient/1.0</c>.
        /// </summary>
        public string UserAgent { get; set; } = "EODHD.CSharpApiClient/1.0";

        /// <summary>
        /// Gets or sets the per-request HTTP timeout. Default: 5 minutes (suitable for large bulk fetches).
        /// </summary>
        public TimeSpan Timeout { get; set; } = TimeSpan.FromMinutes(5);
    }
}
