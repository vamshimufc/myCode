using IdentityModel.Client;
using System.Net.Http;
using System.Threading.Tasks;
namespace wdhrtosis.IntegrationTests
{
    public static class HttpClientExtensions
    {
        public static async Task SetFullAccessBearerToken(this HttpClient client)
        {
            var tokenResponse = await client.GetFullAccessToken();
            client.SetBearerToken(tokenResponse.AccessToken);
        }

        public static async Task SetReadOnlyBearerToken(this HttpClient client)
        {
            var tokenResponse = await client.GetReadOnlyAccessToken();
            client.SetBearerToken(tokenResponse.AccessToken);
        }

        public static async Task<TokenResponse> GetFullAccessToken(this HttpClient client)
        {
            return await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest()
            {
                Address = $"{client.BaseAddress.ToString()}connect/token",
                ClientId = "full-access-client",
                ClientSecret = "secret",
                Scope = $"{IntegrationTestConstants.IntegrationTestProtectedResourceName}.full_access {IntegrationTestConstants.IntegrationTestProtectedResourceName}.read_only "
            });
        }

        public static async Task<TokenResponse> GetReadOnlyAccessToken(this HttpClient client)
        {
            return await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest()
            {
                Address = $"{client.BaseAddress.ToString()}connect/token",
                ClientId = "read-only-client",
                ClientSecret = "secret",
                Scope = $"{IntegrationTestConstants.IntegrationTestProtectedResourceName}.read_only"
            });
        }
    }
}
