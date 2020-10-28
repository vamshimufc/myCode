using System.Security.Cryptography;

namespace wdhrtosis.IntegrationTests.Fixtures
{
    // used for serialization to temporary RSA key
    // See https://github.com/IdentityServer/IdentityServer4/blob/master/src/IdentityServer4/src/Configuration/DependencyInjection/BuilderExtensions/Crypto.cs
    internal class TemporaryRsaKey
    {
        public string KeyId { get; set; }
        public RSAParameters Parameters { get; set; }
    }
}