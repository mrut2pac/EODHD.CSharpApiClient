using System;
using System.Text.Json.Serialization;

namespace EODHD.CSharpApiClient.DataModel.Options
{
    /// <summary>
    /// A single option contract quote from the legacy options endpoint, including pricing and greeks.
    /// </summary>
    public sealed class OptionContract
    {
        /// <summary>
        /// Gets or sets the OCC contract name (e.g. <c>"AAPL260615C00250000"</c>).
        /// </summary>
        [JsonPropertyName("contractName")]
        public string ContractName { get; set; }

        /// <summary>
        /// Gets or sets the contract size (e.g. <c>"REGULAR"</c>).
        /// </summary>
        [JsonPropertyName("contractSize")]
        public string ContractSize { get; set; }

        /// <summary>
        /// Gets or sets the contract period (e.g. <c>"WEEKLY"</c>, <c>"MONTHLY"</c>).
        /// </summary>
        [JsonPropertyName("contractPeriod")]
        public string ContractPeriod { get; set; }

        /// <summary>
        /// Gets or sets the trading currency.
        /// </summary>
        [JsonPropertyName("currency")]
        public string Currency { get; set; }

        /// <summary>
        /// Gets or sets the option type (<c>"CALL"</c> or <c>"PUT"</c>).
        /// </summary>
        [JsonPropertyName("type")]
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets whether the contract is in the money (<c>"TRUE"</c> or <c>"FALSE"</c>).
        /// </summary>
        [JsonPropertyName("inTheMoney")]
        public string InTheMoney { get; set; }

        /// <summary>
        /// Gets or sets the last trade timestamp as sent by the API. This is a raw string because the
        /// endpoint uses a zero sentinel (<c>"0000-00-00 00:00:00"</c>) for contracts that have not traded.
        /// </summary>
        [JsonPropertyName("lastTradeDateTime")]
        public string LastTradeDateTime { get; set; }

        /// <summary>
        /// Gets or sets the expiration date.
        /// </summary>
        [JsonPropertyName("expirationDate")]
        public DateTime? ExpirationDate { get; set; }

        /// <summary>
        /// Gets or sets the strike price.
        /// </summary>
        [JsonPropertyName("strike")]
        public double? Strike { get; set; }

        /// <summary>
        /// Gets or sets the last traded price.
        /// </summary>
        [JsonPropertyName("lastPrice")]
        public double? LastPrice { get; set; }

        /// <summary>
        /// Gets or sets the current bid price.
        /// </summary>
        [JsonPropertyName("bid")]
        public double? Bid { get; set; }

        /// <summary>
        /// Gets or sets the current ask price.
        /// </summary>
        [JsonPropertyName("ask")]
        public double? Ask { get; set; }

        /// <summary>
        /// Gets or sets the price change.
        /// </summary>
        [JsonPropertyName("change")]
        public double? Change { get; set; }

        /// <summary>
        /// Gets or sets the percentage price change.
        /// </summary>
        [JsonPropertyName("changePercent")]
        public double? ChangePercent { get; set; }

        /// <summary>
        /// Gets or sets the traded volume.
        /// </summary>
        [JsonPropertyName("volume")]
        public long? Volume { get; set; }

        /// <summary>
        /// Gets or sets the open interest.
        /// </summary>
        [JsonPropertyName("openInterest")]
        public long? OpenInterest { get; set; }

        /// <summary>
        /// Gets or sets the implied volatility.
        /// </summary>
        [JsonPropertyName("impliedVolatility")]
        public double? ImpliedVolatility { get; set; }

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
        /// Gets or sets the theoretical price.
        /// </summary>
        [JsonPropertyName("theoretical")]
        public double? Theoretical { get; set; }

        /// <summary>
        /// Gets or sets the intrinsic value.
        /// </summary>
        [JsonPropertyName("intrinsicValue")]
        public double? IntrinsicValue { get; set; }

        /// <summary>
        /// Gets or sets the time value.
        /// </summary>
        [JsonPropertyName("timeValue")]
        public double? TimeValue { get; set; }

        /// <summary>
        /// Gets or sets the timestamp the quote was last updated, as sent by the API (space-separated
        /// <c>yyyy-MM-dd HH:mm:ss</c>); kept as a raw string because untraded contracts may carry a sentinel.
        /// </summary>
        [JsonPropertyName("updatedAt")]
        public string UpdatedAt { get; set; }

        /// <summary>
        /// Gets or sets the number of days until expiration.
        /// </summary>
        [JsonPropertyName("daysBeforeExpiration")]
        public int? DaysBeforeExpiration { get; set; }
    }
}
