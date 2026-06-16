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
        // Insider Transactions API
        // ================================================================

        /// <summary>
        /// Returns insider (SEC Form 4) transactions, optionally filtered by symbol and date range.
        /// </summary>
        /// <param name="symbol">Optional ticker to filter to (e.g. <c>"AAPL.US"</c>); <c>null</c> returns all symbols.</param>
        /// <param name="from">Optional inclusive start date (default: one year ago).</param>
        /// <param name="to">Optional inclusive end date (default: today).</param>
        /// <param name="limit">Optional number of entries (1–1000, default 100).</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>The insider transactions.</returns>
        public Task<InsiderTransaction[]> GetInsiderTransactionsAsync(string symbol = null, DateTime? from = null, DateTime? to = null, int? limit = null, CancellationToken ct = default)
        {
            return this.GetJsonAsync<InsiderTransaction[]>(
                "insider-transactions",
                ct,
                ("code", symbol),
                ("from", FormatDate(from)),
                ("to", FormatDate(to)),
                ("limit", FormatInt(limit)));
        }

        /// <summary>
        /// Returns insider (SEC Form 4) transactions, optionally filtered by symbol and date range.
        /// </summary>
        /// <param name="symbol">Optional ticker to filter to (e.g. <c>"AAPL.US"</c>); <c>null</c> returns all symbols.</param>
        /// <param name="from">Optional inclusive start date (default: one year ago).</param>
        /// <param name="to">Optional inclusive end date (default: today).</param>
        /// <param name="limit">Optional number of entries (1–1000, default 100).</param>
        /// <returns>The insider transactions.</returns>
        public InsiderTransaction[] GetInsiderTransactions(string symbol = null, DateTime? from = null, DateTime? to = null, int? limit = null)
        {
            return this.GetInsiderTransactionsAsync(symbol, from, to, limit).GetAwaiter().GetResult();
        }

        // ================================================================
        // Bond Fundamentals API
        // ================================================================

        /// <summary>
        /// Returns the fundamentals for a single bond, identified by ISIN.
        /// </summary>
        /// <param name="isin">The bond ISIN (e.g. <c>"DE000CB83CF0"</c>).</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>The bond fundamentals.</returns>
        public Task<BondFundamentals> GetBondFundamentalsAsync(string isin, CancellationToken ct = default)
        {
            if(string.IsNullOrWhiteSpace(isin))
            {
                throw new ArgumentNullException(nameof(isin));
            }

            return this.GetJsonAsync<BondFundamentals>("bond-fundamentals/" + Uri.EscapeDataString(isin), ct);
        }

        /// <summary>
        /// Returns the fundamentals for a single bond, identified by ISIN.
        /// </summary>
        /// <param name="isin">The bond ISIN (e.g. <c>"DE000CB83CF0"</c>).</param>
        /// <returns>The bond fundamentals.</returns>
        public BondFundamentals GetBondFundamentals(string isin)
        {
            return this.GetBondFundamentalsAsync(isin).GetAwaiter().GetResult();
        }

        // ================================================================
        // Earnings Trends API
        // ================================================================

        /// <summary>
        /// Returns earnings-trend records (analyst estimates, EPS trend, and revisions) for one or more
        /// symbols. The API nests one record set per symbol; the results are flattened into a single
        /// array (each record carries its own <see cref="EarningsTrend.Code"/>).
        /// </summary>
        /// <param name="symbols">The symbols to fetch trends for (e.g. <c>"AAPL.US"</c>, <c>"MSFT.US"</c>).</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>The earnings-trend records across all requested symbols.</returns>
        public async Task<EarningsTrend[]> GetEarningsTrendsAsync(IReadOnlyList<string> symbols, CancellationToken ct = default)
        {
            if(symbols == null || symbols.Count == 0)
            {
                throw new ArgumentException("At least one symbol must be supplied.", nameof(symbols));
            }

            EarningsTrendsResponse response = await this.GetJsonAsync<EarningsTrendsResponse>(
                "calendar/trends",
                ct,
                ("symbols", JoinSymbols(symbols))).ConfigureAwait(false);

            if(response?.Trends == null)
            {
                return Array.Empty<EarningsTrend>();
            }

            List<EarningsTrend> flattened = new List<EarningsTrend>();
            foreach(EarningsTrend[] perSymbol in response.Trends)
            {
                if(perSymbol != null)
                {
                    flattened.AddRange(perSymbol);
                }
            }

            return flattened.ToArray();
        }

        /// <summary>
        /// Returns earnings-trend records (analyst estimates, EPS trend, and revisions) for one or more
        /// symbols, flattened into a single array (each record carries its own
        /// <see cref="EarningsTrend.Code"/>).
        /// </summary>
        /// <param name="symbols">The symbols to fetch trends for (e.g. <c>"AAPL.US"</c>, <c>"MSFT.US"</c>).</param>
        /// <returns>The earnings-trend records across all requested symbols.</returns>
        public EarningsTrend[] GetEarningsTrends(IReadOnlyList<string> symbols)
        {
            return this.GetEarningsTrendsAsync(symbols).GetAwaiter().GetResult();
        }
    }
}
