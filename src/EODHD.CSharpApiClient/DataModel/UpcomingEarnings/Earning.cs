using System;
using System.Text.Json.Serialization;

using EODHD.CSharpApiClient.JsonSupport;

namespace EODHD.CSharpApiClient.DataModel.UpcomingEarnings
{
    /// <summary>
    /// Represents a single upcoming-earnings entry for a symbol.
    /// </summary>
    public sealed class Earning
    {
        /// <summary>
        /// Gets or sets the full EODHD code (symbol and exchange).
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Gets the symbol portion of <see cref="Code"/>.
        /// </summary>
        [JsonIgnore]
        public string Symbol
        {
            get
            {
                if(this.Code == null)
                {
                    return null;
                }

                int pointIndex = this.Code.IndexOf('.');
                return pointIndex == -1 ? this.Code : this.Code.Substring(0, pointIndex);
            }
        }

        /// <summary>
        /// Gets the exchange portion of <see cref="Code"/>.
        /// </summary>
        [JsonIgnore]
        public string Exchange
        {
            get
            {
                if(this.Code == null)
                {
                    return null;
                }

                int pointIndex = this.Code.IndexOf('.');
                return pointIndex == -1 ? string.Empty : this.Code.Substring(pointIndex + 1);
            }
        }

        /// <summary>
        /// Gets or sets the report date; <c>null</c> when the API sends no valid date.
        /// </summary>
        [JsonPropertyName("Report_Date")]
        [JsonConverter(typeof(NaTolerantDateConverter))]
        public DateTime? ReportDate { get; set; }

        /// <summary>
        /// Gets or sets the fiscal period end date; <c>null</c> when the API sends no valid date.
        /// </summary>
        [JsonPropertyName("Date")]
        [JsonConverter(typeof(NaTolerantDateConverter))]
        public DateTime? Date { get; set; }

        /// <summary>
        /// Gets or sets the before/after market indicator.
        /// </summary>
        [JsonPropertyName("Before_After_Market")]
        public string BeforeAfterMarket { get; set; }

        /// <summary>
        /// Gets or sets the reporting currency.
        /// </summary>
        public string Currency { get; set; }

        /// <summary>
        /// Gets or sets the actual reported value.
        /// </summary>
        public double? Actual { get; set; }

        /// <summary>
        /// Gets or sets the estimated value.
        /// </summary>
        public double? Estimate { get; set; }

        /// <summary>
        /// Gets or sets the difference between actual and estimate.
        /// </summary>
        public double? Difference { get; set; }

        /// <summary>
        /// Gets or sets the surprise percentage.
        /// </summary>
        public double? Percent { get; set; }
    }
}
