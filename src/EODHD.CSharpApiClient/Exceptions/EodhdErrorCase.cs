namespace EODHD.CSharpApiClient.Exceptions
{
    /// <summary>
    /// Strongly-typed classification of an EODHD API failure, derived from the HTTP status code.
    /// Lets callers branch on a known case instead of comparing raw status numbers or response text.
    /// The HTTP status is always preserved on <see cref="EodhdHttpException.StatusCode"/>; statuses
    /// this client version does not classify map to <see cref="Unknown"/>.
    /// </summary>
    public enum EodhdErrorCase
    {
        /// <summary>
        /// The status code is not one this client version classifies. Inspect
        /// <see cref="EodhdHttpException.StatusCode"/> and <see cref="EodhdHttpException.ResponseBody"/>.
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// HTTP 400 — the request was malformed (e.g. an invalid parameter value).
        /// </summary>
        BadRequest,

        /// <summary>
        /// HTTP 401 — the API token is missing, invalid, or expired.
        /// </summary>
        Unauthorized,

        /// <summary>
        /// HTTP 402 — payment required: the daily request quota is exhausted or the plan does not
        /// cover this request.
        /// </summary>
        PaymentRequired,

        /// <summary>
        /// HTTP 403 — the token is valid but not entitled to the requested data or endpoint
        /// (subscription tier limitation).
        /// </summary>
        Forbidden,

        /// <summary>
        /// HTTP 404 — the requested symbol, exchange, or endpoint does not exist.
        /// </summary>
        NotFound,

        /// <summary>
        /// HTTP 429 — too many requests: the rate limit has been exceeded.
        /// </summary>
        TooManyRequests,

        /// <summary>
        /// HTTP 5xx — an EODHD server-side error.
        /// </summary>
        ServerError,
    }
}
