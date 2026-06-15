using System.Threading.Tasks;

using EODHD.CSharpApiClient.DataModel;

using Xunit;

namespace EODHD.CSharpApiClient.IntegrationTests
{
    public class UserApiTests : IntegrationTestBase
    {
        [SkippableFact]
        public async Task GetUserInfo_ValidToken_ReturnsAccountInfo()
        {
            this.SkipIfNoApiKey();

            using EodhdClient client = this.CreateClient();

            UserInfo user = await client.GetUserInfoAsync();

            Assert.NotNull(user);
            Assert.True(user.DailyRateLimit > 0, "Expected a positive daily rate limit for a live account.");
        }
    }
}
