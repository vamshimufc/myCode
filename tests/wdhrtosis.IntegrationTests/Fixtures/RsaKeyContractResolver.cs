using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace wdhrtosis.IntegrationTests.Fixtures
{
    internal class RsaKeyContractResolver : DefaultContractResolver
    {
        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var property = base.CreateProperty(member, memberSerialization);

            property.Ignored = false;

            return property;
        }
    }
}