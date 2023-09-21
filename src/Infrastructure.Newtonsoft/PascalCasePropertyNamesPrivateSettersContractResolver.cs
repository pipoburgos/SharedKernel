using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Reflection;

namespace SharedKernel.Infrastructure.Newtonsoft;

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
        // Convierte el nombre de la propiedad a PascalCase
        return ToPascalCase(propertyName);
    }

    private string ToPascalCase(string s)
    {
        if (string.IsNullOrEmpty(s))
            return s;

        var chars = s.ToCharArray();
        chars[0] = char.ToUpper(chars[0]);

        return new string(chars);
    }
}