using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using SharedKernel.Application.Serializers;

namespace SharedKernel.Infrastructure.Newtonsoft;

/// <summary>  </summary>
public class NewtonsoftSerializer : IJsonSerializer
{
    /// <summary>  </summary>
    public string Serialize(object? value, NamingConvention namingConvention = NamingConvention.CamelCase)
    {
        return value == null ? string.Empty : JsonConvert.SerializeObject(value, GetOptions(namingConvention));
    }

    /// <summary>  </summary>
    public T Deserialize<T>(string value, NamingConvention namingConvention = NamingConvention.CamelCase)
    {
        return JsonConvert.DeserializeObject<T>(value, GetOptions(namingConvention))!;
    }

    /// <summary>  </summary>
    public T DeserializeAnonymousType<T>(string value, T obj, NamingConvention namingConvention = NamingConvention.CamelCase)
    {
        return JsonConvert.DeserializeAnonymousType(value, obj, GetOptions(namingConvention))!;
    }

    /// <summary>  </summary>
    public static JsonSerializerSettings GetOptions(NamingConvention namingConvention)
    {
        IContractResolver contractResolver = new CamelCasePropertyNamesPrivateSettersContractResolver();
        switch (namingConvention)
        {
            case NamingConvention.CamelCase:
                contractResolver = new CamelCasePropertyNamesPrivateSettersContractResolver();
                break;
            case NamingConvention.PascalCase:
                contractResolver = new PascalCasePropertyNamesPrivateSettersContractResolver();
                break;
            case NamingConvention.SnakeCase:
                break;
            case NamingConvention.TrainCase:
                break;
            case NamingConvention.KebapCase:
                break;
            default:
                contractResolver = new CamelCasePropertyNamesPrivateSettersContractResolver();
                break;
        }

        return new JsonSerializerSettings
        {
            DateTimeZoneHandling = DateTimeZoneHandling.Utc,
            ContractResolver = contractResolver,
            NullValueHandling = NullValueHandling.Ignore,
            ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor
        };
    }
}