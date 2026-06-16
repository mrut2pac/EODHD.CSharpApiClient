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
        // Calendar — Upcoming Dividends
        // ================================================================

        /// <summary>
        /// Returns the upcoming-dividends calendar (ex-dividend dates). At least one of
        /// <paramref name="symbol"/> or <paramref name="dateEqual"/> must be supplied (the API requires it).
        /// </summary>
        /// <param name="symbol">Optional symbol to filter to (e.g. <c>"AAPL.US"</c>).</param>
        /// <param name="dateEqual">Optional exact date filter. Required when <paramref name="symbol"/> is not supplied.</param>
        /// <param name="from">Optional inclusive start date.</param>
        /// <param name="to">Optional inclusive end date.</param>
        /// <param name="offset">Optional pagination offset.</param>
        /// <param name="limit">Optional page size.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>The upcoming-dividend entries.</returns>
        public async Task<UpcomingDividend[]> GetUpcomingDividendsAsync(string symbol = null, DateTime? dateEqual = null, DateTime? from = null, DateTime? to = null, int? offset = null, int? limit = null, CancellationToken ct = default)
        {
            if(string.IsNullOrWhiteSpace(symbol) && !dateEqual.HasValue)
            {
                throw new ArgumentException("Either symbol or dateEqual must be supplied.", nameof(symbol));
            }

            DataEnvelope<UpcomingDividend> response = await this.GetJsonAsync<DataEnvelope<UpcomingDividend>>(
                "calendar/dividends",
                ct,
                ("filter[symbol]", symbol),
                ("filter[date_eq]", FormatDate(dateEqual)),
                ("filter[date_from]", FormatDate(from)),
                ("filter[date_to]", FormatDate(to)),
                ("page[offset]", FormatInt(offset)),
                ("page[limit]", FormatInt(limit))).ConfigureAwait(false);

            return response?.Data ?? Array.Empty<UpcomingDividend>();
        }

        /// <summary>
        /// Returns the upcoming-dividends calendar (ex-dividend dates). At least one of
        /// <paramref name="symbol"/> or <paramref name="dateEqual"/> must be supplied.
        /// </summary>
        /// <param name="symbol">Optional symbol to filter to (e.g. <c>"AAPL.US"</c>).</param>
        /// <param name="dateEqual">Optional exact date filter. Required when <paramref name="symbol"/> is not supplied.</param>
        /// <param name="from">Optional inclusive start date.</param>
        /// <param name="to">Optional inclusive end date.</param>
        /// <param name="offset">Optional pagination offset.</param>
        /// <param name="limit">Optional page size.</param>
        /// <returns>The upcoming-dividend entries.</returns>
        public UpcomingDividend[] GetUpcomingDividends(string symbol = null, DateTime? dateEqual = null, DateTime? from = null, DateTime? to = null, int? offset = null, int? limit = null)
        {
            return this.GetUpcomingDividendsAsync(symbol, dateEqual, from, to, offset, limit).GetAwaiter().GetResult();
        }
    }
}
