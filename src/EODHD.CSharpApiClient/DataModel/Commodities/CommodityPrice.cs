using System;
using System.Text.Json.Serialization;

namespace EODHD.CSharpApiClient.DataModel.Commodities
{
    /// <summary>
    /// A single historical commodity price observation (date and value, in the series' native unit).
    /// </summary>
    public sealed class CommodityPrice
    {
        /// <summary>
        /// Gets or sets the observation date.
        /// </summary>
        [JsonPropertyName("date")]
        public DateTime? Date { get; set; }

        /// <summary>
        /// Gets or sets the price value, in the series' native unit (e.g. dollars per barrel).
        /// </summary>
        [JsonPropertyName("value")]
        public double? Value { get; set; }
    }
}
