using Newtonsoft.Json;
using SharedKernel.Application.Serializers;

namespace SharedKernel.Infrastructure.Newtonsoft;

/// <summary>  </summary>
public class NewtonsoftSerializer : IJsonSerializer
{
    /// <summary>  </summary>
    public string Serialize(object value, NamingConvention namingConvention = NamingConvention.CamelCase)
    {
        return JsonConvert.SerializeObject(value, GetOptions(namingConvention));
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

    private JsonSerializerSettings GetOptions(NamingConvention namingConvention)
    {
        var contractResolver = new CamelCasePropertyNamesPrivateSettersContractResolver();
        switch (namingConvention)
        {
            case NamingConvention.CamelCase:
                contractResolver = new CamelCasePropertyNamesPrivateSettersContractResolver();
                break;
            case NamingConvention.PascalCase:
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

        var x = new JsonSerializerSettings
        {
            DateTimeZoneHandling = DateTimeZoneHandling.Utc,
            ContractResolver = contractResolver,
            NullValueHandling = NullValueHandling.Ignore,
            ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor
        };

        return x;
    }


}