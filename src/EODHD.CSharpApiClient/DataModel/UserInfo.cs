using System;
using System.Text.Json.Serialization;

namespace EODHD.CSharpApiClient.DataModel
{
    /// <summary>
    /// Account and usage information returned by the EODHD User API (<c>/api/user</c>).
    /// </summary>
    public sealed class UserInfo
    {
        /// <summary>
        /// Gets or sets the account holder's name.
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the account email address.
        /// </summary>
        [JsonPropertyName("email")]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the subscription plan name (e.g. <c>"All-In-One"</c>).
        /// </summary>
        [JsonPropertyName("subscriptionType")]
        public string SubscriptionType { get; set; }

        /// <summary>
        /// Gets or sets the payment method on file.
        /// </summary>
        [JsonPropertyName("paymentMethod")]
        public string PaymentMethod { get; set; }

        /// <summary>
        /// Gets or sets the number of API requests consumed on <see cref="ApiRequestsDate"/>.
        /// </summary>
        [JsonPropertyName("apiRequests")]
        public int ApiRequests { get; set; }

        /// <summary>
        /// Gets or sets the date the <see cref="ApiRequests"/> count applies to.
        /// </summary>
        [JsonPropertyName("apiRequestsDate")]
        public DateTime ApiRequestsDate { get; set; }

        /// <summary>
        /// Gets or sets the daily request quota for the plan.
        /// </summary>
        [JsonPropertyName("dailyRateLimit")]
        public int DailyRateLimit { get; set; }

        /// <summary>
        /// Gets or sets the additional request allowance beyond <see cref="DailyRateLimit"/>.
        /// </summary>
        [JsonPropertyName("extraLimit")]
        public int ExtraLimit { get; set; }

        /// <summary>
        /// Gets or sets the account's referral invite token.
        /// </summary>
        [JsonPropertyName("inviteToken")]
        public string InviteToken { get; set; }

        /// <summary>
        /// Gets or sets the number of times the invite token has been clicked.
        /// </summary>
        [JsonPropertyName("inviteTokenClicked")]
        public int InviteTokenClicked { get; set; }
    }
}
