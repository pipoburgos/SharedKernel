using SharedKernel.Application.Serializers;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SharedKernel.Infrastructure.NetJson;

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
        return JsonSerializer.Deserialize<T>(value, GetOptions(namingConvention))!;
    }

    /// <summary>  </summary>
    public T DeserializeAnonymousType<T>(string value, T obj, NamingConvention namingConvention = NamingConvention.CamelCase)
    {
        return JsonSerializer.Deserialize<T>(value, GetOptions(namingConvention))!;
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
            //Converters = { new NoSetterResolver() },
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            PropertyNamingPolicy = policy
        };
    }

    //private class NoSetterResolver : JsonConverterFactory
    //{
    //    public override bool CanConvert(Type typeToConvert)
    //    {
    //        return typeToConvert.IsClass;
    //    }

    //    public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    //    {
    //        var converterType = typeof(NoSetterConverter<>).MakeGenericType(typeToConvert);
    //        var converter = Activator.CreateInstance(converterType);

    //        return (JsonConverter)converter;
    //    }
    //}

    //private class NoSetterConverter<T> : JsonConverter<T> where T : class, new()
    //{
    //    public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    //    {
    //        var instance = new T();
    //        var properties = typeof(T).GetProperties();

    //        using (var doc = JsonDocument.ParseValue(ref reader))
    //        {
    //            var root = doc.RootElement;

    //            foreach (var property in properties)
    //            {
    //                if (root.TryGetProperty(property.Name, out var value))
    //                {
    //                    property.SetValue(instance, value.GetString());
    //                }
    //            }
    //        }

    //        return instance;
    //    }

    //    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    //    {
    //        writer.WriteStartObject();

    //        foreach (var property in typeof(T).GetProperties())
    //        {
    //            writer.WritePropertyName(property.Name);
    //            JsonSerializer.Serialize(writer, property.GetValue(value), options);
    //        }

    //        writer.WriteEndObject();
    //    }
    //}
}
