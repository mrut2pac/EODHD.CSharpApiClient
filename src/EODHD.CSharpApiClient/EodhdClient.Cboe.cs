using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using EODHD.CSharpApiClient.DataModel.Cboe;

namespace EODHD.CSharpApiClient
{
    public sealed partial class EodhdClient
    {
        // ================================================================
        // Cboe Indices (marketplace)
        // ================================================================

        /// <summary>
        /// Returns the list of available Cboe index snapshots (index-level fields only, without
        /// constituents). This endpoint requires the separate Cboe marketplace subscription.
        /// </summary>
        /// <param name="offset">Optional pagination offset.</param>
        /// <param name="limit">Optional page size.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>The available index snapshots.</returns>
        public async Task<CboeIndex[]> GetCboeIndicesAsync(int? offset = null, int? limit = null, CancellationToken ct = default)
        {
            CboeIndexResponse response = await this.GetJsonAsync<CboeIndexResponse>(
                "cboe/indices",
                ct,
                ("page[offset]", FormatInt(offset)),
                ("page[limit]", FormatInt(limit))).ConfigureAwait(false);

            return FlattenCboeIndices(response);
        }

        /// <summary>
        /// Returns the list of available Cboe index snapshots (index-level fields only, without constituents).
        /// </summary>
        /// <param name="offset">Optional pagination offset.</param>
        /// <param name="limit">Optional page size.</param>
        /// <returns>The available index snapshots.</returns>
        public CboeIndex[] GetCboeIndices(int? offset = null, int? limit = null)
        {
            return this.GetCboeIndicesAsync(offset, limit).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Returns a single Cboe index snapshot, including its constituent components. All three filters
        /// are required by the API. This endpoint requires the separate Cboe marketplace subscription.
        /// </summary>
        /// <param name="indexCode">The index code (e.g. <c>"BNL30N"</c>).</param>
        /// <param name="feedType">The feed type (e.g. <c>"snapshot_official_opening"</c>, <c>"snapshot_official_closing"</c>).</param>
        /// <param name="date">The snapshot date.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>The matching index snapshot with its components, or <c>null</c> if none was found.</returns>
        public async Task<CboeIndex> GetCboeIndexAsync(string indexCode, string feedType, DateTime date, CancellationToken ct = default)
        {
            if(string.IsNullOrWhiteSpace(indexCode))
            {
                throw new ArgumentException("An index code must be supplied.", nameof(indexCode));
            }

            if(string.IsNullOrWhiteSpace(feedType))
            {
                throw new ArgumentException("A feed type must be supplied.", nameof(feedType));
            }

            if(date == default)
            {
                throw new ArgumentException("A snapshot date must be supplied.", nameof(date));
            }

            CboeIndexResponse response = await this.GetJsonAsync<CboeIndexResponse>(
                "cboe/index",
                ct,
                ("filter[index_code]", indexCode),
                ("filter[feed_type]", feedType),
                ("filter[date]", FormatDate(date))).ConfigureAwait(false);

            CboeIndex[] indices = FlattenCboeIndices(response);
            return indices.Length > 0 ? indices[0] : null;
        }

        /// <summary>
        /// Returns a single Cboe index snapshot, including its constituent components.
        /// </summary>
        /// <param name="indexCode">The index code (e.g. <c>"BNL30N"</c>).</param>
        /// <param name="feedType">The feed type (e.g. <c>"snapshot_official_opening"</c>, <c>"snapshot_official_closing"</c>).</param>
        /// <param name="date">The snapshot date.</param>
        /// <returns>The matching index snapshot with its components, or <c>null</c> if none was found.</returns>
        public CboeIndex GetCboeIndex(string indexCode, string feedType, DateTime date)
        {
            return this.GetCboeIndexAsync(indexCode, feedType, date).GetAwaiter().GetResult();
        }

        private static CboeIndex[] FlattenCboeIndices(CboeIndexResponse response)
        {
            if(response?.Data == null)
            {
                return Array.Empty<CboeIndex>();
            }

            List<CboeIndex> result = new List<CboeIndex>(response.Data.Length);
            foreach(CboeIndexResource resource in response.Data)
            {
                if(resource?.Attributes == null)
                {
                    continue;
                }

                CboeIndex index = resource.Attributes;
                if(resource.Components != null)
                {
                    List<CboeIndexComponent> components = new List<CboeIndexComponent>(resource.Components.Length);
                    foreach(CboeIndexComponentResource component in resource.Components)
                    {
                        if(component?.Attributes != null)
                        {
                            components.Add(component.Attributes);
                        }
                    }

                    index.Components = components.ToArray();
                }

                result.Add(index);
            }

            return result.ToArray();
        }
    }
}
