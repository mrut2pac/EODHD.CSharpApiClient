using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;

using EODHD.CSharpApiClient.DataModel;
using EODHD.CSharpApiClient.DataModel.Commodities;
using EODHD.CSharpApiClient.DataModel.Quotes;

namespace EODHD.CSharpApiClient
{
    public sealed partial class EodhdClient
    {
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
        // Commodities API
        // ================================================================

        /// <summary>
        /// Returns historical prices for a commodity.
        /// </summary>
        /// <param name="code">The commodity code (e.g. <c>"BRENT"</c>, <c>"WTI"</c>, <c>"GOLD"</c>, <c>"NATURAL_GAS"</c>).</param>
        /// <param name="interval">Optional aggregation interval (e.g. <c>"monthly"</c>, <c>"daily"</c>).</param>
        /// <param name="from">Optional inclusive start date.</param>
        /// <param name="to">Optional inclusive end date.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>The historical commodity prices.</returns>
        public async Task<CommodityPrice[]> GetCommodityHistoricalPricesAsync(string code, string interval = null, DateTime? from = null, DateTime? to = null, CancellationToken ct = default)
        {
            if(string.IsNullOrWhiteSpace(code))
            {
                throw new ArgumentNullException(nameof(code));
            }

            DataEnvelope<CommodityPrice> response = await this.GetJsonAsync<DataEnvelope<CommodityPrice>>(
                "commodities/historical/" + Uri.EscapeDataString(code),
                ct,
                ("interval", interval),
                ("from", FormatDate(from)),
                ("to", FormatDate(to))).ConfigureAwait(false);

            return response?.Data ?? Array.Empty<CommodityPrice>();
        }

        /// <summary>
        /// Returns historical prices for a commodity.
        /// </summary>
        /// <param name="code">The commodity code (e.g. <c>"BRENT"</c>, <c>"WTI"</c>, <c>"GOLD"</c>, <c>"NATURAL_GAS"</c>).</param>
        /// <param name="interval">Optional aggregation interval (e.g. <c>"monthly"</c>, <c>"daily"</c>).</param>
        /// <param name="from">Optional inclusive start date.</param>
        /// <param name="to">Optional inclusive end date.</param>
        /// <returns>The historical commodity prices.</returns>
        public CommodityPrice[] GetCommodityHistoricalPrices(string code, string interval = null, DateTime? from = null, DateTime? to = null)
        {
            return this.GetCommodityHistoricalPricesAsync(code, interval, from, to).GetAwaiter().GetResult();
        }

        // ================================================================
        // US Delayed Quote API
        // ================================================================

        /// <summary>
        /// Returns delayed US stock quotes for one or more symbols, keyed by symbol code.
        /// </summary>
        /// <param name="symbols">The symbols to quote (e.g. <c>"AAPL"</c>, <c>"MSFT"</c>).</param>
        /// <param name="fields">Optional comma-separated list of fields to restrict the response to.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A dictionary of quotes keyed by symbol code.</returns>
        public async Task<Dictionary<string, UsDelayedQuote>> GetUsDelayedQuotesAsync(IReadOnlyList<string> symbols, string fields = null, CancellationToken ct = default)
        {
            if(symbols == null || symbols.Count == 0)
            {
                throw new ArgumentException("At least one symbol must be supplied.", nameof(symbols));
            }

            UsQuoteResponse response = await this.GetJsonAsync<UsQuoteResponse>(
                "us-quote-delayed",
                ct,
                ("s", JoinSymbols(symbols)),
                ("fields", fields)).ConfigureAwait(false);

            return response?.Data ?? new Dictionary<string, UsDelayedQuote>();
        }

        /// <summary>
        /// Returns delayed US stock quotes for one or more symbols, keyed by symbol code.
        /// </summary>
        /// <param name="symbols">The symbols to quote (e.g. <c>"AAPL"</c>, <c>"MSFT"</c>).</param>
        /// <param name="fields">Optional comma-separated list of fields to restrict the response to.</param>
        /// <returns>A dictionary of quotes keyed by symbol code.</returns>
        public Dictionary<string, UsDelayedQuote> GetUsDelayedQuotes(IReadOnlyList<string> symbols, string fields = null)
        {
            return this.GetUsDelayedQuotesAsync(symbols, fields).GetAwaiter().GetResult();
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
    }
}
