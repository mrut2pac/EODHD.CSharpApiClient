using System.Text.Json.Serialization;

namespace EODHD.CSharpApiClient.DataModel.Fundamental
{
    /// <summary>
    /// Represents ETF performance and risk metrics over standard horizons.
    /// </summary>
    public sealed class Performance
    {
        /// <summary>
        /// Gets or sets the 1-year volatility (percent).
        /// </summary>
        [JsonPropertyName("1y_Volatility")]
        public double? Volatility1Y { get; set; }

        /// <summary>
        /// Gets or sets the 3-year volatility (percent).
        /// </summary>
        [JsonPropertyName("3y_Volatility")]
        public double? Volatility3Y { get; set; }

        /// <summary>
        /// Gets or sets the 3-year expected return (percent).
        /// </summary>
        [JsonPropertyName("3y_ExpReturn")]
        public double? ExpectedReturn3Y { get; set; }

        /// <summary>
        /// Gets or sets the 3-year Sharpe ratio.
        /// </summary>
        [JsonPropertyName("3y_SharpRatio")]
        public double? SharpRatio3Y { get; set; }

        /// <summary>
        /// Gets or sets the year-to-date return (percent).
        /// </summary>
        [JsonPropertyName("Returns_YTD")]
        public double? ReturnsYTD { get; set; }

        /// <summary>
        /// Gets or sets the 1-year return (percent).
        /// </summary>
        [JsonPropertyName("Returns_1Y")]
        public double? Returns1Y { get; set; }

        /// <summary>
        /// Gets or sets the 3-year return (percent).
        /// </summary>
        [JsonPropertyName("Returns_3Y")]
        public double? Returns3Y { get; set; }

        /// <summary>
        /// Gets or sets the 5-year return (percent).
        /// </summary>
        [JsonPropertyName("Returns_5Y")]
        public double? Returns5Y { get; set; }

        /// <summary>
        /// Gets or sets the 10-year return (percent).
        /// </summary>
        [JsonPropertyName("Returns_10Y")]
        public double? Returns10Y { get; set; }
    }
}
