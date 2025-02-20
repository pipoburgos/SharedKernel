using SharedKernel.Application.Serializers;
using SharedKernel.Infrastructure.NetJson.Converters;
using SharedKernel.Infrastructure.NetJson.Policies;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SharedKernel.Infrastructure.NetJson;

/// <summary> . </summary>
public class NetJsonSerializer : IJsonSerializer
{
    /// <summary> . </summary>
    public string Serialize(object? value, NamingConvention namingConvention = NamingConvention.CamelCase)
    {
        return value == null ? string.Empty : JsonSerializer.Serialize(value, GetOptions(namingConvention));
    }

    /// <summary> . </summary>
    public T Deserialize<T>(string value, NamingConvention namingConvention = NamingConvention.CamelCase)
    {
        return JsonSerializer.Deserialize<T>(value, GetOptions(namingConvention))!;
    }

    /// <summary> . </summary>
    public T DeserializeAnonymousType<T>(string value, T obj, NamingConvention namingConvention = NamingConvention.CamelCase)
    {
        return JsonSerializer.Deserialize<T>(value, GetOptions(namingConvention))!;
    }

    /// <summary> . </summary>
    public static JsonSerializerOptions GetOptions(NamingConvention namingConvention)
    {
        JsonNamingPolicy? policy;

        switch (namingConvention)
        {
            case NamingConvention.CamelCase:
                policy = JsonNamingPolicy.CamelCase;
                break;
            case NamingConvention.PascalCase:
                policy = new PascalCaseNamingPolicy();
                break;
            case NamingConvention.SnakeCase:
                policy = JsonNamingPolicy.SnakeCaseLower;
                break;
            case NamingConvention.TrainCase:
                policy = new TrainCaseNamingPolicy();
                break;
            case NamingConvention.KebapCase:
                policy = JsonNamingPolicy.KebabCaseLower;
                break;
            case NamingConvention.NoAction:
                policy = new PascalCaseNamingPolicy();
                break;
            default:
                policy = new PascalCaseNamingPolicy();
                break;
        }

        return new JsonSerializerOptions
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            PropertyNamingPolicy = policy,
            Converters =
            {
                new DateTimeConverter(),
                //new NoSetterConverterFactory(),
                new ConstructorConverterFactory(),
            },
        };
    }

}