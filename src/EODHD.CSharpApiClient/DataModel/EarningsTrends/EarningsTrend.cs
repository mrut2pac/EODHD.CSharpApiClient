using System;
using System.Text.Json.Serialization;

namespace EODHD.CSharpApiClient.DataModel.EarningsTrends
{
    /// <summary>
    /// A single earnings-trend record for a symbol and forecast period: analyst earnings/revenue
    /// estimates, the EPS estimate trend over time, and recent estimate revisions.
    /// </summary>
    public sealed class EarningsTrend
    {
        /// <summary>
        /// Gets or sets the symbol (e.g. <c>"AAPL.US"</c>).
        /// </summary>
        [JsonPropertyName("code")]
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets the period-end date.
        /// </summary>
        [JsonPropertyName("date")]
        public DateTime? Date { get; set; }

        /// <summary>
        /// Gets or sets the forecast period relative to now (e.g. <c>"0q"</c> current quarter,
        /// <c>"+1q"</c> next quarter, <c>"0y"</c> current fiscal year, <c>"+1y"</c> next fiscal year).
        /// </summary>
        [JsonPropertyName("period")]
        public string Period { get; set; }

        /// <summary>
        /// Gets or sets the estimated growth.
        /// </summary>
        [JsonPropertyName("growth")]
        public double? Growth { get; set; }

        /// <summary>
        /// Gets or sets the average earnings (EPS) estimate.
        /// </summary>
        [JsonPropertyName("earningsEstimateAvg")]
        public double? EarningsEstimateAvg { get; set; }

        /// <summary>
        /// Gets or sets the lowest earnings (EPS) estimate.
        /// </summary>
        [JsonPropertyName("earningsEstimateLow")]
        public double? EarningsEstimateLow { get; set; }

        /// <summary>
        /// Gets or sets the highest earnings (EPS) estimate.
        /// </summary>
        [JsonPropertyName("earningsEstimateHigh")]
        public double? EarningsEstimateHigh { get; set; }

        /// <summary>
        /// Gets or sets the year-ago earnings per share.
        /// </summary>
        [JsonPropertyName("earningsEstimateYearAgoEps")]
        public double? EarningsEstimateYearAgoEps { get; set; }

        /// <summary>
        /// Gets or sets the number of analysts contributing earnings estimates.
        /// </summary>
        [JsonPropertyName("earningsEstimateNumberOfAnalysts")]
        public double? EarningsEstimateNumberOfAnalysts { get; set; }

        /// <summary>
        /// Gets or sets the estimated earnings growth.
        /// </summary>
        [JsonPropertyName("earningsEstimateGrowth")]
        public double? EarningsEstimateGrowth { get; set; }

        /// <summary>
        /// Gets or sets the average revenue estimate.
        /// </summary>
        [JsonPropertyName("revenueEstimateAvg")]
        public double? RevenueEstimateAvg { get; set; }

        /// <summary>
        /// Gets or sets the lowest revenue estimate.
        /// </summary>
        [JsonPropertyName("revenueEstimateLow")]
        public double? RevenueEstimateLow { get; set; }

        /// <summary>
        /// Gets or sets the highest revenue estimate.
        /// </summary>
        [JsonPropertyName("revenueEstimateHigh")]
        public double? RevenueEstimateHigh { get; set; }

        /// <summary>
        /// Gets or sets the year-ago revenue figure. (EODHD names this wire field
        /// <c>revenueEstimateYearAgoEps</c> even though it carries revenue, not EPS.)
        /// </summary>
        [JsonPropertyName("revenueEstimateYearAgoEps")]
        public double? RevenueEstimateYearAgoEps { get; set; }

        /// <summary>
        /// Gets or sets the number of analysts contributing revenue estimates.
        /// </summary>
        [JsonPropertyName("revenueEstimateNumberOfAnalysts")]
        public double? RevenueEstimateNumberOfAnalysts { get; set; }

        /// <summary>
        /// Gets or sets the estimated revenue growth.
        /// </summary>
        [JsonPropertyName("revenueEstimateGrowth")]
        public double? RevenueEstimateGrowth { get; set; }

        /// <summary>
        /// Gets or sets the current EPS estimate.
        /// </summary>
        [JsonPropertyName("epsTrendCurrent")]
        public double? EpsTrendCurrent { get; set; }

        /// <summary>
        /// Gets or sets the EPS estimate as of 7 days ago.
        /// </summary>
        [JsonPropertyName("epsTrend7daysAgo")]
        public double? EpsTrend7DaysAgo { get; set; }

        /// <summary>
        /// Gets or sets the EPS estimate as of 30 days ago.
        /// </summary>
        [JsonPropertyName("epsTrend30daysAgo")]
        public double? EpsTrend30DaysAgo { get; set; }

        /// <summary>
        /// Gets or sets the EPS estimate as of 60 days ago.
        /// </summary>
        [JsonPropertyName("epsTrend60daysAgo")]
        public double? EpsTrend60DaysAgo { get; set; }

        /// <summary>
        /// Gets or sets the EPS estimate as of 90 days ago.
        /// </summary>
        [JsonPropertyName("epsTrend90daysAgo")]
        public double? EpsTrend90DaysAgo { get; set; }

        /// <summary>
        /// Gets or sets the number of upward EPS revisions in the last 7 days.
        /// </summary>
        [JsonPropertyName("epsRevisionsUpLast7days")]
        public double? EpsRevisionsUpLast7Days { get; set; }

        /// <summary>
        /// Gets or sets the number of upward EPS revisions in the last 30 days.
        /// </summary>
        [JsonPropertyName("epsRevisionsUpLast30days")]
        public double? EpsRevisionsUpLast30Days { get; set; }

        /// <summary>
        /// Gets or sets the number of downward EPS revisions in the last 30 days.
        /// </summary>
        [JsonPropertyName("epsRevisionsDownLast30days")]
        public double? EpsRevisionsDownLast30Days { get; set; }
    }
}
