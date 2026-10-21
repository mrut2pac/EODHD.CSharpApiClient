using System;
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

        /// <summary>Gets or sets the raw wire value of the snapshot date.</summary>
        [JsonInclude]
        [JsonPropertyName("date")]
        private string DateStr { get; set; }

        /// <summary>Gets the snapshot date, parsed from the raw value.</summary>
        [JsonIgnore]
        public DateTime? Date => Utilities.ParseDate(this.DateStr);

        /// <summary>Gets or sets the index close value.</summary>
        [JsonPropertyName("index_close")]
        public double? IndexClose { get; set; }

        /// <summary>Gets or sets the index divisor.</summary>
        [JsonPropertyName("index_divisor")]
        public double? IndexDivisor { get; set; }

        /// <summary>Gets or sets the raw wire value of the effective date.</summary>
        [JsonInclude]
        [JsonPropertyName("effective_date")]
        private string EffectiveDateStr { get; set; }

        /// <summary>Gets the effective date (single-index endpoint only), parsed from the raw value.</summary>
        [JsonIgnore]
        public DateTime? EffectiveDate => Utilities.ParseDate(this.EffectiveDateStr);

        /// <summary>Gets or sets the raw wire value of the review date.</summary>
        [JsonInclude]
        [JsonPropertyName("review_date")]
        private string ReviewDateStr { get; set; }

        /// <summary>Gets the review date (single-index endpoint only), parsed from the raw value.</summary>
        [JsonIgnore]
        public DateTime? ReviewDate => Utilities.ParseDate(this.ReviewDateStr);

        /// <summary>
        /// Gets or sets the index constituents. Populated only by the single-index endpoint
        /// (<see cref="EodhdClient.GetCboeIndexAsync"/>); <c>null</c> for the list endpoint.
        /// </summary>
        public CboeIndexComponent[] Components { get; set; }
    }
}
