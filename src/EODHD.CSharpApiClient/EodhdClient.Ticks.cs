using System;
using System.Threading;
using System.Threading.Tasks;

using EODHD.CSharpApiClient.DataModel.Ticks;

namespace EODHD.CSharpApiClient
{
    public sealed partial class EodhdClient
    {
        // ================================================================
        // Tick Data API (US equities)
        // ================================================================

        /// <summary>
        /// Returns historical trade-level tick data for a US equity. The API returns the ticks as parallel
        /// columns; they are transposed into one <see cref="Tick"/> per trade. Limited to US exchanges.
        /// </summary>
        /// <param name="symbol">The symbol (e.g. <c>"AAPL"</c> or <c>"AAPL.US"</c>).</param>
        /// <param name="from">Optional inclusive start time (sent as a Unix timestamp in seconds).</param>
        /// <param name="to">Optional inclusive end time (sent as a Unix timestamp in seconds).</param>
        /// <param name="limit">Optional maximum number of ticks to return.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>The ticks, one per trade.</returns>
        public async Task<Tick[]> GetTicksAsync(string symbol, DateTime? from = null, DateTime? to = null, int? limit = null, CancellationToken ct = default)
        {
            if(string.IsNullOrWhiteSpace(symbol))
            {
                throw new ArgumentNullException(nameof(symbol));
            }

            TickResponse response = await this.GetJsonAsync<TickResponse>(
                "ticks",
                ct,
                ("s", symbol),
                ("from", FormatUnix(from)),
                ("to", FormatUnix(to)),
                ("limit", FormatInt(limit))).ConfigureAwait(false);

            return ProjectTicks(response);
        }

        /// <summary>
        /// Returns historical trade-level tick data for a US equity.
        /// </summary>
        /// <param name="symbol">The symbol (e.g. <c>"AAPL"</c> or <c>"AAPL.US"</c>).</param>
        /// <param name="from">Optional inclusive start time (sent as a Unix timestamp in seconds).</param>
        /// <param name="to">Optional inclusive end time (sent as a Unix timestamp in seconds).</param>
        /// <param name="limit">Optional maximum number of ticks to return.</param>
        /// <returns>The ticks, one per trade.</returns>
        public Tick[] GetTicks(string symbol, DateTime? from = null, DateTime? to = null, int? limit = null)
        {
            return this.GetTicksAsync(symbol, from, to, limit).GetAwaiter().GetResult();
        }

        private static Tick[] ProjectTicks(TickResponse response)
        {
            if(response == null)
            {
                return Array.Empty<Tick>();
            }

            int count = 0;
            count = Math.Max(count, response.Ts?.Length ?? 0);
            count = Math.Max(count, response.Price?.Length ?? 0);
            count = Math.Max(count, response.Seq?.Length ?? 0);
            count = Math.Max(count, response.Shares?.Length ?? 0);
            count = Math.Max(count, response.Mkt?.Length ?? 0);
            count = Math.Max(count, response.SubMkt?.Length ?? 0);
            count = Math.Max(count, response.Sl?.Length ?? 0);
            count = Math.Max(count, response.Ex?.Length ?? 0);

            Tick[] ticks = new Tick[count];
            for(int i = 0; i < count; i++)
            {
                ticks[i] = new Tick
                {
                    Exchange = ItemOrNull(response.Ex, i),
                    Market = ItemOrNull(response.Mkt, i),
                    SubMarket = ItemOrNull(response.SubMkt, i),
                    Price = ItemOrNull(response.Price, i),
                    Sequence = ItemOrNull(response.Seq, i),
                    Shares = ItemOrNull(response.Shares, i),
                    SalesCondition = ItemOrNull(response.Sl, i),
                    TimestampMs = ItemOrNull(response.Ts, i),
                };
            }

            return ticks;
        }

        private static string ItemOrNull(string[] column, int index)
        {
            return column != null && index < column.Length ? column[index] : null;
        }

        private static double? ItemOrNull(double[] column, int index)
        {
            return column != null && index < column.Length ? column[index] : (double?)null;
        }

        private static long? ItemOrNull(long[] column, int index)
        {
            return column != null && index < column.Length ? column[index] : (long?)null;
        }
    }
}
