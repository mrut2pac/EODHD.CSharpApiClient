using System.Text.Json.Serialization;

namespace EODHD.CSharpApiClient.DataModel.Screener
{
    /// <summary>
    /// A single instrument returned by the EODHD stock-market screener.
    /// </summary>
    public sealed class ScreenerResult
    {
        /// <summary>
        /// Gets or sets the ticker symbol.
        /// </summary>
        [JsonPropertyName("code")]
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets the company name.
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the date of the most recent end-of-day data point.
        /// </summary>
        [JsonPropertyName("last_day_data_date")]
        public string LastDayDataDate { get; set; }

        /// <summary>
        /// Gets or sets the latest adjusted closing price.
        /// </summary>
        [JsonPropertyName("adjusted_close")]
        public double? AdjustedClose { get; set; }

        /// <summary>
        /// Gets or sets the one-day absolute price change.
        /// </summary>
        [JsonPropertyName("refund_1d")]
        public double? Refund1D { get; set; }

        /// <summary>
        /// Gets or sets the one-day percentage price change.
        /// </summary>
        [JsonPropertyName("refund_1d_p")]
        public double? Refund1DPercent { get; set; }

        /// <summary>
        /// Gets or sets the five-day absolute price change.
        /// </summary>
        [JsonPropertyName("refund_5d")]
        public double? Refund5D { get; set; }

        /// <summary>
        /// Gets or sets the five-day percentage price change.
        /// </summary>
        [JsonPropertyName("refund_5d_p")]
        public double? Refund5DPercent { get; set; }

        /// <summary>
        /// Gets or sets the exchange listing code.
        /// </summary>
        [JsonPropertyName("exchange")]
        public string Exchange { get; set; }

        /// <summary>
        /// Gets or sets the trading-currency symbol (e.g. <c>"$"</c>, <c>"€"</c>).
        /// </summary>
        [JsonPropertyName("currency_symbol")]
        public string CurrencySymbol { get; set; }

        /// <summary>
        /// Gets or sets the market capitalisation.
        /// </summary>
        [JsonPropertyName("market_capitalization")]
        public decimal? MarketCapitalization { get; set; }

        /// <summary>
        /// Gets or sets the earnings per share.
        /// </summary>
        [JsonPropertyName("earnings_share")]
        public double? EarningsShare { get; set; }

        /// <summary>
        /// Gets or sets the dividend yield (as a fraction, e.g. <c>0.0106</c> for 1.06%).
        /// </summary>
        [JsonPropertyName("dividend_yield")]
        public double? DividendYield { get; set; }

        /// <summary>
        /// Gets or sets the sector classification.
        /// </summary>
        [JsonPropertyName("sector")]
        public string Sector { get; set; }

        /// <summary>
        /// Gets or sets the industry classification.
        /// </summary>
        [JsonPropertyName("industry")]
        public string Industry { get; set; }

        /// <summary>
        /// Gets or sets the one-day average trading volume.
        /// </summary>
        [JsonPropertyName("avgvol_1d")]
        public double? AverageVolume1D { get; set; }

        /// <summary>
        /// Gets or sets the 200-day average trading volume.
        /// </summary>
        [JsonPropertyName("avgvol_200d")]
        public double? AverageVolume200D { get; set; }
    }
}
