using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using SharedKernel.Application.Serializers;
using SharedKernel.Infrastructure.Newtonsoft.Resolvers;

namespace SharedKernel.Infrastructure.Newtonsoft;

/// <summary> . </summary>
public class NewtonsoftSerializer : IJsonSerializer
{
    /// <summary> . </summary>
    public string Serialize(object? value, NamingConvention namingConvention = NamingConvention.CamelCase)
    {
        return value == null ? string.Empty : JsonConvert.SerializeObject(value, GetOptions(namingConvention));
    }

    /// <summary> . </summary>
    public T Deserialize<T>(string value, NamingConvention namingConvention = NamingConvention.CamelCase)
    {
        return JsonConvert.DeserializeObject<T>(value, GetOptions(namingConvention))!;
    }

    /// <summary> . </summary>
    public T DeserializeAnonymousType<T>(string value, T obj, NamingConvention namingConvention = NamingConvention.CamelCase)
    {
        return JsonConvert.DeserializeAnonymousType(value, obj, GetOptions(namingConvention))!;
    }

    /// <summary> . </summary>
    public static JsonSerializerSettings GetOptions(NamingConvention namingConvention)
    {
        IContractResolver? contractResolver;
        switch (namingConvention)
        {
            case NamingConvention.CamelCase:
                contractResolver = new CamelCasePropertyNamesPrivateSettersContractResolver();
                break;
            case NamingConvention.PascalCase:
                contractResolver = new PascalCasePropertyNamesPrivateSettersContractResolver();
                break;
            case NamingConvention.SnakeCase:
                contractResolver = new SnakeCasePropertyNamesPrivateSettersContractResolver();
                break;
            case NamingConvention.TrainCase:
                contractResolver = new TrainCasePropertyNamesPrivateSettersContractResolver();
                break;
            case NamingConvention.KebapCase:
                contractResolver = new KebapCasePropertyNamesPrivateSettersContractResolver();
                break;
            case NamingConvention.NoAction:
                contractResolver = new PascalCasePropertyNamesPrivateSettersContractResolver();
                break;
            default:
                contractResolver = new PascalCasePropertyNamesPrivateSettersContractResolver();
                break;
        }

        return new JsonSerializerSettings
        {
            DateTimeZoneHandling = DateTimeZoneHandling.Utc,
            ContractResolver = contractResolver,
            NullValueHandling = NullValueHandling.Ignore,
            ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
        };
    }
}