using EODHD.CSharpApiClient.DataModel.Live;

namespace EODHD.CSharpApiClient.Live
{
    /// <summary>
    /// Factory methods for the typed live (WebSocket) clients, one per EODHD feed.
    /// </summary>
    public static class EodhdWebSocketClient
    {
        /// <summary>Creates a client for the real-time forex feed.</summary>
        /// <param name="apiToken">The EODHD API token.</param>
        /// <param name="options">Optional connection settings.</param>
        /// <returns>A live forex-quote client.</returns>
        public static EodhdWebSocketClient<ForexQuote> Forex(string apiToken, EodhdWebSocketOptions options = null)
        {
            return new EodhdWebSocketClient<ForexQuote>(apiToken, "forex", options);
        }

        /// <summary>Creates a client for the real-time crypto feed.</summary>
        /// <param name="apiToken">The EODHD API token.</param>
        /// <param name="options">Optional connection settings.</param>
        /// <returns>A live crypto-trade client.</returns>
        public static EodhdWebSocketClient<CryptoTrade> Crypto(string apiToken, EodhdWebSocketOptions options = null)
        {
            return new EodhdWebSocketClient<CryptoTrade>(apiToken, "crypto", options);
        }

        /// <summary>Creates a client for the real-time US trades feed (streams during US regular and
        /// extended hours).</summary>
        /// <param name="apiToken">The EODHD API token.</param>
        /// <param name="options">Optional connection settings.</param>
        /// <returns>A live US-trade client.</returns>
        public static EodhdWebSocketClient<UsTrade> UsTrades(string apiToken, EodhdWebSocketOptions options = null)
        {
            return new EodhdWebSocketClient<UsTrade>(apiToken, "us", options);
        }

        /// <summary>Creates a client for the real-time US quotes (top-of-book) feed (streams during US
        /// regular and extended hours).</summary>
        /// <param name="apiToken">The EODHD API token.</param>
        /// <param name="options">Optional connection settings.</param>
        /// <returns>A live US-quote client.</returns>
        public static EodhdWebSocketClient<UsQuote> UsQuotes(string apiToken, EodhdWebSocketOptions options = null)
        {
            return new EodhdWebSocketClient<UsQuote>(apiToken, "us-quote", options);
        }
    }
}
