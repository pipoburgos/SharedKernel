using SharedKernel.Application.Serializers;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SharedKernel.Infrastructure.Serializers;

/// <summary>  </summary>
public class NetJsonSerializer : IJsonSerializer
{
    /// <summary>  </summary>
    public string Serialize(object value, NamingConvention namingConvention = NamingConvention.CamelCase)
    {

        return JsonSerializer.Serialize(value, GetOptions(namingConvention));
    }

    /// <summary>  </summary>
    public T Deserialize<T>(string value, NamingConvention namingConvention = NamingConvention.CamelCase)
    {
        return JsonSerializer.Deserialize<T>(value, GetOptions(namingConvention));
    }

    /// <summary>  </summary>
    public T DeserializeAnonymousType<T>(string value, T obj, NamingConvention namingConvention = NamingConvention.CamelCase)
    {
        return JsonSerializer.Deserialize<T>(value, GetOptions(namingConvention));
    }

    private JsonSerializerOptions GetOptions(NamingConvention namingConvention)
    {
        var policy = JsonNamingPolicy.CamelCase;

        switch (namingConvention)
        {
            case NamingConvention.CamelCase:
                policy = JsonNamingPolicy.CamelCase;
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
                policy = JsonNamingPolicy.CamelCase;
                break;
        }

        return new JsonSerializerOptions
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            PropertyNamingPolicy = policy
        };
    }
}
