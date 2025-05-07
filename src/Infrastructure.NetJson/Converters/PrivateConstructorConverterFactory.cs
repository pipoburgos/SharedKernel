using System.Text.Json;
using System.Text.Json.Serialization;
using DateTime = System.DateTime;

namespace SharedKernel.Infrastructure.NetJson.Converters;

/// <summary> . </summary>
public class PrivateConstructorConverterFactory : JsonConverterFactory
{
    /// <summary> . </summary>
    public override bool CanConvert(Type typeToConvert)
    {
        return typeToConvert.IsClass && typeToConvert.GetConstructors().Length == 0 && typeToConvert != typeof(DateTime);
    }

    /// <summary> . </summary>
    public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        var converterType = typeof(PrivateConstructorConverter<>).MakeGenericType(typeToConvert);
        var converter = Activator.CreateInstance(converterType, options.PropertyNamingPolicy);

        return (JsonConverter)converter!;
    }
}

