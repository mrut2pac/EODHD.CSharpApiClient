using System;
using System.Text.Json.Serialization;

namespace EODHD.CSharpApiClient.DataModel.Treasury
{
    /// <summary>
    /// A US Treasury bill rate for a single date and tenor, including discount and coupon-equivalent rates.
    /// </summary>
    public sealed class TreasuryBillRate
    {
        /// <summary>
        /// Gets or sets the observation date.
        /// </summary>
        [JsonPropertyName("date")]
        public DateTime? Date { get; set; }

        /// <summary>
        /// Gets or sets the tenor (e.g. <c>"4WK"</c>, <c>"13WK"</c>, <c>"52WK"</c>).
        /// </summary>
        [JsonPropertyName("tenor")]
        public string Tenor { get; set; }

        /// <summary>
        /// Gets or sets the bank discount rate, percent.
        /// </summary>
        [JsonPropertyName("discount")]
        public double? Discount { get; set; }

        /// <summary>
        /// Gets or sets the coupon-equivalent rate, percent.
        /// </summary>
        [JsonPropertyName("coupon")]
        public double? Coupon { get; set; }

        /// <summary>
        /// Gets or sets the average bank discount rate, percent.
        /// </summary>
        [JsonPropertyName("avg_discount")]
        public double? AverageDiscount { get; set; }

        /// <summary>
        /// Gets or sets the average coupon-equivalent rate, percent.
        /// </summary>
        [JsonPropertyName("avg_coupon")]
        public double? AverageCoupon { get; set; }

        /// <summary>
        /// Gets or sets the bill maturity date.
        /// </summary>
        [JsonPropertyName("maturity_date")]
        public DateTime? MaturityDate { get; set; }

        /// <summary>
        /// Gets or sets the CUSIP identifier.
        /// </summary>
        [JsonPropertyName("cusip")]
        public string Cusip { get; set; }
    }
}
