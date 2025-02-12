//using SharedKernel.Application.Reflection;
//using System.Text.Json;
//using System.Text.Json.Serialization;

//namespace SharedKernel.Infrastructure.NetJson.Converters;

///// <summary> . </summary>
//public class NoSetterConverterFactory : JsonConverterFactory
//{
//    /// <summary> . </summary>
//    public override bool CanConvert(Type typeToConvert)
//    {
//        return typeToConvert.IsClass && typeToConvert.HasParameterlessConstructor();
//    }

//    /// <summary> . </summary>
//    public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
//    {
//        var converterType = typeof(NoSetterConverter<>).MakeGenericType(typeToConvert);
//        var converter = Activator.CreateInstance(converterType);

//        return (JsonConverter)converter!;
//    }
//}
