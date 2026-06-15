using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

using EODHD.CSharpApiClient.DataModel;
using EODHD.CSharpApiClient.DataModel.BulkFundamental;
using EODHD.CSharpApiClient.DataModel.EconomicEvents;
using EODHD.CSharpApiClient.DataModel.ExchangeInfo;
using EODHD.CSharpApiClient.DataModel.Fundamental;
using EODHD.CSharpApiClient.DataModel.News;
using EODHD.CSharpApiClient.DataModel.Sentiment;
using EODHD.CSharpApiClient.DataModel.TechnicalIndicators;
using EODHD.CSharpApiClient.DataModel.UpcomingEarnings;
using EODHD.CSharpApiClient.DataModel.UpcomingIpos;
using EODHD.CSharpApiClient.DataModel.UpcomingSplits;
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
        // Calendar API
        // ================================================================

        /// <summary>
        /// Returns upcoming earnings announcements, optionally filtered by date range and symbols.
        /// </summary>
        /// <param name="from">Optional inclusive start date.</param>
        /// <param name="to">Optional inclusive end date.</param>
        /// <param name="symbols">Optional list of symbols to filter to.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>The upcoming earnings.</returns>
        public Task<UpcomingEarnings> GetUpcomingEarningsAsync(DateTime? from = null, DateTime? to = null, IReadOnlyList<string> symbols = null, CancellationToken ct = default)
        {
            return this.GetJsonAsync<UpcomingEarnings>(
                "calendar/earnings",
                ct,
                ("from", FormatDate(from)),
                ("to", FormatDate(to)),
                ("symbols", JoinSymbols(symbols)));
        }

        /// <summary>
        /// Returns upcoming earnings announcements, optionally filtered by date range and symbols.
        /// </summary>
        /// <param name="from">Optional inclusive start date.</param>
        /// <param name="to">Optional inclusive end date.</param>
        /// <param name="symbols">Optional list of symbols to filter to.</param>
        /// <returns>The upcoming earnings.</returns>
        public UpcomingEarnings GetUpcomingEarnings(DateTime? from = null, DateTime? to = null, IReadOnlyList<string> symbols = null)
        {
            return this.GetUpcomingEarningsAsync(from, to, symbols).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Returns upcoming stock splits, optionally filtered by date range.
        /// </summary>
        /// <param name="from">Optional inclusive start date.</param>
        /// <param name="to">Optional inclusive end date.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>The upcoming splits.</returns>
        public Task<UpcomingSplits> GetUpcomingSplitsAsync(DateTime? from = null, DateTime? to = null, CancellationToken ct = default)
        {
            return this.GetJsonAsync<UpcomingSplits>(
                "calendar/splits",
                ct,
                ("from", FormatDate(from)),
                ("to", FormatDate(to)));
        }

        /// <summary>
        /// Returns upcoming stock splits, optionally filtered by date range.
        /// </summary>
        /// <param name="from">Optional inclusive start date.</param>
        /// <param name="to">Optional inclusive end date.</param>
        /// <returns>The upcoming splits.</returns>
        public UpcomingSplits GetUpcomingSplits(DateTime? from = null, DateTime? to = null)
        {
            return this.GetUpcomingSplitsAsync(from, to).GetAwaiter().GetResult();
        }

        // ================================================================
        // End-of-Day Historical Stock Market Data API
        // ================================================================

        /// <summary>
        /// Returns end-of-day historical OHLCV bars for a symbol.
        /// </summary>
        /// <param name="symbol">The full EODHD symbol (e.g. <c>"AAPL.US"</c>).</param>
        /// <param name="from">Inclusive start date.</param>
        /// <param name="to">Inclusive end date.</param>
        /// <param name="period">Aggregation period (daily, weekly, monthly).</param>
        /// <param name="ascending">When <see langword="true"/> (default), bars are returned oldest-first.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>The historical bars.</returns>
        public Task<List<HistoricalStockPrice>> GetEndOfDayHistoricalStockPricesAsync(string symbol, DateTime from, DateTime to, EndOfDayPeriod period, bool ascending = true, CancellationToken ct = default)
        {
            if(string.IsNullOrWhiteSpace(symbol))
            {
                throw new ArgumentNullException(nameof(symbol));
            }

            return this.GetJsonAsync<List<HistoricalStockPrice>>(
                "eod/" + Uri.EscapeDataString(symbol),
                ct,
                ("from", from.ToString(DateFormat, CultureInfo.InvariantCulture)),
                ("to", to.ToString(DateFormat, CultureInfo.InvariantCulture)),
                ("period", ((char)period).ToString()),
                ("order", ascending ? null : "d"));
        }

        /// <summary>
        /// Returns end-of-day historical OHLCV bars for a symbol.
        /// </summary>
        /// <param name="symbol">The full EODHD symbol (e.g. <c>"AAPL.US"</c>).</param>
        /// <param name="from">Inclusive start date.</param>
        /// <param name="to">Inclusive end date.</param>
        /// <param name="period">Aggregation period (daily, weekly, monthly).</param>
        /// <param name="ascending">When <see langword="true"/> (default), bars are returned oldest-first.</param>
        /// <returns>The historical bars.</returns>
        public List<HistoricalStockPrice> GetEndOfDayHistoricalStockPrices(string symbol, DateTime from, DateTime to, EndOfDayPeriod period, bool ascending = true)
        {
            return this.GetEndOfDayHistoricalStockPricesAsync(symbol, from, to, period, ascending).GetAwaiter().GetResult();
        }

        // ================================================================
        // Fundamental Data API
        // ================================================================

        /// <summary>
        /// Returns the full fundamentals dataset for a symbol.
        /// </summary>
        /// <param name="symbol">The full EODHD symbol (e.g. <c>"AAPL.US"</c>).</param>
        /// <param name="filters">Optional EODHD <c>filter</c> expression to return only part of the dataset.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>The fundamentals data.</returns>
        public Task<FundamentalData> GetFundamentalsDataAsync(string symbol, string filters = null, CancellationToken ct = default)
        {
            if(string.IsNullOrWhiteSpace(symbol))
            {
                throw new ArgumentNullException(nameof(symbol));
            }

            return this.GetJsonAsync<FundamentalData>(
                "fundamentals/" + Uri.EscapeDataString(symbol),
                ct,
                ("filter", filters));
        }

        /// <summary>
        /// Returns the full fundamentals dataset for a symbol.
        /// </summary>
        /// <param name="symbol">The full EODHD symbol (e.g. <c>"AAPL.US"</c>).</param>
        /// <param name="filters">Optional EODHD <c>filter</c> expression to return only part of the dataset.</param>
        /// <returns>The fundamentals data.</returns>
        public FundamentalData GetFundamentalsData(string symbol, string filters = null)
        {
            return this.GetFundamentalsDataAsync(symbol, filters).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Returns bulk fundamentals for all symbols on an exchange, keyed by symbol code.
        /// </summary>
        /// <param name="exchangeCode">The EODHD exchange code (e.g. <c>"US"</c>).</param>
        /// <param name="offset">Optional pagination offset.</param>
        /// <param name="limit">Optional page size.</param>
        /// <param name="symbols">Optional list of symbols to restrict the response to.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A dictionary of bulk fundamentals keyed by symbol code.</returns>
        public Task<Dictionary<string, BulkFundamentalData>> GetBulkFundamentalsDataAsync(string exchangeCode, int? offset = null, int? limit = null, IReadOnlyList<string> symbols = null, CancellationToken ct = default)
        {
            if(string.IsNullOrWhiteSpace(exchangeCode))
            {
                throw new ArgumentNullException(nameof(exchangeCode));
            }

            return this.GetJsonAsync<Dictionary<string, BulkFundamentalData>>(
                "bulk-fundamentals/" + Uri.EscapeDataString(exchangeCode),
                ct,
                ("offset", FormatInt(offset)),
                ("limit", FormatInt(limit)),
                ("symbols", JoinSymbols(symbols)));
        }

        /// <summary>
        /// Returns bulk fundamentals for all symbols on an exchange, keyed by symbol code.
        /// </summary>
        /// <param name="exchangeCode">The EODHD exchange code (e.g. <c>"US"</c>).</param>
        /// <param name="offset">Optional pagination offset.</param>
        /// <param name="limit">Optional page size.</param>
        /// <param name="symbols">Optional list of symbols to restrict the response to.</param>
        /// <returns>A dictionary of bulk fundamentals keyed by symbol code.</returns>
        public Dictionary<string, BulkFundamentalData> GetBulkFundamentalsData(string exchangeCode, int? offset = null, int? limit = null, IReadOnlyList<string> symbols = null)
        {
            return this.GetBulkFundamentalsDataAsync(exchangeCode, offset, limit, symbols).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Returns the extended (version 1.2) bulk fundamentals for an exchange as a list.
        /// </summary>
        /// <param name="exchangeCode">The EODHD exchange code (e.g. <c>"US"</c>).</param>
        /// <param name="offset">Optional pagination offset.</param>
        /// <param name="limit">Optional page size.</param>
        /// <param name="symbols">Optional list of symbols to restrict the response to.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>The bulk fundamentals records.</returns>
        public Task<List<BulkFundamentalData>> GetBulkFundamentalsExtendedDataAsync(string exchangeCode, int? offset = null, int? limit = null, IReadOnlyList<string> symbols = null, CancellationToken ct = default)
        {
            if(string.IsNullOrWhiteSpace(exchangeCode))
            {
                throw new ArgumentNullException(nameof(exchangeCode));
            }

            return this.GetJsonAsync<List<BulkFundamentalData>>(
                "bulk-fundamentals/" + Uri.EscapeDataString(exchangeCode),
                ct,
                ("version", "1.2"),
                ("offset", FormatInt(offset)),
                ("limit", FormatInt(limit)),
                ("symbols", JoinSymbols(symbols)));
        }

        /// <summary>
        /// Returns the extended (version 1.2) bulk fundamentals for an exchange as a list.
        /// </summary>
        /// <param name="exchangeCode">The EODHD exchange code (e.g. <c>"US"</c>).</param>
        /// <param name="offset">Optional pagination offset.</param>
        /// <param name="limit">Optional page size.</param>
        /// <param name="symbols">Optional list of symbols to restrict the response to.</param>
        /// <returns>The bulk fundamentals records.</returns>
        public List<BulkFundamentalData> GetBulkFundamentalsExtendedData(string exchangeCode, int? offset = null, int? limit = null, IReadOnlyList<string> symbols = null)
        {
            return this.GetBulkFundamentalsExtendedDataAsync(exchangeCode, offset, limit, symbols).GetAwaiter().GetResult();
        }

        // ================================================================
        // Dividends and Splits
        // ================================================================

        /// <summary>
        /// Returns the historical stock splits for a symbol.
        /// </summary>
        /// <param name="symbol">The full EODHD symbol (e.g. <c>"AAPL.US"</c>).</param>
        /// <param name="from">Optional inclusive start date.</param>
        /// <param name="to">Optional inclusive end date.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>The historical splits.</returns>
        public Task<HistoricalSplit[]> GetHistoricalSplitsAsync(string symbol, DateTime? from = null, DateTime? to = null, CancellationToken ct = default)
        {
            if(string.IsNullOrWhiteSpace(symbol))
            {
                throw new ArgumentNullException(nameof(symbol));
            }

            return this.GetJsonAsync<HistoricalSplit[]>(
                "splits/" + Uri.EscapeDataString(symbol),
                ct,
                ("from", FormatDate(from)),
                ("to", FormatDate(to)));
        }

        /// <summary>
        /// Returns the historical stock splits for a symbol.
        /// </summary>
        /// <param name="symbol">The full EODHD symbol (e.g. <c>"AAPL.US"</c>).</param>
        /// <param name="from">Optional inclusive start date.</param>
        /// <param name="to">Optional inclusive end date.</param>
        /// <returns>The historical splits.</returns>
        public HistoricalSplit[] GetHistoricalSplits(string symbol, DateTime? from = null, DateTime? to = null)
        {
            return this.GetHistoricalSplitsAsync(symbol, from, to).GetAwaiter().GetResult();
        }

        // ================================================================
        // Bulk API for EOD, Splits and Dividends
        // ================================================================

        /// <summary>
        /// Returns the bulk end-of-day prices for all US symbols for a single day.
        /// </summary>
        /// <param name="date">The trading day to fetch; <c>null</c> returns the most recent available day.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>The bulk end-of-day prices.</returns>
        public Task<BulkHistoricalStockPrice[]> GetBulkEndOfDayHistoricalStockPricesAsync(DateTime? date = null, CancellationToken ct = default)
        {
            return this.GetJsonAsync<BulkHistoricalStockPrice[]>(
                "eod-bulk-last-day/US",
                ct,
                ("date", FormatDate(date)));
        }

        /// <summary>
        /// Returns the bulk end-of-day prices for all US symbols for a single day.
        /// </summary>
        /// <param name="date">The trading day to fetch; <c>null</c> returns the most recent available day.</param>
        /// <returns>The bulk end-of-day prices.</returns>
        public BulkHistoricalStockPrice[] GetBulkEndOfDayHistoricalStockPrices(DateTime? date = null)
        {
            return this.GetBulkEndOfDayHistoricalStockPricesAsync(date).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Returns the bulk stock splits for all US symbols for a single day.
        /// </summary>
        /// <param name="date">The trading day to fetch; <c>null</c> returns the most recent available day.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>The bulk splits.</returns>
        public Task<BulkHistoricalSplit[]> GetBulkHistoricalSplitsAsync(DateTime? date = null, CancellationToken ct = default)
        {
            return this.GetJsonAsync<BulkHistoricalSplit[]>(
                "eod-bulk-last-day/US",
                ct,
                ("type", "splits"),
                ("date", FormatDate(date)));
        }

        /// <summary>
        /// Returns the bulk stock splits for all US symbols for a single day.
        /// </summary>
        /// <param name="date">The trading day to fetch; <c>null</c> returns the most recent available day.</param>
        /// <returns>The bulk splits.</returns>
        public BulkHistoricalSplit[] GetBulkHistoricalSplits(DateTime? date = null)
        {
            return this.GetBulkHistoricalSplitsAsync(date).GetAwaiter().GetResult();
        }

        // ================================================================
        // Intraday Historical Data API
        // ================================================================

        /// <summary>
        /// Returns intraday OHLCV bars for a symbol.
        /// </summary>
        /// <param name="symbol">The full EODHD symbol (e.g. <c>"AAPL.US"</c>).</param>
        /// <param name="interval">Bar interval. Defaults to five minutes.</param>
        /// <param name="from">Optional inclusive start (UTC); EODHD limits the range to ~120 days.</param>
        /// <param name="to">Optional inclusive end (UTC).</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>The intraday bars.</returns>
        public Task<List<IntradayHistoricalStockPrice>> GetIntradayHistoricalStockPricesAsync(string symbol, IntradayInterval interval = IntradayInterval.FiveMinutes, DateTime? from = null, DateTime? to = null, CancellationToken ct = default)
        {
            if(string.IsNullOrWhiteSpace(symbol))
            {
                throw new ArgumentNullException(nameof(symbol));
            }

            return this.GetJsonAsync<List<IntradayHistoricalStockPrice>>(
                "intraday/" + Uri.EscapeDataString(symbol),
                ct,
                ("interval", IntervalToString(interval)),
                ("from", FormatUnix(from)),
                ("to", FormatUnix(to)));
        }

        /// <summary>
        /// Returns intraday OHLCV bars for a symbol.
        /// </summary>
        /// <param name="symbol">The full EODHD symbol (e.g. <c>"AAPL.US"</c>).</param>
        /// <param name="interval">Bar interval. Defaults to five minutes.</param>
        /// <param name="from">Optional inclusive start (UTC); EODHD limits the range to ~120 days.</param>
        /// <param name="to">Optional inclusive end (UTC).</param>
        /// <returns>The intraday bars.</returns>
        public List<IntradayHistoricalStockPrice> GetIntradayHistoricalStockPrices(string symbol, IntradayInterval interval = IntradayInterval.FiveMinutes, DateTime? from = null, DateTime? to = null)
        {
            return this.GetIntradayHistoricalStockPricesAsync(symbol, interval, from, to).GetAwaiter().GetResult();
        }

        // ================================================================
        // Dividends API
        // ================================================================

        /// <summary>
        /// Returns the historical dividends for a symbol.
        /// </summary>
        /// <param name="symbol">The full EODHD symbol (e.g. <c>"AAPL.US"</c>).</param>
        /// <param name="from">Optional inclusive start date.</param>
        /// <param name="to">Optional inclusive end date.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>The historical dividends.</returns>
        public Task<HistoricalDividend[]> GetHistoricalDividendsAsync(string symbol, DateTime? from = null, DateTime? to = null, CancellationToken ct = default)
        {
            if(string.IsNullOrWhiteSpace(symbol))
            {
                throw new ArgumentNullException(nameof(symbol));
            }

            return this.GetJsonAsync<HistoricalDividend[]>(
                "div/" + Uri.EscapeDataString(symbol),
                ct,
                ("from", FormatDate(from)),
                ("to", FormatDate(to)));
        }

        /// <summary>
        /// Returns the historical dividends for a symbol.
        /// </summary>
        /// <param name="symbol">The full EODHD symbol (e.g. <c>"AAPL.US"</c>).</param>
        /// <param name="from">Optional inclusive start date.</param>
        /// <param name="to">Optional inclusive end date.</param>
        /// <returns>The historical dividends.</returns>
        public HistoricalDividend[] GetHistoricalDividends(string symbol, DateTime? from = null, DateTime? to = null)
        {
            return this.GetHistoricalDividendsAsync(symbol, from, to).GetAwaiter().GetResult();
        }

        // ================================================================
        // Live / Delayed (Real-Time) API
        // ================================================================

        /// <summary>
        /// Returns the live (delayed ~15–20 min) quote for a single symbol.
        /// </summary>
        /// <param name="symbol">The full EODHD symbol (e.g. <c>"AAPL.US"</c>).</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>The live quote.</returns>
        public Task<LiveStockPrice> GetLivePriceAsync(string symbol, CancellationToken ct = default)
        {
            if(string.IsNullOrWhiteSpace(symbol))
            {
                throw new ArgumentNullException(nameof(symbol));
            }

            return this.GetJsonAsync<LiveStockPrice>("real-time/" + Uri.EscapeDataString(symbol), ct);
        }

        /// <summary>
        /// Returns the live (delayed ~15–20 min) quote for a single symbol.
        /// </summary>
        /// <param name="symbol">The full EODHD symbol (e.g. <c>"AAPL.US"</c>).</param>
        /// <returns>The live quote.</returns>
        public LiveStockPrice GetLivePrice(string symbol)
        {
            return this.GetLivePriceAsync(symbol).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Returns live (delayed ~15–20 min) quotes for several symbols in a single request. EODHD
        /// recommends no more than 15–20 symbols per call.
        /// </summary>
        /// <param name="primarySymbol">The primary EODHD symbol (path segment).</param>
        /// <param name="additionalSymbols">Additional symbols sent via the <c>s</c> parameter.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>One quote per requested symbol.</returns>
        public Task<LiveStockPrice[]> GetLivePricesAsync(string primarySymbol, IReadOnlyList<string> additionalSymbols, CancellationToken ct = default)
        {
            if(string.IsNullOrWhiteSpace(primarySymbol))
            {
                throw new ArgumentNullException(nameof(primarySymbol));
            }

            return this.GetJsonAsync<LiveStockPrice[]>(
                "real-time/" + Uri.EscapeDataString(primarySymbol),
                ct,
                ("s", JoinSymbols(additionalSymbols)));
        }

        /// <summary>
        /// Returns live (delayed ~15–20 min) quotes for several symbols in a single request. EODHD
        /// recommends no more than 15–20 symbols per call.
        /// </summary>
        /// <param name="primarySymbol">The primary EODHD symbol (path segment).</param>
        /// <param name="additionalSymbols">Additional symbols sent via the <c>s</c> parameter.</param>
        /// <returns>One quote per requested symbol.</returns>
        public LiveStockPrice[] GetLivePrices(string primarySymbol, IReadOnlyList<string> additionalSymbols)
        {
            return this.GetLivePricesAsync(primarySymbol, additionalSymbols).GetAwaiter().GetResult();
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
        // Calendar — IPOs
        // ================================================================

        /// <summary>
        /// Returns the IPO calendar, optionally filtered by date range.
        /// </summary>
        /// <param name="from">Optional inclusive start date.</param>
        /// <param name="to">Optional inclusive end date.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>The IPO calendar.</returns>
        public Task<UpcomingIpos> GetUpcomingIposAsync(DateTime? from = null, DateTime? to = null, CancellationToken ct = default)
        {
            return this.GetJsonAsync<UpcomingIpos>(
                "calendar/ipos",
                ct,
                ("from", FormatDate(from)),
                ("to", FormatDate(to)));
        }

        /// <summary>
        /// Returns the IPO calendar, optionally filtered by date range.
        /// </summary>
        /// <param name="from">Optional inclusive start date.</param>
        /// <param name="to">Optional inclusive end date.</param>
        /// <returns>The IPO calendar.</returns>
        public UpcomingIpos GetUpcomingIpos(DateTime? from = null, DateTime? to = null)
        {
            return this.GetUpcomingIposAsync(from, to).GetAwaiter().GetResult();
        }

        // ================================================================
        // Financial News API
        // ================================================================

        /// <summary>
        /// Returns financial news articles for a symbol and/or topic tag. At least one of
        /// <paramref name="symbol"/> or <paramref name="tag"/> must be supplied.
        /// </summary>
        /// <param name="symbol">Optional ticker to fetch news for (e.g. <c>"AAPL.US"</c>).</param>
        /// <param name="tag">Optional topic tag to fetch news for (e.g. <c>"technology"</c>).</param>
        /// <param name="from">Optional inclusive start date.</param>
        /// <param name="to">Optional inclusive end date.</param>
        /// <param name="limit">Optional results per request (1–1000, default 50).</param>
        /// <param name="offset">Optional pagination offset (default 0).</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>The matching news articles.</returns>
        public Task<NewsArticle[]> GetNewsAsync(string symbol = null, string tag = null, DateTime? from = null, DateTime? to = null, int? limit = null, int? offset = null, CancellationToken ct = default)
        {
            if(string.IsNullOrWhiteSpace(symbol) && string.IsNullOrWhiteSpace(tag))
            {
                throw new ArgumentException("At least one of symbol or tag must be supplied.", nameof(symbol));
            }

            return this.GetJsonAsync<NewsArticle[]>(
                "news",
                ct,
                ("s", symbol),
                ("t", tag),
                ("from", FormatDate(from)),
                ("to", FormatDate(to)),
                ("limit", FormatInt(limit)),
                ("offset", FormatInt(offset)));
        }

        /// <summary>
        /// Returns financial news articles for a symbol and/or topic tag. At least one of
        /// <paramref name="symbol"/> or <paramref name="tag"/> must be supplied.
        /// </summary>
        /// <param name="symbol">Optional ticker to fetch news for (e.g. <c>"AAPL.US"</c>).</param>
        /// <param name="tag">Optional topic tag to fetch news for (e.g. <c>"technology"</c>).</param>
        /// <param name="from">Optional inclusive start date.</param>
        /// <param name="to">Optional inclusive end date.</param>
        /// <param name="limit">Optional results per request (1–1000, default 50).</param>
        /// <param name="offset">Optional pagination offset (default 0).</param>
        /// <returns>The matching news articles.</returns>
        public NewsArticle[] GetNews(string symbol = null, string tag = null, DateTime? from = null, DateTime? to = null, int? limit = null, int? offset = null)
        {
            return this.GetNewsAsync(symbol, tag, from, to, limit, offset).GetAwaiter().GetResult();
        }

        // ================================================================
        // Sentiment API
        // ================================================================

        /// <summary>
        /// Returns daily aggregated news sentiment for one or more symbols, keyed by symbol code.
        /// </summary>
        /// <param name="symbols">The symbols to fetch sentiment for.</param>
        /// <param name="from">Optional inclusive start date.</param>
        /// <param name="to">Optional inclusive end date.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A dictionary of daily sentiment series keyed by symbol code.</returns>
        public Task<Dictionary<string, List<SentimentPoint>>> GetNewsSentimentsAsync(IReadOnlyList<string> symbols, DateTime? from = null, DateTime? to = null, CancellationToken ct = default)
        {
            return this.GetSentimentsAsync("sentiments", symbols, from, to, ct);
        }

        /// <summary>
        /// Returns daily aggregated news sentiment for one or more symbols, keyed by symbol code.
        /// </summary>
        /// <param name="symbols">The symbols to fetch sentiment for.</param>
        /// <param name="from">Optional inclusive start date.</param>
        /// <param name="to">Optional inclusive end date.</param>
        /// <returns>A dictionary of daily sentiment series keyed by symbol code.</returns>
        public Dictionary<string, List<SentimentPoint>> GetNewsSentiments(IReadOnlyList<string> symbols, DateTime? from = null, DateTime? to = null)
        {
            return this.GetNewsSentimentsAsync(symbols, from, to).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Returns daily aggregated social-media (tweets) sentiment for one or more symbols, keyed by
        /// symbol code.
        /// </summary>
        /// <param name="symbols">The symbols to fetch sentiment for.</param>
        /// <param name="from">Optional inclusive start date.</param>
        /// <param name="to">Optional inclusive end date.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A dictionary of daily sentiment series keyed by symbol code.</returns>
        public Task<Dictionary<string, List<SentimentPoint>>> GetTweetsSentimentsAsync(IReadOnlyList<string> symbols, DateTime? from = null, DateTime? to = null, CancellationToken ct = default)
        {
            return this.GetSentimentsAsync("tweets-sentiments", symbols, from, to, ct);
        }

        /// <summary>
        /// Returns daily aggregated social-media (tweets) sentiment for one or more symbols, keyed by
        /// symbol code.
        /// </summary>
        /// <param name="symbols">The symbols to fetch sentiment for.</param>
        /// <param name="from">Optional inclusive start date.</param>
        /// <param name="to">Optional inclusive end date.</param>
        /// <returns>A dictionary of daily sentiment series keyed by symbol code.</returns>
        public Dictionary<string, List<SentimentPoint>> GetTweetsSentiments(IReadOnlyList<string> symbols, DateTime? from = null, DateTime? to = null)
        {
            return this.GetTweetsSentimentsAsync(symbols, from, to).GetAwaiter().GetResult();
        }

        // ================================================================
        // Economic Events API
        // ================================================================

        /// <summary>
        /// Returns economic calendar events, optionally filtered.
        /// </summary>
        /// <param name="from">Optional inclusive start date.</param>
        /// <param name="to">Optional inclusive end date.</param>
        /// <param name="country">Optional ISO 3166 country code filter (e.g. <c>"US"</c>).</param>
        /// <param name="comparison">Optional comparison basis (<c>"mom"</c>, <c>"qoq"</c>, <c>"yoy"</c>).</param>
        /// <param name="type">Optional event-type filter (e.g. <c>"GDP Growth Rate"</c>).</param>
        /// <param name="offset">Optional pagination offset (0–1000, default 0).</param>
        /// <param name="limit">Optional number of results (0–1000, default 50).</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>The economic events.</returns>
        public Task<EconomicEvent[]> GetEconomicEventsAsync(DateTime? from = null, DateTime? to = null, string country = null, string comparison = null, string type = null, int? offset = null, int? limit = null, CancellationToken ct = default)
        {
            return this.GetJsonAsync<EconomicEvent[]>(
                "economic-events",
                ct,
                ("from", FormatDate(from)),
                ("to", FormatDate(to)),
                ("country", country),
                ("comparison", comparison),
                ("type", type),
                ("offset", FormatInt(offset)),
                ("limit", FormatInt(limit)));
        }

        /// <summary>
        /// Returns economic calendar events, optionally filtered.
        /// </summary>
        /// <param name="from">Optional inclusive start date.</param>
        /// <param name="to">Optional inclusive end date.</param>
        /// <param name="country">Optional ISO 3166 country code filter (e.g. <c>"US"</c>).</param>
        /// <param name="comparison">Optional comparison basis (<c>"mom"</c>, <c>"qoq"</c>, <c>"yoy"</c>).</param>
        /// <param name="type">Optional event-type filter (e.g. <c>"GDP Growth Rate"</c>).</param>
        /// <param name="offset">Optional pagination offset (0–1000, default 0).</param>
        /// <param name="limit">Optional number of results (0–1000, default 50).</param>
        /// <returns>The economic events.</returns>
        public EconomicEvent[] GetEconomicEvents(DateTime? from = null, DateTime? to = null, string country = null, string comparison = null, string type = null, int? offset = null, int? limit = null)
        {
            return this.GetEconomicEventsAsync(from, to, country, comparison, type, offset, limit).GetAwaiter().GetResult();
        }

        // ================================================================
        // Technical Indicators API
        // ================================================================

        /// <summary>
        /// Returns a technical-indicator series for a symbol. The output values vary by
        /// <paramref name="function"/> and are exposed per data point in
        /// <see cref="TechnicalIndicatorPoint.Values"/>.
        /// </summary>
        /// <param name="symbol">The full EODHD symbol (e.g. <c>"AAPL.US"</c>).</param>
        /// <param name="function">The indicator function to compute.</param>
        /// <param name="period">Optional look-back period (default 50; range 2–100000).</param>
        /// <param name="from">Optional inclusive start date.</param>
        /// <param name="to">Optional inclusive end date.</param>
        /// <param name="ascending">When <see langword="true"/> (default), points are oldest-first.</param>
        /// <param name="extraParameters">
        /// Optional function-specific parameters (e.g. <c>fast_kperiod</c>, <c>slow_kperiod</c>,
        /// <c>signal_period</c>, <c>acceleration</c>, <c>maximum</c>, <c>code2</c>, <c>agg_period</c>),
        /// passed through verbatim as query parameters.
        /// </param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>The indicator data points.</returns>
        public Task<TechnicalIndicatorPoint[]> GetTechnicalIndicatorAsync(string symbol, TechnicalFunction function, int? period = null, DateTime? from = null, DateTime? to = null, bool ascending = true, IReadOnlyDictionary<string, string> extraParameters = null, CancellationToken ct = default)
        {
            if(string.IsNullOrWhiteSpace(symbol))
            {
                throw new ArgumentNullException(nameof(symbol));
            }

            List<(string Name, string Value)> query = new List<(string Name, string Value)>
            {
                ("function", TechnicalFunctionToString(function)),
                ("period", FormatInt(period)),
                ("from", FormatDate(from)),
                ("to", FormatDate(to)),
                ("order", ascending ? null : "d"),
            };

            if(extraParameters != null)
            {
                foreach(KeyValuePair<string, string> parameter in extraParameters)
                {
                    query.Add((parameter.Key, parameter.Value));
                }
            }

            return this.GetJsonAsync<TechnicalIndicatorPoint[]>(
                "technical/" + Uri.EscapeDataString(symbol),
                ct,
                query.ToArray());
        }

        /// <summary>
        /// Returns a technical-indicator series for a symbol. The output values vary by
        /// <paramref name="function"/> and are exposed per data point in
        /// <see cref="TechnicalIndicatorPoint.Values"/>.
        /// </summary>
        /// <param name="symbol">The full EODHD symbol (e.g. <c>"AAPL.US"</c>).</param>
        /// <param name="function">The indicator function to compute.</param>
        /// <param name="period">Optional look-back period (default 50; range 2–100000).</param>
        /// <param name="from">Optional inclusive start date.</param>
        /// <param name="to">Optional inclusive end date.</param>
        /// <param name="ascending">When <see langword="true"/> (default), points are oldest-first.</param>
        /// <param name="extraParameters">Optional function-specific parameters passed through verbatim.</param>
        /// <returns>The indicator data points.</returns>
        public TechnicalIndicatorPoint[] GetTechnicalIndicator(string symbol, TechnicalFunction function, int? period = null, DateTime? from = null, DateTime? to = null, bool ascending = true, IReadOnlyDictionary<string, string> extraParameters = null)
        {
            return this.GetTechnicalIndicatorAsync(symbol, function, period, from, to, ascending, extraParameters).GetAwaiter().GetResult();
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

        private static string FormatUnix(DateTime? date)
        {
            if(!date.HasValue)
            {
                return null;
            }

            DateTime utc = DateTime.SpecifyKind(date.Value, DateTimeKind.Utc);
            return new DateTimeOffset(utc).ToUnixTimeSeconds().ToString(CultureInfo.InvariantCulture);
        }

        private Task<Dictionary<string, List<SentimentPoint>>> GetSentimentsAsync(string path, IReadOnlyList<string> symbols, DateTime? from, DateTime? to, CancellationToken ct)
        {
            if(symbols == null || symbols.Count == 0)
            {
                throw new ArgumentException("At least one symbol must be supplied.", nameof(symbols));
            }

            return this.GetJsonAsync<Dictionary<string, List<SentimentPoint>>>(
                path,
                ct,
                ("s", JoinSymbols(symbols)),
                ("from", FormatDate(from)),
                ("to", FormatDate(to)));
        }

        private static string TechnicalFunctionToString(TechnicalFunction function)
        {
            switch(function)
            {
                case TechnicalFunction.AverageVolume:
                    return "avgvol";
                case TechnicalFunction.AverageVolumeByCurrency:
                    return "avgvolccy";
                case TechnicalFunction.Sma:
                    return "sma";
                case TechnicalFunction.Ema:
                    return "ema";
                case TechnicalFunction.Wma:
                    return "wma";
                case TechnicalFunction.Volatility:
                    return "volatility";
                case TechnicalFunction.Stochastic:
                    return "stochastic";
                case TechnicalFunction.Rsi:
                    return "rsi";
                case TechnicalFunction.StandardDeviation:
                    return "stddev";
                case TechnicalFunction.StochasticRsi:
                    return "stochrsi";
                case TechnicalFunction.Slope:
                    return "slope";
                case TechnicalFunction.DirectionalMovementIndex:
                    return "dmi";
                case TechnicalFunction.Adx:
                    return "adx";
                case TechnicalFunction.Macd:
                    return "macd";
                case TechnicalFunction.Atr:
                    return "atr";
                case TechnicalFunction.Cci:
                    return "cci";
                case TechnicalFunction.ParabolicSar:
                    return "sar";
                case TechnicalFunction.Beta:
                    return "beta";
                case TechnicalFunction.BollingerBands:
                    return "bbands";
                case TechnicalFunction.SplitAdjusted:
                    return "splitadjusted";
                default:
                    throw new ArgumentOutOfRangeException(nameof(function), function, "Unknown technical function.");
            }
        }

        private static string IntervalToString(IntradayInterval interval)
        {
            switch(interval)
            {
                case IntradayInterval.OneMinute:
                    return "1m";
                case IntradayInterval.OneHour:
                    return "1h";
                case IntradayInterval.FiveMinutes:
                default:
                    return "5m";
            }
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
