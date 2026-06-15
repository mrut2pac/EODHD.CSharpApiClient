using System;
using System.Text.Json.Serialization;

namespace EODHD.CSharpApiClient.DataModel.InsiderTransactions
{
    /// <summary>
    /// A single insider (SEC Form 4) transaction record.
    /// </summary>
    public sealed class InsiderTransaction
    {
        /// <summary>
        /// Gets or sets the ticker symbol.
        /// </summary>
        [JsonPropertyName("code")]
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets the exchange listing code.
        /// </summary>
        [JsonPropertyName("exchange")]
        public string Exchange { get; set; }

        /// <summary>
        /// Gets or sets the record date.
        /// </summary>
        [JsonPropertyName("date")]
        public DateTime? Date { get; set; }

        /// <summary>
        /// Gets or sets the date the transaction was reported to the SEC.
        /// </summary>
        [JsonPropertyName("reportDate")]
        public DateTime? ReportDate { get; set; }

        /// <summary>
        /// Gets or sets the owner's SEC Central Index Key.
        /// </summary>
        [JsonPropertyName("ownerCik")]
        public string OwnerCik { get; set; }

        /// <summary>
        /// Gets or sets the owner (insider) name.
        /// </summary>
        [JsonPropertyName("ownerName")]
        public string OwnerName { get; set; }

        /// <summary>
        /// Gets or sets the owner's relationship to the company.
        /// </summary>
        [JsonPropertyName("ownerRelationship")]
        public string OwnerRelationship { get; set; }

        /// <summary>
        /// Gets or sets the owner's title (e.g. <c>"Director"</c>, <c>"COO"</c>).
        /// </summary>
        [JsonPropertyName("ownerTitle")]
        public string OwnerTitle { get; set; }

        /// <summary>
        /// Gets or sets the transaction date.
        /// </summary>
        [JsonPropertyName("transactionDate")]
        public DateTime? TransactionDate { get; set; }

        /// <summary>
        /// Gets or sets the SEC transaction code (e.g. <c>"P"</c> purchase, <c>"S"</c> sale).
        /// </summary>
        [JsonPropertyName("transactionCode")]
        public string TransactionCode { get; set; }

        /// <summary>
        /// Gets or sets the number of shares in the transaction.
        /// </summary>
        [JsonPropertyName("transactionAmount")]
        public decimal? TransactionAmount { get; set; }

        /// <summary>
        /// Gets or sets the per-share transaction price.
        /// </summary>
        [JsonPropertyName("transactionPrice")]
        public double? TransactionPrice { get; set; }

        /// <summary>
        /// Gets or sets whether shares were acquired (<c>"A"</c>) or disposed (<c>"D"</c>).
        /// </summary>
        [JsonPropertyName("transactionAcquiredDisposed")]
        public string TransactionAcquiredDisposed { get; set; }

        /// <summary>
        /// Gets or sets the share balance held after the transaction.
        /// </summary>
        [JsonPropertyName("postTransactionAmount")]
        public decimal? PostTransactionAmount { get; set; }

        /// <summary>
        /// Gets or sets the URL of the underlying SEC filing.
        /// </summary>
        [JsonPropertyName("link")]
        public string Link { get; set; }
    }
}
