using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using wdhrtosis.Options;
using EA.Serilog.Sinks.StaticRolling;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Serilog;

namespace wdhrtosis.IntegrationTests.Fixtures
{
    public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup>
        where TStartup : class
    {
        public IServiceScope serviceScope;

        protected override void ConfigureClient(HttpClient client) =>
            client.BaseAddress = new Uri("http://localhost");

        protected override TestServer CreateServer(IWebHostBuilder webHostBuilder)
        {
            webHostBuilder
                .ConfigureAppConfiguration(config => 
                    config.Add(new MemoryConfigurationSource() { InitialData = new Dictionary<string, string>
                        {
                            { $"{nameof(AuthOptions)}:{nameof(AuthOptions.Authority)}", "http://localhost" },
                            { $"{nameof(AuthOptions)}:{nameof(AuthOptions.ProtectedResourceName)}", IntegrationTestConstants.IntegrationTestProtectedResourceName },
                        }
                    }));

            webHostBuilder
                .ConfigureServices(
                services =>
                {
                    //Use SQLite in-memory database(s) in order for tests to run without db connection errors in Bamboo builds.
                    //In DRA application, modify for the actual dbcontexts needed.

                    // Create a new service provider.
                    var dbContextInternalServiceProvider = new ServiceCollection()
                        .AddEntityFrameworkSqlite()
                        .BuildServiceProvider();

                    // Add a database context using an in-memory database for testing 
                    // (registering over the top of the previous DbContext(s)).
                    // Change database context as required and add more if needed.
                    services.AddDbContext<wdhrtosis.Data.PersonImportContext>(options =>
                    {
                        options
                            .UseSqlite("Data Source=InMemoryDbForTesting-Empty.db")
                            .UseInternalServiceProvider(dbContextInternalServiceProvider);
                    });

                    IdentityModelEventSource.ShowPII = true;
                })
                .ConfigureTestServices(
                services =>
                {
                    var keyFile = File.ReadAllText("./tempkey.rsa");
                    var tempKey = JsonConvert.DeserializeObject<TemporaryRsaKey>(keyFile, new JsonSerializerSettings { ContractResolver = new RsaKeyContractResolver() });

                    var tokenValidationParams = new TokenValidationParameters()
                    {
                        ValidIssuer = "http://localhost",
                        IssuerSigningKey = IdentityServerBuilderExtensionsCrypto.CreateRsaSecurityKey(tempKey.Parameters, tempKey.KeyId),
                        ValidAudience = IntegrationTestConstants.IntegrationTestProtectedResourceName,
                        ValidateLifetime = true
                    };

                    services
                    // Set the new default to Integration for testing.
                    .AddAuthentication(IntegrationTestConstants.IntegrationTestDefaultAuthenticationScheme)
                    // Register a new handler for Integration
                    .AddJwtBearer(IntegrationTestConstants.IntegrationTestDefaultAuthenticationScheme, "Integration Testing Auth Scheme", options =>
                     {
                         options.TokenValidationParameters = tokenValidationParams;
                     });

                    // This is required to initialize the StaticRolling singleton
                    Log.Logger = new LoggerConfiguration()
                        .WriteTo.StaticRolling()
                        .CreateLogger();
                });

            var testServer = base.CreateServer(webHostBuilder);

            return testServer;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                serviceScope?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}