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
        // Options API (legacy)
        // ================================================================

        /// <summary>
        /// Returns the option chain for a symbol from the legacy options endpoint: one entry per
        /// expiration date, each carrying the call and put contracts with pricing and greeks.
        /// </summary>
        /// <param name="symbol">The underlying EODHD symbol (e.g. <c>"AAPL.US"</c>).</param>
        /// <param name="from">Optional inclusive start of the expiration-date range.</param>
        /// <param name="to">Optional inclusive end of the expiration-date range.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>The option chain.</returns>
        public Task<OptionsChain> GetOptionsChainAsync(string symbol, DateTime? from = null, DateTime? to = null, CancellationToken ct = default)
        {
            if(string.IsNullOrWhiteSpace(symbol))
            {
                throw new ArgumentNullException(nameof(symbol));
            }

            return this.GetJsonAsync<OptionsChain>(
                "options/" + Uri.EscapeDataString(symbol),
                ct,
                ("from", FormatDate(from)),
                ("to", FormatDate(to)));
        }

        /// <summary>
        /// Returns the option chain for a symbol from the legacy options endpoint.
        /// </summary>
        /// <param name="symbol">The underlying EODHD symbol (e.g. <c>"AAPL.US"</c>).</param>
        /// <param name="from">Optional inclusive start of the expiration-date range.</param>
        /// <param name="to">Optional inclusive end of the expiration-date range.</param>
        /// <returns>The option chain.</returns>
        public OptionsChain GetOptionsChain(string symbol, DateTime? from = null, DateTime? to = null)
        {
            return this.GetOptionsChainAsync(symbol, from, to).GetAwaiter().GetResult();
        }

        // ================================================================
        // Options API (marketplace: end-of-day prices and contracts)
        // ================================================================

        /// <summary>
        /// Returns end-of-day option prices from the marketplace options API. This endpoint requires the
        /// separate marketplace options subscription.
        /// </summary>
        /// <param name="underlyingSymbol">The underlying symbol (e.g. <c>"AAPL"</c>).</param>
        /// <param name="optionType">Optional option type filter (<c>"call"</c> or <c>"put"</c>).</param>
        /// <param name="strikeFrom">Optional inclusive minimum strike.</param>
        /// <param name="strikeTo">Optional inclusive maximum strike.</param>
        /// <param name="expirationFrom">Optional inclusive start of the expiration-date range.</param>
        /// <param name="expirationTo">Optional inclusive end of the expiration-date range.</param>
        /// <param name="tradeDateFrom">Optional inclusive start of the trade-date range.</param>
        /// <param name="tradeDateTo">Optional inclusive end of the trade-date range.</param>
        /// <param name="sort">Optional sort field; prefix with <c>-</c> for descending (e.g. <c>"-exp_date"</c>).</param>
        /// <param name="offset">Optional pagination offset.</param>
        /// <param name="limit">Optional page size.</param>
        /// <param name="extraParameters">Optional additional query parameters passed through verbatim (e.g. further <c>filter[...]</c> keys); these are appended to, and do not override, the built-in filters.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>The end-of-day option records.</returns>
        public Task<OptionData[]> GetOptionsEodAsync(string underlyingSymbol, string optionType = null, double? strikeFrom = null, double? strikeTo = null, DateTime? expirationFrom = null, DateTime? expirationTo = null, DateTime? tradeDateFrom = null, DateTime? tradeDateTo = null, string sort = null, int? offset = null, int? limit = null, IReadOnlyDictionary<string, string> extraParameters = null, CancellationToken ct = default)
        {
            return this.GetMarketplaceOptionsAsync("mp/unicornbay/options/eod", underlyingSymbol, optionType, strikeFrom, strikeTo, expirationFrom, expirationTo, tradeDateFrom, tradeDateTo, sort, offset, limit, extraParameters, ct);
        }

        /// <summary>
        /// Returns end-of-day option prices from the marketplace options API.
        /// </summary>
        /// <param name="underlyingSymbol">The underlying symbol (e.g. <c>"AAPL"</c>).</param>
        /// <param name="optionType">Optional option type filter (<c>"call"</c> or <c>"put"</c>).</param>
        /// <param name="strikeFrom">Optional inclusive minimum strike.</param>
        /// <param name="strikeTo">Optional inclusive maximum strike.</param>
        /// <param name="expirationFrom">Optional inclusive start of the expiration-date range.</param>
        /// <param name="expirationTo">Optional inclusive end of the expiration-date range.</param>
        /// <param name="tradeDateFrom">Optional inclusive start of the trade-date range.</param>
        /// <param name="tradeDateTo">Optional inclusive end of the trade-date range.</param>
        /// <param name="sort">Optional sort field; prefix with <c>-</c> for descending.</param>
        /// <param name="offset">Optional pagination offset.</param>
        /// <param name="limit">Optional page size.</param>
        /// <param name="extraParameters">Optional additional query parameters passed through verbatim.</param>
        /// <returns>The end-of-day option records.</returns>
        public OptionData[] GetOptionsEod(string underlyingSymbol, string optionType = null, double? strikeFrom = null, double? strikeTo = null, DateTime? expirationFrom = null, DateTime? expirationTo = null, DateTime? tradeDateFrom = null, DateTime? tradeDateTo = null, string sort = null, int? offset = null, int? limit = null, IReadOnlyDictionary<string, string> extraParameters = null)
        {
            return this.GetOptionsEodAsync(underlyingSymbol, optionType, strikeFrom, strikeTo, expirationFrom, expirationTo, tradeDateFrom, tradeDateTo, sort, offset, limit, extraParameters).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Returns the latest option-contract snapshots from the marketplace options API. This endpoint
        /// requires the separate marketplace options subscription.
        /// </summary>
        /// <param name="underlyingSymbol">The underlying symbol (e.g. <c>"AAPL"</c>).</param>
        /// <param name="optionType">Optional option type filter (<c>"call"</c> or <c>"put"</c>).</param>
        /// <param name="strikeFrom">Optional inclusive minimum strike.</param>
        /// <param name="strikeTo">Optional inclusive maximum strike.</param>
        /// <param name="expirationFrom">Optional inclusive start of the expiration-date range.</param>
        /// <param name="expirationTo">Optional inclusive end of the expiration-date range.</param>
        /// <param name="sort">Optional sort field; prefix with <c>-</c> for descending (e.g. <c>"-exp_date"</c>).</param>
        /// <param name="offset">Optional pagination offset.</param>
        /// <param name="limit">Optional page size.</param>
        /// <param name="extraParameters">Optional additional query parameters passed through verbatim; these are appended to, and do not override, the built-in filters.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>The option-contract records.</returns>
        public Task<OptionData[]> GetOptionContractsAsync(string underlyingSymbol, string optionType = null, double? strikeFrom = null, double? strikeTo = null, DateTime? expirationFrom = null, DateTime? expirationTo = null, string sort = null, int? offset = null, int? limit = null, IReadOnlyDictionary<string, string> extraParameters = null, CancellationToken ct = default)
        {
            return this.GetMarketplaceOptionsAsync("mp/unicornbay/options/contracts", underlyingSymbol, optionType, strikeFrom, strikeTo, expirationFrom, expirationTo, null, null, sort, offset, limit, extraParameters, ct);
        }

        /// <summary>
        /// Returns the latest option-contract snapshots from the marketplace options API.
        /// </summary>
        /// <param name="underlyingSymbol">The underlying symbol (e.g. <c>"AAPL"</c>).</param>
        /// <param name="optionType">Optional option type filter (<c>"call"</c> or <c>"put"</c>).</param>
        /// <param name="strikeFrom">Optional inclusive minimum strike.</param>
        /// <param name="strikeTo">Optional inclusive maximum strike.</param>
        /// <param name="expirationFrom">Optional inclusive start of the expiration-date range.</param>
        /// <param name="expirationTo">Optional inclusive end of the expiration-date range.</param>
        /// <param name="sort">Optional sort field; prefix with <c>-</c> for descending.</param>
        /// <param name="offset">Optional pagination offset.</param>
        /// <param name="limit">Optional page size.</param>
        /// <param name="extraParameters">Optional additional query parameters passed through verbatim.</param>
        /// <returns>The option-contract records.</returns>
        public OptionData[] GetOptionContracts(string underlyingSymbol, string optionType = null, double? strikeFrom = null, double? strikeTo = null, DateTime? expirationFrom = null, DateTime? expirationTo = null, string sort = null, int? offset = null, int? limit = null, IReadOnlyDictionary<string, string> extraParameters = null)
        {
            return this.GetOptionContractsAsync(underlyingSymbol, optionType, strikeFrom, strikeTo, expirationFrom, expirationTo, sort, offset, limit, extraParameters).GetAwaiter().GetResult();
        }

        // ================================================================
        // Options — Underlying Symbols (marketplace)
        // ================================================================

        /// <summary>
        /// Returns the list of underlying symbols that have options, from the marketplace options API.
        /// This endpoint requires the separate marketplace options subscription.
        /// </summary>
        /// <param name="offset">Optional pagination offset.</param>
        /// <param name="limit">Optional page size.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>The underlying symbols.</returns>
        public async Task<string[]> GetOptionUnderlyingSymbolsAsync(int? offset = null, int? limit = null, CancellationToken ct = default)
        {
            DataEnvelope<string> response = await this.GetJsonAsync<DataEnvelope<string>>(
                "mp/unicornbay/options/underlying-symbols",
                ct,
                ("page[offset]", FormatInt(offset)),
                ("page[limit]", FormatInt(limit))).ConfigureAwait(false);

            return response?.Data ?? Array.Empty<string>();
        }

        /// <summary>
        /// Returns the list of underlying symbols that have options, from the marketplace options API.
        /// </summary>
        /// <param name="offset">Optional pagination offset.</param>
        /// <param name="limit">Optional page size.</param>
        /// <returns>The underlying symbols.</returns>
        public string[] GetOptionUnderlyingSymbols(int? offset = null, int? limit = null)
        {
            return this.GetOptionUnderlyingSymbolsAsync(offset, limit).GetAwaiter().GetResult();
        }

        private async Task<OptionData[]> GetMarketplaceOptionsAsync(string path, string underlyingSymbol, string optionType, double? strikeFrom, double? strikeTo, DateTime? expirationFrom, DateTime? expirationTo, DateTime? tradeDateFrom, DateTime? tradeDateTo, string sort, int? offset, int? limit, IReadOnlyDictionary<string, string> extraParameters, CancellationToken ct)
        {
            if(string.IsNullOrWhiteSpace(underlyingSymbol))
            {
                throw new ArgumentNullException(nameof(underlyingSymbol));
            }

            List<(string Name, string Value)> query = new List<(string Name, string Value)>
            {
                ("filter[underlying_symbol]", underlyingSymbol),
                ("filter[type]", optionType),
                ("filter[strike_from]", FormatDouble(strikeFrom)),
                ("filter[strike_to]", FormatDouble(strikeTo)),
                ("filter[exp_date_from]", FormatDate(expirationFrom)),
                ("filter[exp_date_to]", FormatDate(expirationTo)),
                ("filter[tradetime_from]", FormatDate(tradeDateFrom)),
                ("filter[tradetime_to]", FormatDate(tradeDateTo)),
                ("sort", sort),
                ("page[offset]", FormatInt(offset)),
                ("page[limit]", FormatInt(limit)),
            };

            if(extraParameters != null)
            {
                foreach(KeyValuePair<string, string> parameter in extraParameters)
                {
                    query.Add((parameter.Key, parameter.Value));
                }
            }

            OptionsApiResponse response = await this.GetJsonAsync<OptionsApiResponse>(path, ct, query.ToArray()).ConfigureAwait(false);

            if(response?.Data == null)
            {
                return Array.Empty<OptionData>();
            }

            List<OptionData> result = new List<OptionData>(response.Data.Length);
            foreach(OptionsApiResource resource in response.Data)
            {
                if(resource?.Attributes != null)
                {
                    result.Add(resource.Attributes);
                }
            }

            return result.ToArray();
        }
    }
}
