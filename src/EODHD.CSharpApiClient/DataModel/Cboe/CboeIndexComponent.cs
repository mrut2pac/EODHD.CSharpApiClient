using System.Text.Json.Serialization;

namespace EODHD.CSharpApiClient.DataModel.Cboe
{
    /// <summary>
    /// A single constituent of a Cboe index, with identifiers, pricing, and the weighting applied
    /// inside the index.
    /// </summary>
    public sealed class CboeIndexComponent
    {
        /// <summary>Gets or sets the constituent symbol (e.g. <c>"KPN.AS"</c>).</summary>
        [JsonPropertyName("symbol")]
        public string Symbol { get; set; }

        /// <summary>Gets or sets the ISIN.</summary>
        [JsonPropertyName("isin")]
        public string Isin { get; set; }

        /// <summary>Gets or sets the company name.</summary>
        [JsonPropertyName("name")]
        public string Name { get; set; }

        /// <summary>Gets or sets the Bloomberg-style equity identifier.</summary>
        [JsonPropertyName("equity")]
        public string Equity { get; set; }

        /// <summary>Gets or sets the SEDOL.</summary>
        [JsonPropertyName("sedol")]
        public string Sedol { get; set; }

        /// <summary>Gets or sets the CUSIP.</summary>
        [JsonPropertyName("cusip")]
        public string Cusip { get; set; }

        /// <summary>Gets or sets the country of incorporation.</summary>
        [JsonPropertyName("country")]
        public string Country { get; set; }

        /// <summary>Gets or sets the revenue country.</summary>
        [JsonPropertyName("revenue_country")]
        public string RevenueCountry { get; set; }

        /// <summary>Gets or sets the closing price.</summary>
        [JsonPropertyName("closing_price")]
        public double? ClosingPrice { get; set; }

        /// <summary>Gets or sets the trading currency.</summary>
        [JsonPropertyName("currency")]
        public string Currency { get; set; }

        /// <summary>Gets or sets the closing factor.</summary>
        [JsonPropertyName("closing_factor")]
        public double? ClosingFactor { get; set; }

        /// <summary>Gets or sets the total shares outstanding.</summary>
        [JsonPropertyName("total_shares")]
        public decimal? TotalShares { get; set; }

        /// <summary>Gets or sets the market capitalisation.</summary>
        [JsonPropertyName("market_cap")]
        public decimal? MarketCap { get; set; }

        /// <summary>Gets or sets the free-float market capitalisation.</summary>
        [JsonPropertyName("market_cap_free_float")]
        public decimal? MarketCapFreeFloat { get; set; }

        /// <summary>Gets or sets the free-float factor.</summary>
        [JsonPropertyName("free_float_factor")]
        public double? FreeFloatFactor { get; set; }

        /// <summary>Gets or sets the weighting cap factor.</summary>
        [JsonPropertyName("weighting_cap_factor")]
        public double? WeightingCapFactor { get; set; }

        /// <summary>Gets or sets the constituent's weighting in the index (percent).</summary>
        [JsonPropertyName("index_weighting")]
        public double? IndexWeighting { get; set; }

        /// <summary>Gets or sets the number of index shares.</summary>
        [JsonPropertyName("index_shares")]
        public double? IndexShares { get; set; }

        /// <summary>Gets or sets the constituent's index value contribution.</summary>
        [JsonPropertyName("index_value")]
        public double? IndexValue { get; set; }

        /// <summary>Gets or sets the sector.</summary>
        [JsonPropertyName("sector")]
        public string Sector { get; set; }
    }
}
