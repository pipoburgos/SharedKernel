using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using SharedKernel.Application.Serializers;
using SharedKernel.Infrastructure.Newtonsoft.Resolvers;

namespace SharedKernel.Infrastructure.Newtonsoft;

/// <summary> . </summary>
public class NewtonsoftSerializer : IJsonSerializer
{
    /// <summary> . </summary>
    public async Task<T?> DeserializeAsync<T>(Stream stream,
        NamingConvention namingConvention = NamingConvention.CamelCase, CancellationToken cancellationToken = default)
    {
        var reader = new StreamReader(stream);

#if NET6_0_OR_GREATER
        var content = await reader.ReadToEndAsync(cancellationToken);
#else
        var content = await reader.ReadToEndAsync();
#endif

        return JsonConvert.DeserializeObject<T>(content, GetOptions(namingConvention))!;
    }

    /// <summary> . </summary>
    public string Serialize(object? value, NamingConvention namingConvention = NamingConvention.CamelCase)
    {
        return value == null ? string.Empty : JsonConvert.SerializeObject(value, GetOptions(namingConvention));
    }

    /// <summary> . </summary>
    public void Serialize<T>(T data, Stream stream, NamingConvention namingConvention = NamingConvention.CamelCase)
    {
        var writer = new StreamWriter(stream);
        writer.Write(JsonConvert.SerializeObject(data, GetOptions(namingConvention)));
    }

    /// <summary> . </summary>
    public Task SerializeAsync<T>(T data, Stream stream,
        NamingConvention namingConvention = NamingConvention.CamelCase, CancellationToken cancellationToken = default)
    {
        var writer = new StreamWriter(stream);
        return writer.WriteAsync(JsonConvert.SerializeObject(data, GetOptions(namingConvention)));
    }

    /// <summary> . </summary>
    public T Deserialize<T>(string value, NamingConvention namingConvention = NamingConvention.CamelCase)
    {
        return JsonConvert.DeserializeObject<T>(value, GetOptions(namingConvention))!;
    }

    /// <summary> . </summary>
    public object Deserialize(Type type, Stream stream, NamingConvention namingConvention = NamingConvention.CamelCase)
    {
        var reader = new StreamReader(stream);
        var content = reader.ReadToEnd();
        return JsonConvert.DeserializeObject(content, type, GetOptions(namingConvention))!;
    }

    /// <summary> . </summary>
    public T Deserialize<T>(Stream stream, NamingConvention namingConvention = NamingConvention.CamelCase)
    {
        var reader = new StreamReader(stream);
        var content = reader.ReadToEnd();
        return JsonConvert.DeserializeObject<T>(content, GetOptions(namingConvention))!;
    }

    /// <summary> . </summary>
    public async Task<object> DeserializeAsync(Type type, Stream stream,
        NamingConvention namingConvention = NamingConvention.CamelCase, CancellationToken cancellationToken = default)
    {
        var reader = new StreamReader(stream);
#if NET6_0_OR_GREATER
        var content = await reader.ReadToEndAsync(cancellationToken);
#else
        var content = await reader.ReadToEndAsync();
#endif

        return JsonConvert.DeserializeObject(content, type, GetOptions(namingConvention))!;
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
                contractResolver = new PropertyNamesPrivateSettersContractResolver();
                break;
            default:
                contractResolver = new PropertyNamesPrivateSettersContractResolver();
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
