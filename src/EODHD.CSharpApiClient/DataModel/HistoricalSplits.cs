using System;
using System.Text.Json.Serialization;

namespace EODHD.CSharpApiClient.DataModel
{
    /// <summary>
    /// Represents a single historical stock split event for a symbol.
    /// </summary>
    public class HistoricalSplit
    {
        /// <summary>
        /// Gets or sets the date the split took effect.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets the raw split ratio string (e.g. <c>"2.000000/1.000000"</c>).
        /// </summary>
        public string Split { get; set; }

        /// <summary>
        /// Gets the split factor parsed from <see cref="Split"/>, or <c>null</c> when it cannot be parsed.
        /// </summary>
        [JsonIgnore]
        public double? Factor => Utilities.ParseSplitFactor(this.Split);
    }
}
