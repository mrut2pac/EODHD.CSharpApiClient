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
    }
}
