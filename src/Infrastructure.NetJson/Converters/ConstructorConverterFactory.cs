using System.Text.Json;
using System.Text.Json.Serialization;

namespace SharedKernel.Infrastructure.NetJson.Converters;

/// <summary> . </summary>
public class ConstructorConverterFactory : JsonConverterFactory
{
    /// <summary> . </summary>
    public override bool CanConvert(Type typeToConvert)
    {
        return typeToConvert.IsClass;// && !typeToConvert.HasParameterlessConstructor();
    }

    /// <summary> . </summary>
    public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        var converterType = typeof(ConstructorConverter<>).MakeGenericType(typeToConvert);
        var converter = Activator.CreateInstance(converterType, options.PropertyNamingPolicy);

        return (JsonConverter)converter!;
    }
}

