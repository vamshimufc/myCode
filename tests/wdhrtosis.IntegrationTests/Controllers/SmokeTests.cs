using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using wdhrtosis.IntegrationTests.Fixtures;
using FluentAssertions;
using IdentityModel.Client;
using Xunit;

namespace wdhrtosis.IntegrationTests.Controllers
{
    public class SmokeTests : IClassFixture<CustomWebApplicationFactory<Startup>>, IDisposable
    {
        private readonly HttpClient client;

        public SmokeTests(CustomWebApplicationFactory<Startup> factory)
        {
            client = factory.CreateClient();
        }

        [Fact]
        public async Task Development_IS4_Discovery_Success()
        {
            var response = await client.GetDiscoveryDocumentAsync();
            response.IsError.Should().BeFalse();
            response.TokenEndpoint.Should().NotBeNullOrWhiteSpace();
        }

        //[Fact]
        //public async Task Welcome_Valid_Returns_SuccessStatusCode()
        //{
        //    var response = await client.GetAsync("/");
        //    response.EnsureSuccessStatusCode();
        //}

        [Fact]
        public async Task GetLog_Success()
        {
            await client.SetReadOnlyBearerToken();
            var response = await client.GetAsync("api/log");
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task GetDRA1Status_Success()
        {
            await client.SetReadOnlyBearerToken();
            var response = await client.GetAsync("api/DRA1/Status");
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task GetDRA1Status_Returns_401Unauthorized_When_Not_Sending_A_Token()
        {
            var response = await client.GetAsync("api/DRA1/Status");
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task GetDRA1Help_Success()
        {
            await client.SetReadOnlyBearerToken();
            var response = await client.GetAsync("api/DRA1");
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task PostDRA1Run_Success()
        {
            await client.SetFullAccessBearerToken();
            var response = await client.PostAsync("api/DRA1/runtask", null);
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task PostDRA1Run_403Unauthorized_When_Using_Read_Only_Token()
        {
            await client.SetReadOnlyBearerToken();
            var response = await client.PostAsync("api/DRA1/runtask", null);
            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task Log_Returns_Request_log()
        {
            await client.SetFullAccessBearerToken();
            var response = await client.GetAsync("api/log");
            response.EnsureSuccessStatusCode();
        }

        public void Dispose()
        {
            client.SetBearerToken(string.Empty);
        }
    }
}