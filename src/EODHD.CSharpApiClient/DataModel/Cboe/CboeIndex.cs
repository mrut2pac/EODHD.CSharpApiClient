using System.Text.Json.Serialization;

namespace EODHD.CSharpApiClient.DataModel.Cboe
{
    /// <summary>
    /// A Cboe index snapshot. The list endpoint returns the index-level fields only; the single-index
    /// endpoint additionally populates <see cref="EffectiveDate"/>, <see cref="ReviewDate"/>, and the
    /// constituent <see cref="Components"/>.
    /// </summary>
    public sealed class CboeIndex
    {
        /// <summary>Gets or sets the region the index belongs to (e.g. <c>"United States"</c>, <c>"Germany"</c>).</summary>
        [JsonPropertyName("region")]
        public string Region { get; set; }

        /// <summary>Gets or sets the index code (e.g. <c>"BNL30N"</c>).</summary>
        [JsonPropertyName("index_code")]
        public string IndexCode { get; set; }

        /// <summary>Gets or sets the feed type (e.g. <c>"snapshot_official_opening"</c>, <c>"snapshot_official_closing"</c>).</summary>
        [JsonPropertyName("feed_type")]
        public string FeedType { get; set; }

        /// <summary>Gets or sets the snapshot date.</summary>
        [JsonPropertyName("date")]
        public string Date { get; set; }

        /// <summary>Gets or sets the index close value.</summary>
        [JsonPropertyName("index_close")]
        public double? IndexClose { get; set; }

        /// <summary>Gets or sets the index divisor.</summary>
        [JsonPropertyName("index_divisor")]
        public double? IndexDivisor { get; set; }

        /// <summary>Gets or sets the effective date (single-index endpoint only).</summary>
        [JsonPropertyName("effective_date")]
        public string EffectiveDate { get; set; }

        /// <summary>Gets or sets the review date (single-index endpoint only).</summary>
        [JsonPropertyName("review_date")]
        public string ReviewDate { get; set; }

        /// <summary>
        /// Gets or sets the index constituents. Populated only by the single-index endpoint
        /// (<see cref="EodhdClient.GetCboeIndexAsync"/>); <c>null</c> for the list endpoint.
        /// </summary>
        public CboeIndexComponent[] Components { get; set; }
    }
}
