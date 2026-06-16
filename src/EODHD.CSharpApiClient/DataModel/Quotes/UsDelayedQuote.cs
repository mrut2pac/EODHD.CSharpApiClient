using System;
using System.Text.Json.Serialization;

namespace EODHD.CSharpApiClient.DataModel.Quotes
{
    /// <summary>
    /// A delayed US stock quote with reference data, intraday OHLC, top-of-book, last trade, and a range
    /// of statistics (moving averages, 52-week range, market cap, valuation ratios, and dividend data).
    /// </summary>
    public sealed class UsDelayedQuote
    {
        /// <summary>Gets or sets the symbol (e.g. <c>"AAPL.US"</c>).</summary>
        [JsonPropertyName("symbol")]
        public string Symbol { get; set; }

        /// <summary>Gets or sets the exchange code.</summary>
        [JsonPropertyName("exchange")]
        public string Exchange { get; set; }

        /// <summary>Gets or sets the ISO (MIC) exchange code.</summary>
        [JsonPropertyName("isoExchange")]
        public string IsoExchange { get; set; }

        /// <summary>Gets or sets the Benzinga exchange code.</summary>
        [JsonPropertyName("bzExchange")]
        public string BzExchange { get; set; }

        /// <summary>Gets or sets the OTC market, when applicable.</summary>
        [JsonPropertyName("otcMarket")]
        public string OtcMarket { get; set; }

        /// <summary>Gets or sets the OTC tier, when applicable.</summary>
        [JsonPropertyName("otcTier")]
        public string OtcTier { get; set; }

        /// <summary>Gets or sets the instrument type (e.g. <c>"STOCK"</c>).</summary>
        [JsonPropertyName("type")]
        public string Type { get; set; }

        /// <summary>Gets or sets the short name.</summary>
        [JsonPropertyName("name")]
        public string Name { get; set; }

        /// <summary>Gets or sets the standardised company name.</summary>
        [JsonPropertyName("companyStandardName")]
        public string CompanyStandardName { get; set; }

        /// <summary>Gets or sets the description.</summary>
        [JsonPropertyName("description")]
        public string Description { get; set; }

        /// <summary>Gets or sets the sector.</summary>
        [JsonPropertyName("sector")]
        public string Sector { get; set; }

        /// <summary>Gets or sets the industry.</summary>
        [JsonPropertyName("industry")]
        public string Industry { get; set; }

        /// <summary>Gets or sets the opening price.</summary>
        [JsonPropertyName("open")]
        public double? Open { get; set; }

        /// <summary>Gets or sets the high price.</summary>
        [JsonPropertyName("high")]
        public double? High { get; set; }

        /// <summary>Gets or sets the low price.</summary>
        [JsonPropertyName("low")]
        public double? Low { get; set; }

        /// <summary>Gets or sets the bid price.</summary>
        [JsonPropertyName("bidPrice")]
        public double? BidPrice { get; set; }

        /// <summary>Gets or sets the ask price.</summary>
        [JsonPropertyName("askPrice")]
        public double? AskPrice { get; set; }

        /// <summary>Gets or sets the ask size.</summary>
        [JsonPropertyName("askSize")]
        public long? AskSize { get; set; }

        /// <summary>Gets or sets the bid size.</summary>
        [JsonPropertyName("bidSize")]
        public long? BidSize { get; set; }

        /// <summary>Gets or sets the last trade size.</summary>
        [JsonPropertyName("size")]
        public long? Size { get; set; }

        /// <summary>Gets or sets the bid timestamp, in milliseconds since the Unix epoch.</summary>
        [JsonPropertyName("bidTime")]
        public long? BidTimeMs { get; set; }

        /// <summary>Gets the bid timestamp as a UTC instant (derived from <see cref="BidTimeMs"/>).</summary>
        [JsonIgnore]
        public DateTimeOffset? BidTimeUtc => UnixTime.FromMilliseconds(this.BidTimeMs);

        /// <summary>Gets or sets the ask timestamp, in milliseconds since the Unix epoch.</summary>
        [JsonPropertyName("askTime")]
        public long? AskTimeMs { get; set; }

        /// <summary>Gets the ask timestamp as a UTC instant (derived from <see cref="AskTimeMs"/>).</summary>
        [JsonIgnore]
        public DateTimeOffset? AskTimeUtc => UnixTime.FromMilliseconds(this.AskTimeMs);

        /// <summary>Gets or sets the last trade price.</summary>
        [JsonPropertyName("lastTradePrice")]
        public double? LastTradePrice { get; set; }

        /// <summary>Gets or sets the last trade timestamp, in milliseconds since the Unix epoch.</summary>
        [JsonPropertyName("lastTradeTime")]
        public long? LastTradeTimeMs { get; set; }

        /// <summary>Gets the last trade timestamp as a UTC instant (derived from <see cref="LastTradeTimeMs"/>).</summary>
        [JsonIgnore]
        public DateTimeOffset? LastTradeTimeUtc => UnixTime.FromMilliseconds(this.LastTradeTimeMs);

        /// <summary>Gets or sets the traded volume.</summary>
        [JsonPropertyName("volume")]
        public long? Volume { get; set; }

        /// <summary>Gets or sets the price change.</summary>
        [JsonPropertyName("change")]
        public double? Change { get; set; }

        /// <summary>Gets or sets the percentage price change.</summary>
        [JsonPropertyName("changePercent")]
        public double? ChangePercent { get; set; }

        /// <summary>Gets or sets the previous close price.</summary>
        [JsonPropertyName("previousClosePrice")]
        public double? PreviousClosePrice { get; set; }

        /// <summary>Gets or sets the previous close date/time, as sent by the API (space-separated).</summary>
        [JsonPropertyName("previousCloseDate")]
        public string PreviousCloseDate { get; set; }

        /// <summary>Gets or sets the 50-day average price.</summary>
        [JsonPropertyName("fiftyDayAveragePrice")]
        public double? FiftyDayAveragePrice { get; set; }

        /// <summary>Gets or sets the 100-day average price.</summary>
        [JsonPropertyName("hundredDayAveragePrice")]
        public double? HundredDayAveragePrice { get; set; }

        /// <summary>Gets or sets the 200-day average price.</summary>
        [JsonPropertyName("twoHundredDayAveragePrice")]
        public double? TwoHundredDayAveragePrice { get; set; }

        /// <summary>Gets or sets the average volume.</summary>
        [JsonPropertyName("averageVolume")]
        public long? AverageVolume { get; set; }

        /// <summary>Gets or sets the 52-week high.</summary>
        [JsonPropertyName("fiftyTwoWeekHigh")]
        public double? FiftyTwoWeekHigh { get; set; }

        /// <summary>Gets or sets the 52-week low.</summary>
        [JsonPropertyName("fiftyTwoWeekLow")]
        public double? FiftyTwoWeekLow { get; set; }

        /// <summary>Gets or sets the market capitalisation.</summary>
        [JsonPropertyName("marketCap")]
        public decimal? MarketCap { get; set; }

        /// <summary>Gets or sets the shares outstanding.</summary>
        [JsonPropertyName("sharesOutstanding")]
        public decimal? SharesOutstanding { get; set; }

        /// <summary>Gets or sets the free-float shares.</summary>
        [JsonPropertyName("sharesFloat")]
        public decimal? SharesFloat { get; set; }

        /// <summary>Gets or sets the price/earnings ratio.</summary>
        [JsonPropertyName("pe")]
        public double? Pe { get; set; }

        /// <summary>Gets or sets the forward price/earnings ratio.</summary>
        [JsonPropertyName("forwardPE")]
        public double? ForwardPe { get; set; }

        /// <summary>Gets or sets the dividend yield.</summary>
        [JsonPropertyName("dividendYield")]
        public double? DividendYield { get; set; }

        /// <summary>Gets or sets the dividend per share.</summary>
        [JsonPropertyName("dividend")]
        public double? Dividend { get; set; }

        /// <summary>Gets or sets the dividend payout ratio.</summary>
        [JsonPropertyName("payoutRatio")]
        public double? PayoutRatio { get; set; }

        /// <summary>Gets or sets the extended-hours (ETH) price.</summary>
        [JsonPropertyName("ethPrice")]
        public double? EthPrice { get; set; }

        /// <summary>Gets or sets the extended-hours (ETH) volume.</summary>
        [JsonPropertyName("ethVolume")]
        public long? EthVolume { get; set; }

        /// <summary>Gets or sets the extended-hours (ETH) timestamp, in milliseconds since the Unix epoch.</summary>
        [JsonPropertyName("ethTime")]
        public long? EthTimeMs { get; set; }

        /// <summary>Gets the extended-hours (ETH) timestamp as a UTC instant (derived from <see cref="EthTimeMs"/>).</summary>
        [JsonIgnore]
        public DateTimeOffset? EthTimeUtc => UnixTime.FromMilliseconds(this.EthTimeMs);

        /// <summary>Gets or sets the trading currency.</summary>
        [JsonPropertyName("currency")]
        public string Currency { get; set; }

        /// <summary>Gets or sets the issuer name.</summary>
        [JsonPropertyName("issuerName")]
        public string IssuerName { get; set; }

        /// <summary>Gets or sets whether this is the primary listing.</summary>
        [JsonPropertyName("primary")]
        public bool? Primary { get; set; }

        /// <summary>Gets or sets the short description.</summary>
        [JsonPropertyName("shortDescription")]
        public string ShortDescription { get; set; }

        /// <summary>Gets or sets the short issuer name.</summary>
        [JsonPropertyName("issuerShortName")]
        public string IssuerShortName { get; set; }

        /// <summary>Gets or sets the quote timestamp, in milliseconds since the Unix epoch.</summary>
        [JsonPropertyName("timestamp")]
        public long? TimestampMs { get; set; }

        /// <summary>Gets the quote timestamp as a UTC instant (derived from <see cref="TimestampMs"/>).</summary>
        [JsonIgnore]
        public DateTimeOffset? TimestampUtc => UnixTime.FromMilliseconds(this.TimestampMs);
    }
}
