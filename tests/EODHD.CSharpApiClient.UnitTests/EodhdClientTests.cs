using System;
using System.Net;

using EODHD.CSharpApiClient.DataModel;
using EODHD.CSharpApiClient.Exceptions;

using Xunit;

namespace EODHD.CSharpApiClient.UnitTests
{
    public class EodhdClientTests
    {
        private const string UserInfoJson =
            "{\"name\":\"Jane Doe\",\"email\":\"jane@example.com\",\"subscriptionType\":\"All-In-One\"," +
            "\"paymentMethod\":\"card\",\"apiRequests\":123,\"apiRequestsDate\":\"2026-06-15\"," +
            "\"dailyRateLimit\":100000,\"extraLimit\":0,\"inviteToken\":\"abc\",\"inviteTokenClicked\":2}";

        [Fact]
        public async System.Threading.Tasks.Task GetUserInfoAsync_ValidResponse_DeserializesAllFields()
        {
            FakeHttpTransport transport = new FakeHttpTransport(HttpStatusCode.OK, UserInfoJson);
            using EodhdClient client = new EodhdClient(new EodhdClientOptions { ApiToken = "test" }, transport);

            UserInfo user = await client.GetUserInfoAsync();

            Assert.Equal("Jane Doe", user.Name);
            Assert.Equal("jane@example.com", user.Email);
            Assert.Equal("All-In-One", user.SubscriptionType);
            Assert.Equal(123, user.ApiRequests);
            Assert.Equal(new DateTime(2026, 6, 15), user.ApiRequestsDate);
            Assert.Equal(100000, user.DailyRateLimit);
            Assert.Equal(2, user.InviteTokenClicked);
        }

        [Fact]
        public async System.Threading.Tasks.Task GetUserInfoAsync_Always_SendsApiTokenAndJsonFormat()
        {
            FakeHttpTransport transport = new FakeHttpTransport(HttpStatusCode.OK, UserInfoJson);
            using EodhdClient client = new EodhdClient(new EodhdClientOptions { ApiToken = "secret-token" }, transport);

            await client.GetUserInfoAsync();

            Assert.NotNull(transport.LastRequestUri);
            string requested = transport.LastRequestUri.ToString();
            Assert.Equal("https://eodhd.com/api/user", transport.LastRequestUri.GetLeftPart(UriPartial.Path));
            Assert.Contains("api_token=secret-token", requested, StringComparison.Ordinal);
            Assert.Contains("fmt=json", requested, StringComparison.Ordinal);
        }

        [Fact]
        public async System.Threading.Tasks.Task GetUserInfoAsync_Unauthorized_ThrowsTypedException()
        {
            FakeHttpTransport transport = new FakeHttpTransport(HttpStatusCode.Unauthorized, "Forbidden: invalid token");
            using EodhdClient client = new EodhdClient(new EodhdClientOptions { ApiToken = "bad" }, transport);

            EodhdHttpException ex = await Assert.ThrowsAsync<EodhdHttpException>(() => client.GetUserInfoAsync());

            Assert.Equal(401, ex.StatusCode);
            Assert.Equal(EodhdErrorCase.Unauthorized, ex.Code);
        }

        [Fact]
        public void Constructor_NullToken_Throws()
        {
            Assert.Throws<ArgumentException>(() => new EodhdClient(new EodhdClientOptions { ApiToken = null }));
        }
    }
}
