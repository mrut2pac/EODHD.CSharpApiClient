using System;

namespace EODHD.CSharpApiClient.DataModel.EarningTrends
{
    /// <summary>
    /// Represents a single earning-trend data point for a symbol and period.
    /// </summary>
    public sealed class Trend
    {
        /// <summary>
        /// Gets or sets the symbol code.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets the trend date.
        /// </summary>
        public DateTime? Date { get; set; }

        /// <summary>
        /// Gets or sets the reporting period.
        /// </summary>
        public string Period { get; set; }

        /// <summary>
        /// Gets or sets the growth estimate.
        /// </summary>
        public double? Growth { get; set; }

        /// <summary>
        /// Gets or sets the average earnings estimate.
        /// </summary>
        public double? EarningsEstimateAvg { get; set; }

        /// <summary>
        /// Gets or sets the low earnings estimate.
        /// </summary>
        public double? EarningsEstimateLow { get; set; }

        /// <summary>
        /// Gets or sets the high earnings estimate.
        /// </summary>
        public double? EarningsEstimateHigh { get; set; }

        /// <summary>
        /// Gets or sets the year-ago earnings-per-share estimate.
        /// </summary>
        public double? EarningsEstimateYearAgoEps { get; set; }

        /// <summary>
        /// Gets or sets the number of analysts behind the earnings estimate.
        /// </summary>
        public double? EarningsEstimateNumberOfAnalysts { get; set; }

        /// <summary>
        /// Gets or sets the earnings-estimate growth.
        /// </summary>
        public double? EarningsEstimateGrowth { get; set; }

        /// <summary>
        /// Gets or sets the average revenue estimate.
        /// </summary>
        public decimal? RevenueEstimateAvg { get; set; }

        /// <summary>
        /// Gets or sets the low revenue estimate.
        /// </summary>
        public decimal? RevenueEstimateLow { get; set; }

        /// <summary>
        /// Gets or sets the high revenue estimate.
        /// </summary>
        public decimal? RevenueEstimateHigh { get; set; }

        /// <summary>
        /// Gets or sets the year-ago revenue-per-share estimate.
        /// </summary>
        public double? RevenueEstimateYearAgoEps { get; set; }

        /// <summary>
        /// Gets or sets the number of analysts behind the revenue estimate.
        /// </summary>
        public double? RevenueEstimateNumberOfAnalysts { get; set; }

        /// <summary>
        /// Gets or sets the revenue-estimate growth.
        /// </summary>
        public double? RevenueEstimateGrowth { get; set; }

        /// <summary>
        /// Gets or sets the current earnings-per-share trend.
        /// </summary>
        public double? EpsTrendCurrent { get; set; }

        /// <summary>
        /// Gets or sets the earnings-per-share trend 7 days ago.
        /// </summary>
        public double? EpsTrend7daysAgo { get; set; }

        /// <summary>
        /// Gets or sets the earnings-per-share trend 30 days ago.
        /// </summary>
        public double? EpsTrend30daysAgo { get; set; }

        /// <summary>
        /// Gets or sets the earnings-per-share trend 60 days ago.
        /// </summary>
        public double? EpsTrend60daysAgo { get; set; }

        /// <summary>
        /// Gets or sets the earnings-per-share trend 90 days ago.
        /// </summary>
        public double? EpsTrend90daysAgo { get; set; }

        /// <summary>
        /// Gets or sets the number of upward earnings-per-share revisions in the last 7 days.
        /// </summary>
        public double? EpsRevisionsUpLast7days { get; set; }

        /// <summary>
        /// Gets or sets the number of upward earnings-per-share revisions in the last 30 days.
        /// </summary>
        public double? EpsRevisionsUpLast30days { get; set; }

        /// <summary>
        /// Gets or sets the number of downward earnings-per-share revisions in the last 7 days.
        /// </summary>
        public double? EpsRevisionsDownLast7days { get; set; }

        /// <summary>
        /// Gets or sets the number of downward earnings-per-share revisions in the last 30 days.
        /// </summary>
        public double? EpsRevisionsDownLast30days { get; set; }
    }
}
