using System.Text.Json.Serialization;

namespace EODHD.CSharpApiClient.DataModel.Options
{
    /// <summary>
    /// A single option record from the marketplace options API (end-of-day prices or contract snapshot):
    /// identification, OHLC, bid/ask, volume and open interest, volatility, and greeks. Timestamp fields
    /// are exposed as raw strings because the API mixes formats (ISO-8601 and space-separated) and uses
    /// nulls for unavailable values.
    /// </summary>
    public sealed class OptionData
    {
        /// <summary>
        /// Gets or sets the OCC contract symbol.
        /// </summary>
        [JsonPropertyName("contract")]
        public string Contract { get; set; }

        /// <summary>
        /// Gets or sets the underlying symbol.
        /// </summary>
        [JsonPropertyName("underlying_symbol")]
        public string UnderlyingSymbol { get; set; }

        /// <summary>
        /// Gets or sets the expiration date (<c>yyyy-MM-dd</c>).
        /// </summary>
        [JsonPropertyName("exp_date")]
        public string ExpirationDate { get; set; }

        /// <summary>
        /// Gets or sets the expiration type (e.g. <c>"monthly"</c>, <c>"weekly"</c>).
        /// </summary>
        [JsonPropertyName("expiration_type")]
        public string ExpirationType { get; set; }

        /// <summary>
        /// Gets or sets the option type (<c>"call"</c> or <c>"put"</c>).
        /// </summary>
        [JsonPropertyName("type")]
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the strike price.
        /// </summary>
        [JsonPropertyName("strike")]
        public double? Strike { get; set; }

        /// <summary>
        /// Gets or sets the exchange.
        /// </summary>
        [JsonPropertyName("exchange")]
        public string Exchange { get; set; }

        /// <summary>
        /// Gets or sets the trading currency.
        /// </summary>
        [JsonPropertyName("currency")]
        public string Currency { get; set; }

        /// <summary>
        /// Gets or sets the opening price.
        /// </summary>
        [JsonPropertyName("open")]
        public double? Open { get; set; }

        /// <summary>
        /// Gets or sets the high price.
        /// </summary>
        [JsonPropertyName("high")]
        public double? High { get; set; }

        /// <summary>
        /// Gets or sets the low price.
        /// </summary>
        [JsonPropertyName("low")]
        public double? Low { get; set; }

        /// <summary>
        /// Gets or sets the last price.
        /// </summary>
        [JsonPropertyName("last")]
        public double? Last { get; set; }

        /// <summary>
        /// Gets or sets the last trade size.
        /// </summary>
        [JsonPropertyName("last_size")]
        public long? LastSize { get; set; }

        /// <summary>
        /// Gets or sets the price change.
        /// </summary>
        [JsonPropertyName("change")]
        public double? Change { get; set; }

        /// <summary>
        /// Gets or sets the percentage price change.
        /// </summary>
        [JsonPropertyName("pctchange")]
        public double? PercentChange { get; set; }

        /// <summary>
        /// Gets or sets the previous price.
        /// </summary>
        [JsonPropertyName("previous")]
        public double? Previous { get; set; }

        /// <summary>
        /// Gets or sets the previous price date (<c>yyyy-MM-dd</c>), or <c>null</c>.
        /// </summary>
        [JsonPropertyName("previous_date")]
        public string PreviousDate { get; set; }

        /// <summary>
        /// Gets or sets the bid price.
        /// </summary>
        [JsonPropertyName("bid")]
        public double? Bid { get; set; }

        /// <summary>
        /// Gets or sets the bid timestamp, as sent by the API (format varies).
        /// </summary>
        [JsonPropertyName("bid_date")]
        public string BidDate { get; set; }

        /// <summary>
        /// Gets or sets the bid size.
        /// </summary>
        [JsonPropertyName("bid_size")]
        public long? BidSize { get; set; }

        /// <summary>
        /// Gets or sets the ask price.
        /// </summary>
        [JsonPropertyName("ask")]
        public double? Ask { get; set; }

        /// <summary>
        /// Gets or sets the ask timestamp, as sent by the API (format varies).
        /// </summary>
        [JsonPropertyName("ask_date")]
        public string AskDate { get; set; }

        /// <summary>
        /// Gets or sets the ask size.
        /// </summary>
        [JsonPropertyName("ask_size")]
        public long? AskSize { get; set; }

        /// <summary>
        /// Gets or sets the moneyness.
        /// </summary>
        [JsonPropertyName("moneyness")]
        public double? Moneyness { get; set; }

        /// <summary>
        /// Gets or sets the traded volume.
        /// </summary>
        [JsonPropertyName("volume")]
        public long? Volume { get; set; }

        /// <summary>
        /// Gets or sets the change in volume.
        /// </summary>
        [JsonPropertyName("volume_change")]
        public long? VolumeChange { get; set; }

        /// <summary>
        /// Gets or sets the percentage change in volume.
        /// </summary>
        [JsonPropertyName("volume_pctchange")]
        public double? VolumePercentChange { get; set; }

        /// <summary>
        /// Gets or sets the open interest.
        /// </summary>
        [JsonPropertyName("open_interest")]
        public long? OpenInterest { get; set; }

        /// <summary>
        /// Gets or sets the change in open interest.
        /// </summary>
        [JsonPropertyName("open_interest_change")]
        public long? OpenInterestChange { get; set; }

        /// <summary>
        /// Gets or sets the percentage change in open interest.
        /// </summary>
        [JsonPropertyName("open_interest_pctchange")]
        public double? OpenInterestPercentChange { get; set; }

        /// <summary>
        /// Gets or sets the implied volatility.
        /// </summary>
        [JsonPropertyName("volatility")]
        public double? Volatility { get; set; }

        /// <summary>
        /// Gets or sets the change in implied volatility.
        /// </summary>
        [JsonPropertyName("volatility_change")]
        public double? VolatilityChange { get; set; }

        /// <summary>
        /// Gets or sets the percentage change in implied volatility.
        /// </summary>
        [JsonPropertyName("volatility_pctchange")]
        public double? VolatilityPercentChange { get; set; }

        /// <summary>
        /// Gets or sets the theoretical price.
        /// </summary>
        [JsonPropertyName("theoretical")]
        public double? Theoretical { get; set; }

        /// <summary>
        /// Gets or sets the delta greek.
        /// </summary>
        [JsonPropertyName("delta")]
        public double? Delta { get; set; }

        /// <summary>
        /// Gets or sets the gamma greek.
        /// </summary>
        [JsonPropertyName("gamma")]
        public double? Gamma { get; set; }

        /// <summary>
        /// Gets or sets the theta greek.
        /// </summary>
        [JsonPropertyName("theta")]
        public double? Theta { get; set; }

        /// <summary>
        /// Gets or sets the vega greek.
        /// </summary>
        [JsonPropertyName("vega")]
        public double? Vega { get; set; }

        /// <summary>
        /// Gets or sets the rho greek.
        /// </summary>
        [JsonPropertyName("rho")]
        public double? Rho { get; set; }

        /// <summary>
        /// Gets or sets the trade date (<c>yyyy-MM-dd</c>), or <c>null</c>.
        /// </summary>
        [JsonPropertyName("tradetime")]
        public string TradeTime { get; set; }

        /// <summary>
        /// Gets or sets the volume / open-interest ratio.
        /// </summary>
        [JsonPropertyName("vol_oi_ratio")]
        public double? VolumeOpenInterestRatio { get; set; }

        /// <summary>
        /// Gets or sets the number of days until expiration.
        /// </summary>
        [JsonPropertyName("dte")]
        public int? DaysToExpiration { get; set; }

        /// <summary>
        /// Gets or sets the bid/ask midpoint.
        /// </summary>
        [JsonPropertyName("midpoint")]
        public double? Midpoint { get; set; }
    }
}
