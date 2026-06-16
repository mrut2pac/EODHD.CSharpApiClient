using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

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
    public sealed partial class EodhdClient : IDisposable
    {
        private const string DateFormat = "yyyy-MM-dd";

        private readonly EodhdClientOptions options;
        private readonly IHttpTransport transport;
        private readonly RequestRateLimiter rateLimiter;

        // Property names are matched case-insensitively so EODHD's camelCase wire fields bind to the
        // PascalCase model properties even where a [JsonPropertyName] is not declared. EODHD also
        // string-encodes some numeric fields, so numbers are allowed to be read from JSON strings —
        // this is set once here rather than with a per-property attribute on every model.
        private static readonly JsonSerializerOptions DeserializeOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            NumberHandling = JsonNumberHandling.AllowReadingFromString,
        };

        // The screener "filters" value is a JSON array containing comparison operators (> and <). The
        // default encoder rewrites those operators to their unicode escape sequences inside the JSON,
        // which the API does not accept, so filter triples are serialized with the relaxed encoder.
        private static readonly JsonSerializerOptions FilterSerializeOptions = new JsonSerializerOptions
        {
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
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

            if(this.options.MaxRequestsPerMinute.HasValue && this.options.MaxRequestsPerMinute.Value > 0)
            {
                this.rateLimiter = new RequestRateLimiter(this.options.MaxRequestsPerMinute.Value);
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

        /// <summary>
        /// Releases the underlying HTTP transport (and its <see cref="HttpClient"/>) and the rate limiter.
        /// </summary>
        public void Dispose()
        {
            this.rateLimiter?.Dispose();
            this.transport.Dispose();
        }

        private static string FormatDate(DateTime? date)
        {
            return date?.ToString(DateFormat, CultureInfo.InvariantCulture);
        }

        private static string FormatInt(int? value)
        {
            return value?.ToString(CultureInfo.InvariantCulture);
        }

        private static string FormatDouble(double? value)
        {
            return value?.ToString(CultureInfo.InvariantCulture);
        }

        private static string FormatUnix(DateTime? date)
        {
            if(!date.HasValue)
            {
                return null;
            }

            DateTime utc = DateTime.SpecifyKind(date.Value, DateTimeKind.Utc);
            return new DateTimeOffset(utc).ToUnixTimeSeconds().ToString(CultureInfo.InvariantCulture);
        }

        private static string JoinSymbols(IReadOnlyList<string> symbols)
        {
            return symbols != null && symbols.Count > 0 ? string.Join(",", symbols) : null;
        }

        /// <summary>
        /// Issues a GET against <paramref name="path"/> (relative to the base URI) with the supplied query
        /// parameters (the <c>api_token</c> and <c>fmt=json</c> parameters are appended automatically; any
        /// query parameter with a <see langword="null"/> value is omitted) and deserializes the JSON
        /// response body into <typeparamref name="T"/>.
        /// </summary>
        private async Task<T> GetJsonAsync<T>(string path, CancellationToken ct, params (string Name, string Value)[] query)
        {
            if(this.rateLimiter != null)
            {
                await this.rateLimiter.GateRequestAsync().ConfigureAwait(false);
            }

            Uri uri = this.BuildRequestUri(path, query);

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
        /// Issues a GET against an absolute <paramref name="uri"/> and returns the raw response bytes.
        /// Unlike <see cref="GetJsonAsync{T}"/> this neither appends the API token / <c>fmt</c> parameters
        /// nor deserializes the body; it is used for binary resources such as logo images.
        /// </summary>
        private async Task<byte[]> GetBytesAsync(Uri uri, CancellationToken ct)
        {
            if(this.rateLimiter != null)
            {
                await this.rateLimiter.GateRequestAsync().ConfigureAwait(false);
            }

            using(HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, uri))
            using(HttpResponseMessage response = await this.transport.SendAsync(request, ct).ConfigureAwait(false))
            {
                if(!response.IsSuccessStatusCode)
                {
                    string body = response.Content != null
                        ? await response.Content.ReadAsStringAsync().ConfigureAwait(false)
                        : string.Empty;
                    throw EodhdHttpException.Create((int)response.StatusCode, body);
                }

                return response.Content != null
                    ? await response.Content.ReadAsByteArrayAsync().ConfigureAwait(false)
                    : Array.Empty<byte>();
            }
        }

        /// <summary>
        /// Builds the absolute request URI from a relative path and query parameters, appending the
        /// <c>api_token</c> and <c>fmt=json</c> parameters and skipping any parameter with a null value.
        /// </summary>
        private Uri BuildRequestUri(string path, (string Name, string Value)[] query)
        {
            StringBuilder builder = new StringBuilder(new Uri(this.options.BaseUri, path).ToString());
            builder.Append("?api_token=").Append(Uri.EscapeDataString(this.options.ApiToken)).Append("&fmt=json");

            if(query != null)
            {
                foreach((string Name, string Value) parameter in query)
                {
                    if(parameter.Value != null)
                    {
                        builder.Append('&').Append(parameter.Name).Append('=').Append(Uri.EscapeDataString(parameter.Value));
                    }
                }
            }

            return new Uri(builder.ToString());
        }
    }
}
