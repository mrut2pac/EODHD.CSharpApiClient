using System;

namespace EODHD.CSharpApiClient.Exceptions
{
    /// <summary>
    /// Thrown when the EODHD API returns a non-success HTTP status code. Carries the HTTP status,
    /// the raw response body, and a strongly-typed <see cref="Code"/> derived from the status, so
    /// callers can branch on a known case instead of comparing status numbers or matching text.
    /// </summary>
    public sealed class EodhdHttpException : Exception
    {
        /// <summary>
        /// Gets the HTTP status code returned by the API (e.g. 401, 402, 404, 429, 500).
        /// </summary>
        public int StatusCode { get; }

        /// <summary>
        /// Gets the raw response body, truncated to 512 characters when very long.
        /// </summary>
        public string ResponseBody { get; }

        /// <summary>
        /// Gets the strongly-typed classification of <see cref="StatusCode"/>.
        /// <see cref="EodhdErrorCase.Unknown"/> when the status is not one this client classifies.
        /// </summary>
        public EodhdErrorCase Code { get; }

        /// <summary>
        /// Initialises a new instance with the given HTTP status and response body.
        /// </summary>
        /// <param name="statusCode">HTTP status code.</param>
        /// <param name="responseBody">Response body text (may be <see langword="null"/>).</param>
        public EodhdHttpException(int statusCode, string responseBody)
            : base(BuildMessage(statusCode, responseBody))
        {
            this.StatusCode = statusCode;
            this.ResponseBody = responseBody ?? string.Empty;
            this.Code = MapStatusCode(statusCode);
        }

        /// <summary>
        /// Builds the exception for the given HTTP status and response body. Every HTTP failure in the
        /// client is routed through this factory.
        /// </summary>
        /// <param name="statusCode">HTTP status code.</param>
        /// <param name="responseBody">Raw response body (may be <see langword="null"/>).</param>
        /// <returns>A populated <see cref="EodhdHttpException"/>.</returns>
        public static EodhdHttpException Create(int statusCode, string responseBody)
        {
            return new EodhdHttpException(statusCode, responseBody ?? string.Empty);
        }

        /// <summary>
        /// Maps an HTTP status code to its <see cref="EodhdErrorCase"/>, returning
        /// <see cref="EodhdErrorCase.Unknown"/> for unclassified statuses.
        /// </summary>
        /// <param name="statusCode">HTTP status code.</param>
        /// <returns>The mapped enum value.</returns>
        public static EodhdErrorCase MapStatusCode(int statusCode)
        {
            switch(statusCode)
            {
                case 400:
                    return EodhdErrorCase.BadRequest;
                case 401:
                    return EodhdErrorCase.Unauthorized;
                case 402:
                    return EodhdErrorCase.PaymentRequired;
                case 403:
                    return EodhdErrorCase.Forbidden;
                case 404:
                    return EodhdErrorCase.NotFound;
                case 429:
                    return EodhdErrorCase.TooManyRequests;
                default:
                    return statusCode >= 500 && statusCode < 600 ? EodhdErrorCase.ServerError : EodhdErrorCase.Unknown;
            }
        }

        private static string BuildMessage(int statusCode, string body)
        {
            string msg = "HTTP " + statusCode + " [" + MapStatusCode(statusCode) + "]";

            if(!string.IsNullOrEmpty(body))
            {
                string truncated = body.Length > 512 ? string.Concat(body.AsSpan(0, 512), "...") : body;
                msg += ": " + truncated;
            }

            return msg;
        }
    }
}
