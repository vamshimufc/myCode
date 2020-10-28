using IdentityServer4.Models;
using System.Collections.Generic;

namespace wdhrtosis
{
    public static class DevelopmentIdentityServerConfiguration
    {
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new IdentityResource[]
            {
                new IdentityResources.OpenId()
            };
        }

        public static IEnumerable<ApiResource> GetApis(string protectedResourceName)
        {
            return new List<ApiResource>
            {
                 new ApiResource
                {
                    Name = protectedResourceName,
                    Description = "Workday HR to SIS",
                    Scopes =
                    {
                        new Scope
                        {
                            Name = $"{protectedResourceName}.full_access",
                            DisplayName = $"Full access to {protectedResourceName}"
                        },
                        new Scope
                        {
                            Name = $"{protectedResourceName}.read_only",
                            DisplayName = $"Read only access to {protectedResourceName}"
                        },
                    }
                }
            };
        }

        public static IEnumerable<Client> GetClients(string protectedResourceName)
        {
            return new List<Client>
            {
                new Client
                {
                    ClientId = "full-access-client",

                    // no interactive user, use the clientid/secret for authentication
                    AllowedGrantTypes = GrantTypes.ClientCredentials,

                    // secret for authentication
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },

                    // scopes that client has access to
                    AllowedScopes = { $"{protectedResourceName}.full_access", $"{protectedResourceName}.read_only" }
                },
                new Client
                {
                    ClientId = "read-only-client",

                    // no interactive user, use the clientid/secret for authentication
                    AllowedGrantTypes = GrantTypes.ClientCredentials,

                    // secret for authentication
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },

                    // scopes that client has access to
                    AllowedScopes = { $"{protectedResourceName}.read_only" }
                }
            };
        }
    }
}
