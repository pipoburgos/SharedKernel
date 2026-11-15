using System.Text.Json;
using System.Text.Json.Serialization;

namespace SharedKernel.Infrastructure.NetJson.Converters;

public sealed class SystemTypeConverter : JsonConverter<Type>
{
    public override Type? Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options)
    {
        var typeName = reader.GetString();

        if (string.IsNullOrWhiteSpace(typeName))
            return null;

        return Type.GetType(typeName)
               ?? throw new JsonException($"No se encontró el tipo: {typeName}");
    }

    public override void Write(
        Utf8JsonWriter writer,
        Type value,
        JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.AssemblyQualifiedName);
    }
}