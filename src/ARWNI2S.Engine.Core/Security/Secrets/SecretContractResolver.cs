using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Reflection;

namespace ARWNI2S.Security.Secrets
{
    internal sealed class SecretContractResolver : DefaultContractResolver
    {
        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var property = base.CreateProperty(member, memberSerialization);

            if (member.CustomAttributes.Any(a => a.AttributeType == typeof(SecretAttribute)))
            {
                property.ValueProvider = new SecretValueProvider(member as PropertyInfo);
            }

            return property;
        }
    }
}
