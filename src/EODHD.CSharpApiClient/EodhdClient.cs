using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

using EODHD.CSharpApiClient.DataModel;
using EODHD.CSharpApiClient.Exceptions;
using EODHD.CSharpApiClient.Transport;

namespace EODHD.CSharpApiClient
{
    /// <summary>
    /// Async-first HTTP client for the EOD Historical Data (EODHD) REST API. Every endpoint is exposed
    /// as a <see cref="Task"/>-returning method with a trailing <see cref="CancellationToken"/>; a
    /// synchronous convenience wrapper alongside each one delegates via <c>GetAwaiter().GetResult()</c>.
    /// The API token is supplied at construction and sent as the <c>api_token</c> query parameter.
    /// </summary>
    public sealed class EodhdClient : IDisposable
    {
        private readonly EodhdClientOptions options;
        private readonly IHttpTransport transport;

        // Property names are matched case-insensitively so EODHD's camelCase wire fields bind to the
        // PascalCase model properties even where a [JsonPropertyName] is not declared.
        private static readonly JsonSerializerOptions DeserializeOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
        };

        /// <summary>
        /// Initialises a new client with the supplied API token and default options.
        /// </summary>
        /// <param name="apiToken">The EODHD API token.</param>
        public EodhdClient(string apiToken)
            : this(new EodhdClientOptions { ApiToken = apiToken })
        {
        }

        /// <summary>
        /// Initialises a new client using the supplied options. A default <see cref="HttpClient"/>-backed
        /// transport is created unless a custom <paramref name="transport"/> is provided (useful for tests).
        /// </summary>
        /// <param name="options">Connection and authentication settings.</param>
        /// <param name="transport">Optional custom HTTP transport; leave <see langword="null"/> to use the built-in client.</param>
        public EodhdClient(EodhdClientOptions options, IHttpTransport transport = null)
        {
            this.options = options ?? throw new ArgumentNullException(nameof(options));

            if(string.IsNullOrWhiteSpace(this.options.ApiToken))
            {
                throw new ArgumentException("ApiToken must be set.", nameof(options));
            }

            if(this.options.BaseUri == null)
            {
                throw new ArgumentException("BaseUri must be set.", nameof(options));
            }

            if(transport != null)
            {
                this.transport = transport;
            }
            else
            {
                HttpClient httpClient = new HttpClient
                {
                    Timeout = this.options.Timeout,
                };
                httpClient.DefaultRequestHeaders.UserAgent.ParseAdd(this.options.UserAgent ?? "EODHD.CSharpApiClient/1.0");
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                this.transport = new DefaultHttpTransport(httpClient);
            }
        }

        // ================================================================
        // User API
        // ================================================================

        /// <summary>
        /// Returns account and usage information for the authenticated API token.
        /// </summary>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>The account's <see cref="UserInfo"/>.</returns>
        public Task<UserInfo> GetUserInfoAsync(CancellationToken ct = default)
        {
            return this.GetJsonAsync<UserInfo>("user", ct);
        }

        /// <summary>
        /// Returns account and usage information for the authenticated API token.
        /// </summary>
        /// <returns>The account's <see cref="UserInfo"/>.</returns>
        public UserInfo GetUserInfo()
        {
            return this.GetUserInfoAsync().GetAwaiter().GetResult();
        }

        /// <summary>
        /// Releases the underlying HTTP transport (and its <see cref="HttpClient"/>).
        /// </summary>
        public void Dispose()
        {
            this.transport.Dispose();
        }

        /// <summary>
        /// Issues a GET against <paramref name="pathAndQuery"/> (relative to the base URI, excluding the
        /// <c>api_token</c> and <c>fmt</c> parameters, which are appended automatically) and deserializes
        /// the JSON response body into <typeparamref name="T"/>.
        /// </summary>
        private async Task<T> GetJsonAsync<T>(string pathAndQuery, CancellationToken ct)
        {
            Uri uri = this.BuildRequestUri(pathAndQuery);

            using(HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, uri))
            using(HttpResponseMessage response = await this.transport.SendAsync(request, ct).ConfigureAwait(false))
            {
                string body = response.Content != null
                    ? await response.Content.ReadAsStringAsync().ConfigureAwait(false)
                    : string.Empty;

                if(!response.IsSuccessStatusCode)
                {
                    throw EodhdHttpException.Create((int)response.StatusCode, body);
                }

                return JsonSerializer.Deserialize<T>(body, DeserializeOptions);
            }
        }

        /// <summary>
        /// Builds the absolute request URI from a relative path/query, appending the <c>api_token</c> and
        /// <c>fmt=json</c> parameters with the correct separator.
        /// </summary>
        private Uri BuildRequestUri(string pathAndQuery)
        {
            Uri uri = new Uri(this.options.BaseUri, pathAndQuery);
            string separator = string.IsNullOrEmpty(uri.Query) ? "?" : "&";
            string full = uri + separator + "api_token=" + Uri.EscapeDataString(this.options.ApiToken) + "&fmt=json";
            return new Uri(full);
        }
    }
}
