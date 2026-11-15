using SharedKernel.Application.Serializers;
using SharedKernel.Infrastructure.NetJson.Converters;
using SharedKernel.Infrastructure.NetJson.Policies;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

namespace SharedKernel.Infrastructure.NetJson;

/// <summary> . </summary>
public class NetJsonSerializer : IJsonSerializer
{
    /// <summary> . </summary>
    public string Serialize(object? value, NamingConvention namingConvention = NamingConvention.CamelCase)
    {
        return JsonSerializer.Serialize(value, GetOptions(namingConvention));
    }

    public string Serialize(object? value, Type type, NamingConvention namingConvention = NamingConvention.CamelCase)
    {
        return JsonSerializer.Serialize(value, type, GetOptions(namingConvention));
    }

    /// <summary> . </summary>
    public void Serialize<T>(T data, Stream stream, NamingConvention namingConvention = NamingConvention.CamelCase)
    {
        JsonSerializer.Serialize(stream, data, GetOptions(namingConvention));
    }

    /// <summary> . </summary>
    public Task SerializeAsync<T>(T data, Stream stream,
        NamingConvention namingConvention = NamingConvention.CamelCase, CancellationToken cancellationToken = default)
    {
        return JsonSerializer.SerializeAsync(stream, data, GetOptions(namingConvention), cancellationToken);
    }

    /// <summary> . </summary>
    public T Deserialize<T>(string value, NamingConvention namingConvention = NamingConvention.CamelCase)
    {
        return JsonSerializer.Deserialize<T>(value, GetOptions(namingConvention))
            ?? throw new JsonException("Deserialization returned null.");
    }

    /// <summary> . </summary>
    public object Deserialize(Type type, Stream stream, NamingConvention namingConvention = NamingConvention.CamelCase)
    {
        return JsonSerializer.Deserialize(stream, type, GetOptions(namingConvention))
            ?? throw new JsonException("Deserialization returned null.");
    }

    /// <summary> . </summary>
    public T? Deserialize<T>(Stream stream, NamingConvention namingConvention = NamingConvention.CamelCase)
    {
        if (TryReturnDefault(stream, out T? deserialize))
            return deserialize;

        return JsonSerializer.Deserialize<T>(stream, GetOptions(namingConvention));
    }

    public object? Deserialize(string value, Type type, NamingConvention namingConvention = NamingConvention.CamelCase)
    {
        return JsonSerializer.Deserialize(value, type, GetOptions(namingConvention));
    }

    /// <summary> . </summary>
    public async Task<T?> DeserializeAsync<T>(Stream stream,
        NamingConvention namingConvention = NamingConvention.CamelCase, CancellationToken cancellationToken = default)
    {
        return await JsonSerializer.DeserializeAsync<T>(stream, GetOptions(namingConvention), cancellationToken)
               ?? throw new JsonException("Deserialization returned null.");
    }

    /// <summary> . </summary>
    public async Task<object> DeserializeAsync(Type type, Stream stream,
        NamingConvention namingConvention = NamingConvention.CamelCase, CancellationToken cancellationToken = default)
    {
        return await JsonSerializer.DeserializeAsync(stream, type, GetOptions(namingConvention), cancellationToken)
            ?? throw new JsonException("Deserialization returned null.");
    }

    /// <summary> . </summary>
    public T DeserializeAnonymousType<T>(string value, T obj, NamingConvention namingConvention = NamingConvention.CamelCase)
    {
        return JsonSerializer.Deserialize<T>(value, GetOptions(namingConvention))
            ?? throw new JsonException("Deserialization returned null.");
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
                policy = null;
                break;
            default:
                policy = null;
                break;
        }

        return GetOptions(policy);
    }

    public static JsonSerializerOptions GetOptions(JsonNamingPolicy? jsonNamingPolicy)
    {
        var resolver = new DefaultJsonTypeInfoResolver();

        resolver.Modifiers.Add(typeInfo =>
        {
            if (typeInfo.Kind != JsonTypeInfoKind.Object)
                return;

            foreach (var property in typeInfo.Type.GetProperties(
                         BindingFlags.Instance | BindingFlags.NonPublic))
            {
                if (property.GetMethod is null || property.GetMethod.IsPublic)
                    continue;

                var jsonProperty = typeInfo.CreateJsonPropertyInfo(
                    property.PropertyType,
                    property.Name);

                jsonProperty.Get = property.GetValue;
                jsonProperty.Set = property.SetValue;

                jsonProperty.Name = jsonNamingPolicy?.ConvertName(property.Name)
                                    ?? property.Name;

                typeInfo.Properties.Add(jsonProperty);
            }
        });

        return new JsonSerializerOptions
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            PropertyNamingPolicy = jsonNamingPolicy,
            TypeInfoResolver = resolver,
            Converters =
            {
                new SystemTypeConverter(),
                new DateTimeConverter(),
                new PrivateConstructorConverterFactory(),
            },
        };
    }

    /// <summary> . </summary>
    public static void SetOptions(JsonSerializerOptions jsonNamingPolicy, NamingConvention namingConvention)
    {
        jsonNamingPolicy.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        jsonNamingPolicy.PropertyNamingPolicy = GetOptions(namingConvention).PropertyNamingPolicy;
        jsonNamingPolicy.Converters.Add(new DateTimeConverter());
        jsonNamingPolicy.Converters.Add(new PrivateConstructorConverterFactory());
    }

    private static bool TryReturnDefault<T>(Stream? stream, out T? deserialize)
    {
        deserialize = default;
        return stream is null || stream == Stream.Null || (stream.CanSeek && stream.Length == 0);
    }
}
