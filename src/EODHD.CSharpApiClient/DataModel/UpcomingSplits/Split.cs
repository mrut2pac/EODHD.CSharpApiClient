using System;
using System.Text.Json.Serialization;

namespace EODHD.CSharpApiClient.DataModel.UpcomingSplits
{
    /// <summary>
    /// Represents a single upcoming stock-split entry for a symbol.
    /// </summary>
    public sealed class Split
    {
        /// <summary>
        /// Gets or sets the full EODHD code (symbol and exchange).
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Gets the symbol portion of <see cref="Code"/>.
        /// </summary>
        public string Symbol
        {
            get
            {
                if(this.Code == null)
                {
                    return null;
                }

                int pointIndex = this.Code.IndexOf('.');
                return pointIndex == -1 ? this.Code : this.Code.Substring(0, pointIndex);
            }
        }

        /// <summary>
        /// Gets the exchange portion of <see cref="Code"/>.
        /// </summary>
        public string Exchange
        {
            get
            {
                if(this.Code == null)
                {
                    return null;
                }

                int pointIndex = this.Code.IndexOf('.');
                return pointIndex == -1 ? string.Empty : this.Code.Substring(pointIndex + 1);
            }
        }

        /// <summary>
        /// Gets or sets the date the split takes effect.
        /// </summary>
        [JsonPropertyName("Split_Date")]
        public DateTime? SplitDate { get; set; }

        /// <summary>
        /// Gets or sets the optionable flag.
        /// </summary>
        public string Optionable { get; set; }

        /// <summary>
        /// Gets or sets the number of old shares in the split ratio.
        /// </summary>
        [JsonPropertyName("Old_Shares")]
        public double? OldShares { get; set; }

        /// <summary>
        /// Gets or sets the number of new shares in the split ratio.
        /// </summary>
        [JsonPropertyName("New_Shares")]
        public double? NewShares { get; set; }

        /// <summary>
        /// Gets the split factor (old shares divided by new shares), or <c>null</c> when it cannot be computed.
        /// </summary>
        public double? SplitFactor
        {
            get
            {
                if(this.OldShares.HasValue && this.NewShares.HasValue && this.NewShares.Value != 0)
                {
                    return this.OldShares.Value / this.NewShares.Value;
                }
                else
                {
                    return null;
                }
            }
        }
    }
}
