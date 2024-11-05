using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using SharedKernel.Application.Extensions;
using System.Reflection;

namespace SharedKernel.Infrastructure.Newtonsoft.Resolvers;

internal class PascalCasePropertyNamesPrivateSettersContractResolver : DefaultContractResolver
{
    protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
    {
        var prop = base.CreateProperty(member, memberSerialization);

        if (prop.Writable)
            return prop;

        var property = member as PropertyInfo;
        if (property == null)
            return prop;

        var hasPrivateSetter = property.GetSetMethod(true) != null;
        prop.Writable = hasPrivateSetter;

        return prop;
    }

    protected override string ResolvePropertyName(string propertyName)
    {
        return propertyName.ToCamelCase();
    }
}