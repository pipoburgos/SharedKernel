//using System.Text.Json;
//using System.Text.Json.Serialization;

//namespace SharedKernel.Infrastructure.NetJson.Converters;

///// <summary>  </summary>
//public class NoSetterConverter<T> : JsonConverter<T> where T : class
//{
//    /// <summary>  </summary>
//    public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
//    {
//        var instance = new T();
//        var properties = typeof(T).GetProperties();

//        using var doc = JsonDocument.ParseValue(ref reader);
//        var root = doc.RootElement;

//        foreach (var property in properties)
//        {
//            if (root.TryGetProperty(property.Name, out var value))
//            {
//                property.SetValue(instance, value.GetString());
//            }
//        }

//        return instance;
//    }

//    /// <summary>  </summary>
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
