using System;

namespace EODHD.CSharpApiClient.DataModel.Ticks
{
    /// <summary>
    /// A single historical trade (tick) for a US equity. The API returns ticks as parallel columns;
    /// the client transposes them into one record per trade.
    /// </summary>
    public sealed class Tick
    {
        /// <summary>Gets or sets the exchange where the transaction took place.</summary>
        public string Exchange { get; set; }

        /// <summary>Gets or sets the market where the trade took place.</summary>
        public string Market { get; set; }

        /// <summary>Gets or sets the sub-market where the trade took place.</summary>
        public string SubMarket { get; set; }

        /// <summary>Gets or sets the transaction price.</summary>
        public double? Price { get; set; }

        /// <summary>Gets or sets the trade sequence number.</summary>
        public long? Sequence { get; set; }

        /// <summary>Gets or sets the number of shares in the transaction.</summary>
        public long? Shares { get; set; }

        /// <summary>Gets or sets the sales-condition codes for the trade.</summary>
        public string SalesCondition { get; set; }

        /// <summary>Gets or sets the trade timestamp, in milliseconds since the Unix epoch.</summary>
        public long? TimestampMs { get; set; }

        /// <summary>Gets the trade timestamp as a UTC instant (derived from <see cref="TimestampMs"/>).</summary>
        public DateTimeOffset? TimestampUtc => UnixTime.FromMilliseconds(this.TimestampMs);
    }
}
