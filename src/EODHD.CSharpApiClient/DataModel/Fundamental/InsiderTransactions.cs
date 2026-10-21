using System;
using System.Text.Json.Serialization;

using EODHD.CSharpApiClient.JsonSupport;

namespace EODHD.CSharpApiClient.DataModel.Fundamental
{
    /// <summary>
    /// Represents a single insider transaction record.
    /// </summary>
    public sealed class InsiderTransactions
    {
        /// <summary>
        /// Gets or sets the record date.
        /// </summary>
        [JsonPropertyName("Date")]
        [JsonConverter(typeof(NaTolerantDateConverter))]
        public DateTime? Date { get; set; }

        /// <summary>
        /// Gets or sets the owner Central Index Key (CIK).
        /// </summary>
        public string OwnerCik { get; set; }

        /// <summary>
        /// Gets or sets the owner name.
        /// </summary>
        public string OwnerName { get; set; }

        /// <summary>
        /// Gets or sets the transaction date.
        /// </summary>
        [JsonPropertyName("TransactionDate")]
        [JsonConverter(typeof(NaTolerantDateConverter))]
        public DateTime? TransactionDate { get; set; }

        /// <summary>
        /// Gets or sets the transaction code.
        /// </summary>
        public string TransactionCode { get; set; }

        /// <summary>
        /// Gets or sets the transaction amount, as a count of shares.
        /// </summary>
        public decimal? TransactionAmount { get; set; }

        /// <summary>
        /// Gets or sets the transaction price, as a price.
        /// </summary>
        public double? TransactionPrice { get; set; }

        /// <summary>
        /// Gets or sets the transaction acquired/disposed indicator.
        /// </summary>
        public string TransactionAcquiredDisposed { get; set; }

        /// <summary>
        /// Gets or sets the post-transaction amount, as a count of shares.
        /// </summary>
        public decimal? PostTransactionAmount { get; set; }

        /// <summary>
        /// Gets or sets the SEC filing link.
        /// </summary>
        public string SecLink { get; set; }
    }
}
