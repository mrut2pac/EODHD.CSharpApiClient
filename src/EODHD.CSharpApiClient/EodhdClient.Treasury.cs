using System;
using System.Threading;
using System.Threading.Tasks;

using EODHD.CSharpApiClient.DataModel;
using EODHD.CSharpApiClient.DataModel.Treasury;

namespace EODHD.CSharpApiClient
{
    public sealed partial class EodhdClient
    {
        // ================================================================
        // US Treasury Rates API
        // ================================================================

        /// <summary>
        /// Returns the daily US Treasury par yield curve rates. When <paramref name="year"/> is omitted,
        /// the API returns the current year.
        /// </summary>
        /// <param name="year">Optional calendar year to fetch (e.g. <c>2024</c>).</param>
        /// <param name="offset">Optional pagination offset.</param>
        /// <param name="limit">Optional page size.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>The Treasury yield-curve rates.</returns>
        public Task<TreasuryRate[]> GetTreasuryYieldRatesAsync(int? year = null, int? offset = null, int? limit = null, CancellationToken ct = default)
        {
            return this.GetTreasuryRatesAsync<TreasuryRate>("ust/yield-rates", year, offset, limit, ct);
        }

        /// <summary>
        /// Returns the daily US Treasury par yield curve rates.
        /// </summary>
        /// <param name="year">Optional calendar year to fetch (e.g. <c>2024</c>).</param>
        /// <param name="offset">Optional pagination offset.</param>
        /// <param name="limit">Optional page size.</param>
        /// <returns>The Treasury yield-curve rates.</returns>
        public TreasuryRate[] GetTreasuryYieldRates(int? year = null, int? offset = null, int? limit = null)
        {
            return this.GetTreasuryYieldRatesAsync(year, offset, limit).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Returns the daily US Treasury real (inflation-indexed) yield curve rates. When
        /// <paramref name="year"/> is omitted, the API returns the current year.
        /// </summary>
        /// <param name="year">Optional calendar year to fetch (e.g. <c>2024</c>).</param>
        /// <param name="offset">Optional pagination offset.</param>
        /// <param name="limit">Optional page size.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>The Treasury real yield-curve rates.</returns>
        public Task<TreasuryRate[]> GetTreasuryRealYieldRatesAsync(int? year = null, int? offset = null, int? limit = null, CancellationToken ct = default)
        {
            return this.GetTreasuryRatesAsync<TreasuryRate>("ust/real-yield-rates", year, offset, limit, ct);
        }

        /// <summary>
        /// Returns the daily US Treasury real (inflation-indexed) yield curve rates.
        /// </summary>
        /// <param name="year">Optional calendar year to fetch (e.g. <c>2024</c>).</param>
        /// <param name="offset">Optional pagination offset.</param>
        /// <param name="limit">Optional page size.</param>
        /// <returns>The Treasury real yield-curve rates.</returns>
        public TreasuryRate[] GetTreasuryRealYieldRates(int? year = null, int? offset = null, int? limit = null)
        {
            return this.GetTreasuryRealYieldRatesAsync(year, offset, limit).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Returns the daily US Treasury bill rates (discount and coupon-equivalent). When
        /// <paramref name="year"/> is omitted, the API returns the current year.
        /// </summary>
        /// <param name="year">Optional calendar year to fetch (e.g. <c>2024</c>).</param>
        /// <param name="offset">Optional pagination offset.</param>
        /// <param name="limit">Optional page size.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>The Treasury bill rates.</returns>
        public Task<TreasuryBillRate[]> GetTreasuryBillRatesAsync(int? year = null, int? offset = null, int? limit = null, CancellationToken ct = default)
        {
            return this.GetTreasuryRatesAsync<TreasuryBillRate>("ust/bill-rates", year, offset, limit, ct);
        }

        /// <summary>
        /// Returns the daily US Treasury bill rates (discount and coupon-equivalent).
        /// </summary>
        /// <param name="year">Optional calendar year to fetch (e.g. <c>2024</c>).</param>
        /// <param name="offset">Optional pagination offset.</param>
        /// <param name="limit">Optional page size.</param>
        /// <returns>The Treasury bill rates.</returns>
        public TreasuryBillRate[] GetTreasuryBillRates(int? year = null, int? offset = null, int? limit = null)
        {
            return this.GetTreasuryBillRatesAsync(year, offset, limit).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Returns the daily US Treasury long-term rates. When <paramref name="year"/> is omitted, the API
        /// returns the current year.
        /// </summary>
        /// <param name="year">Optional calendar year to fetch (e.g. <c>2024</c>).</param>
        /// <param name="offset">Optional pagination offset.</param>
        /// <param name="limit">Optional page size.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>The Treasury long-term rates.</returns>
        public Task<TreasuryLongTermRate[]> GetTreasuryLongTermRatesAsync(int? year = null, int? offset = null, int? limit = null, CancellationToken ct = default)
        {
            return this.GetTreasuryRatesAsync<TreasuryLongTermRate>("ust/long-term-rates", year, offset, limit, ct);
        }

        /// <summary>
        /// Returns the daily US Treasury long-term rates.
        /// </summary>
        /// <param name="year">Optional calendar year to fetch (e.g. <c>2024</c>).</param>
        /// <param name="offset">Optional pagination offset.</param>
        /// <param name="limit">Optional page size.</param>
        /// <returns>The Treasury long-term rates.</returns>
        public TreasuryLongTermRate[] GetTreasuryLongTermRates(int? year = null, int? offset = null, int? limit = null)
        {
            return this.GetTreasuryLongTermRatesAsync(year, offset, limit).GetAwaiter().GetResult();
        }

        private async Task<T[]> GetTreasuryRatesAsync<T>(string path, int? year, int? offset, int? limit, CancellationToken ct)
        {
            DataEnvelope<T> response = await this.GetJsonAsync<DataEnvelope<T>>(
                path,
                ct,
                ("filter[year]", FormatInt(year)),
                ("page[offset]", FormatInt(offset)),
                ("page[limit]", FormatInt(limit))).ConfigureAwait(false);

            return response?.Data ?? Array.Empty<T>();
        }
    }
}
