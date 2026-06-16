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

using EODHD.CSharpApiClient.DataModel;
using EODHD.CSharpApiClient.DataModel.Bonds;
using EODHD.CSharpApiClient.DataModel.BulkFundamental;
using EODHD.CSharpApiClient.DataModel.Cboe;
using EODHD.CSharpApiClient.DataModel.Commodities;
using EODHD.CSharpApiClient.DataModel.EarningsTrends;
using EODHD.CSharpApiClient.DataModel.EconomicEvents;
using EODHD.CSharpApiClient.DataModel.ExchangeInfo;
using EODHD.CSharpApiClient.DataModel.Fundamental;
using EODHD.CSharpApiClient.DataModel.IdMappings;
using EODHD.CSharpApiClient.DataModel.InsiderTransactions;
using EODHD.CSharpApiClient.DataModel.Macro;
using EODHD.CSharpApiClient.DataModel.MarketCap;
using EODHD.CSharpApiClient.DataModel.News;
using EODHD.CSharpApiClient.DataModel.Options;
using EODHD.CSharpApiClient.DataModel.Quotes;
using EODHD.CSharpApiClient.DataModel.Screener;
using EODHD.CSharpApiClient.DataModel.Sentiment;
using EODHD.CSharpApiClient.DataModel.TechnicalIndicators;
using EODHD.CSharpApiClient.DataModel.Treasury;
using EODHD.CSharpApiClient.DataModel.UpcomingDividends;
using EODHD.CSharpApiClient.DataModel.UpcomingEarnings;
using EODHD.CSharpApiClient.DataModel.UpcomingIpos;
using EODHD.CSharpApiClient.DataModel.UpcomingSplits;
using EODHD.CSharpApiClient.Exceptions;
using EODHD.CSharpApiClient.Transport;

namespace EODHD.CSharpApiClient
{
    public sealed partial class EodhdClient
    {
        // ================================================================
        // Ticker Logos
        // ================================================================

        // EODHD does not derive a logo path from the symbol in a predictable way (casing differs by
        // exchange — "US" logos are lower-cased, others are not), so the authoritative source is the
        // fundamentals General.LogoURL field. The logos themselves are public PNGs served from the site
        // root (no API token), so they are fetched with the raw-bytes transport rather than GetJsonAsync.
        private const string NoLogoSentinel = "NA";

        /// <summary>
        /// Resolves the absolute URL of a symbol's logo via its fundamentals <c>General.LogoURL</c>.
        /// </summary>
        /// <param name="symbol">The full EODHD symbol (e.g. <c>"AAPL.US"</c>).</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>The absolute logo URL, or <c>null</c> if the symbol has no logo.</returns>
        public async Task<string> GetLogoUrlAsync(string symbol, CancellationToken ct = default)
        {
            if(string.IsNullOrWhiteSpace(symbol))
            {
                throw new ArgumentNullException(nameof(symbol));
            }

            string path = await this.GetJsonAsync<string>(
                "fundamentals/" + Uri.EscapeDataString(symbol),
                ct,
                ("filter", "General::LogoURL")).ConfigureAwait(false);

            if(string.IsNullOrWhiteSpace(path) || string.Equals(path, NoLogoSentinel, StringComparison.Ordinal))
            {
                return null;
            }

            return new Uri(this.options.BaseUri, path).ToString();
        }

        /// <summary>
        /// Resolves the absolute URL of a symbol's logo via its fundamentals <c>General.LogoURL</c>.
        /// </summary>
        /// <param name="symbol">The full EODHD symbol (e.g. <c>"AAPL.US"</c>).</param>
        /// <returns>The absolute logo URL, or <c>null</c> if the symbol has no logo.</returns>
        public string GetLogoUrl(string symbol)
        {
            return this.GetLogoUrlAsync(symbol).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Downloads the raw image bytes of a symbol's logo. Resolves the logo URL via
        /// <see cref="GetLogoUrlAsync"/> and then fetches it.
        /// </summary>
        /// <param name="symbol">The full EODHD symbol (e.g. <c>"AAPL.US"</c>).</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>The logo image bytes (typically PNG), or <c>null</c> if the symbol has no logo.</returns>
        public async Task<byte[]> GetLogoBytesAsync(string symbol, CancellationToken ct = default)
        {
            string url = await this.GetLogoUrlAsync(symbol, ct).ConfigureAwait(false);
            if(url == null)
            {
                return null;
            }

            return await this.DownloadLogoBytesAsync(url, ct).ConfigureAwait(false);
        }

        /// <summary>
        /// Downloads the raw image bytes of a symbol's logo.
        /// </summary>
        /// <param name="symbol">The full EODHD symbol (e.g. <c>"AAPL.US"</c>).</param>
        /// <returns>The logo image bytes (typically PNG), or <c>null</c> if the symbol has no logo.</returns>
        public byte[] GetLogoBytes(string symbol)
        {
            return this.GetLogoBytesAsync(symbol).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Downloads the raw image bytes from a logo URL (for example a <c>General.LogoURL</c> obtained from
        /// a previously fetched fundamentals record, avoiding a second fundamentals call). The URL may be
        /// absolute or a site-root-relative path (e.g. <c>"/img/logos/US/aapl.png"</c>).
        /// </summary>
        /// <param name="logoUrl">The logo URL or site-relative path.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>The logo image bytes (typically PNG).</returns>
        public Task<byte[]> DownloadLogoBytesAsync(string logoUrl, CancellationToken ct = default)
        {
            if(string.IsNullOrWhiteSpace(logoUrl))
            {
                throw new ArgumentNullException(nameof(logoUrl));
            }

            return this.GetBytesAsync(new Uri(this.options.BaseUri, logoUrl), ct);
        }

        /// <summary>
        /// Downloads the raw image bytes from a logo URL (for example a <c>General.LogoURL</c> obtained from
        /// a previously fetched fundamentals record). The URL may be absolute or a site-root-relative path.
        /// </summary>
        /// <param name="logoUrl">The logo URL or site-relative path.</param>
        /// <returns>The logo image bytes (typically PNG).</returns>
        public byte[] DownloadLogoBytes(string logoUrl)
        {
            return this.DownloadLogoBytesAsync(logoUrl).GetAwaiter().GetResult();
        }
    }
}
