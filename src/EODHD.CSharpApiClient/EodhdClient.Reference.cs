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

        // ================================================================
        // Exchanges API
        // ================================================================

        /// <summary>
        /// Returns details (name, operating hours, holidays) for an exchange.
        /// </summary>
        /// <param name="exchangeCode">The EODHD exchange code (e.g. <c>"US"</c>).</param>
        /// <param name="from">Optional inclusive start date for the holidays range.</param>
        /// <param name="to">Optional inclusive end date for the holidays range.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>The exchange details.</returns>
        public Task<ExchangeDetails> GetExchangeDetailsAsync(string exchangeCode, DateTime? from = null, DateTime? to = null, CancellationToken ct = default)
        {
            if(string.IsNullOrWhiteSpace(exchangeCode))
            {
                throw new ArgumentNullException(nameof(exchangeCode));
            }

            return this.GetJsonAsync<ExchangeDetails>(
                "exchange-details/" + Uri.EscapeDataString(exchangeCode),
                ct,
                ("from", FormatDate(from)),
                ("to", FormatDate(to)));
        }

        /// <summary>
        /// Returns details (name, operating hours, holidays) for an exchange.
        /// </summary>
        /// <param name="exchangeCode">The EODHD exchange code (e.g. <c>"US"</c>).</param>
        /// <param name="from">Optional inclusive start date for the holidays range.</param>
        /// <param name="to">Optional inclusive end date for the holidays range.</param>
        /// <returns>The exchange details.</returns>
        public ExchangeDetails GetExchangeDetails(string exchangeCode, DateTime? from = null, DateTime? to = null)
        {
            return this.GetExchangeDetailsAsync(exchangeCode, from, to).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Returns the history of ticker symbol changes across exchanges.
        /// </summary>
        /// <param name="from">Optional inclusive start date.</param>
        /// <param name="to">Optional inclusive end date.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>The symbol-change records.</returns>
        public Task<SymbolChange[]> GetSymbolChangeHistoryAsync(DateTime? from = null, DateTime? to = null, CancellationToken ct = default)
        {
            return this.GetJsonAsync<SymbolChange[]>(
                "symbol-change-history",
                ct,
                ("from", FormatDate(from)),
                ("to", FormatDate(to)));
        }

        /// <summary>
        /// Returns the history of ticker symbol changes across exchanges.
        /// </summary>
        /// <param name="from">Optional inclusive start date.</param>
        /// <param name="to">Optional inclusive end date.</param>
        /// <returns>The symbol-change records.</returns>
        public SymbolChange[] GetSymbolChangeHistory(DateTime? from = null, DateTime? to = null)
        {
            return this.GetSymbolChangeHistoryAsync(from, to).GetAwaiter().GetResult();
        }

        // ================================================================
        // Exchange Symbols API
        // ================================================================

        /// <summary>
        /// Returns the list of symbols traded on an exchange.
        /// </summary>
        /// <param name="exchangeCode">The EODHD exchange code (e.g. <c>"US"</c>).</param>
        /// <param name="delisted">When <see langword="true"/>, returns delisted symbols instead of active ones.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>The exchange symbols.</returns>
        public Task<List<ExchangeSymbol>> GetExchangeSymbolsAsync(string exchangeCode, bool delisted = false, CancellationToken ct = default)
        {
            if(string.IsNullOrWhiteSpace(exchangeCode))
            {
                throw new ArgumentNullException(nameof(exchangeCode));
            }

            return this.GetJsonAsync<List<ExchangeSymbol>>(
                "exchange-symbol-list/" + Uri.EscapeDataString(exchangeCode),
                ct,
                ("delisted", delisted ? "1" : null));
        }

        /// <summary>
        /// Returns the list of symbols traded on an exchange.
        /// </summary>
        /// <param name="exchangeCode">The EODHD exchange code (e.g. <c>"US"</c>).</param>
        /// <param name="delisted">When <see langword="true"/>, returns delisted symbols instead of active ones.</param>
        /// <returns>The exchange symbols.</returns>
        public List<ExchangeSymbol> GetExchangeSymbols(string exchangeCode, bool delisted = false)
        {
            return this.GetExchangeSymbolsAsync(exchangeCode, delisted).GetAwaiter().GetResult();
        }

        // ================================================================
        // Search API
        // ================================================================

        /// <summary>
        /// Searches active tickers by symbol, company name, or ISIN.
        /// </summary>
        /// <param name="query">The search query (ticker, name, or ISIN).</param>
        /// <param name="limit">Optional maximum number of results (default 15, max 500).</param>
        /// <param name="type">Optional asset-type filter (e.g. <c>"stock"</c>, <c>"etf"</c>, <c>"all"</c>).</param>
        /// <param name="exchange">Optional exchange-code filter (e.g. <c>"US"</c>).</param>
        /// <param name="bondsOnly">When <see langword="true"/>, restricts results to bonds.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>The matching instruments.</returns>
        public Task<SearchResult[]> SearchAsync(string query, int? limit = null, string type = null, string exchange = null, bool bondsOnly = false, CancellationToken ct = default)
        {
            if(string.IsNullOrWhiteSpace(query))
            {
                throw new ArgumentNullException(nameof(query));
            }

            return this.GetJsonAsync<SearchResult[]>(
                "search/" + Uri.EscapeDataString(query),
                ct,
                ("limit", FormatInt(limit)),
                ("type", type),
                ("exchange", exchange),
                ("bonds_only", bondsOnly ? "1" : null));
        }

        /// <summary>
        /// Searches active tickers by symbol, company name, or ISIN.
        /// </summary>
        /// <param name="query">The search query (ticker, name, or ISIN).</param>
        /// <param name="limit">Optional maximum number of results (default 15, max 500).</param>
        /// <param name="type">Optional asset-type filter (e.g. <c>"stock"</c>, <c>"etf"</c>, <c>"all"</c>).</param>
        /// <param name="exchange">Optional exchange-code filter (e.g. <c>"US"</c>).</param>
        /// <param name="bondsOnly">When <see langword="true"/>, restricts results to bonds.</param>
        /// <returns>The matching instruments.</returns>
        public SearchResult[] Search(string query, int? limit = null, string type = null, string exchange = null, bool bondsOnly = false)
        {
            return this.SearchAsync(query, limit, type, exchange, bondsOnly).GetAwaiter().GetResult();
        }

        // ================================================================
        // Exchanges List API
        // ================================================================

        /// <summary>
        /// Returns the list of all exchanges (and asset-class venues) supported by EODHD.
        /// </summary>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>The exchanges.</returns>
        public Task<List<Exchange>> GetExchangesListAsync(CancellationToken ct = default)
        {
            return this.GetJsonAsync<List<Exchange>>("exchanges-list/", ct);
        }

        /// <summary>
        /// Returns the list of all exchanges (and asset-class venues) supported by EODHD.
        /// </summary>
        /// <returns>The exchanges.</returns>
        public List<Exchange> GetExchangesList()
        {
            return this.GetExchangesListAsync().GetAwaiter().GetResult();
        }

        // ================================================================
        // ID Mapping API
        // ================================================================

        /// <summary>
        /// Maps between financial identifiers. At least one filter must be supplied.
        /// </summary>
        /// <param name="symbol">Optional EODHD symbol filter (e.g. <c>"AAPL.US"</c>).</param>
        /// <param name="exchange">Optional exchange-code filter.</param>
        /// <param name="isin">Optional ISIN filter.</param>
        /// <param name="figi">Optional FIGI filter.</param>
        /// <param name="lei">Optional LEI filter.</param>
        /// <param name="cusip">Optional CUSIP filter.</param>
        /// <param name="offset">Optional pagination offset.</param>
        /// <param name="limit">Optional page size.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>The matching identifier mappings.</returns>
        public async Task<IdMapping[]> GetIdMappingAsync(string symbol = null, string exchange = null, string isin = null, string figi = null, string lei = null, string cusip = null, int? offset = null, int? limit = null, CancellationToken ct = default)
        {
            if(string.IsNullOrWhiteSpace(symbol) && string.IsNullOrWhiteSpace(exchange) && string.IsNullOrWhiteSpace(isin)
                && string.IsNullOrWhiteSpace(figi) && string.IsNullOrWhiteSpace(lei) && string.IsNullOrWhiteSpace(cusip))
            {
                throw new ArgumentException("At least one identifier filter must be supplied.", nameof(symbol));
            }

            DataEnvelope<IdMapping> response = await this.GetJsonAsync<DataEnvelope<IdMapping>>(
                "id-mapping",
                ct,
                ("filter[symbol]", symbol),
                ("filter[ex]", exchange),
                ("filter[isin]", isin),
                ("filter[figi]", figi),
                ("filter[lei]", lei),
                ("filter[cusip]", cusip),
                ("page[offset]", FormatInt(offset)),
                ("page[limit]", FormatInt(limit))).ConfigureAwait(false);

            return response?.Data ?? Array.Empty<IdMapping>();
        }

        /// <summary>
        /// Maps between financial identifiers. At least one filter must be supplied.
        /// </summary>
        /// <param name="symbol">Optional EODHD symbol filter (e.g. <c>"AAPL.US"</c>).</param>
        /// <param name="exchange">Optional exchange-code filter.</param>
        /// <param name="isin">Optional ISIN filter.</param>
        /// <param name="figi">Optional FIGI filter.</param>
        /// <param name="lei">Optional LEI filter.</param>
        /// <param name="cusip">Optional CUSIP filter.</param>
        /// <param name="offset">Optional pagination offset.</param>
        /// <param name="limit">Optional page size.</param>
        /// <returns>The matching identifier mappings.</returns>
        public IdMapping[] GetIdMapping(string symbol = null, string exchange = null, string isin = null, string figi = null, string lei = null, string cusip = null, int? offset = null, int? limit = null)
        {
            return this.GetIdMappingAsync(symbol, exchange, isin, figi, lei, cusip, offset, limit).GetAwaiter().GetResult();
        }
    }
}
