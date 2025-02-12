using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SharedKernel.Infrastructure.NetJson.Converters;

/// <summary> . </summary>
public class ConstructorConverter<T> : JsonConverter<T>
{
    /// <summary> . </summary>
    public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
        {
            throw new JsonException($"Esperado un objeto JSON para {typeof(T).Name}");
        }

        // Obtener el constructor con más parámetros
        var constructor = typeof(T).GetConstructors().OrderByDescending(c => c.GetParameters().Length).FirstOrDefault();
        if (constructor == null)
        {
            throw new JsonException($"No se encontró un constructor válido en {typeof(T).Name}");
        }

        var parameters = constructor.GetParameters();
        var args = new object?[parameters.Length];

        // Leer JSON como diccionario
        using var doc = JsonDocument.ParseValue(ref reader);
        var jsonObject = doc.RootElement;

        for (var i = 0; i < parameters.Length; i++)
        {
            var paramName = parameters[i].Name!;
            var paramType = parameters[i].ParameterType;

            if (jsonObject.TryGetProperty(paramName, out var element))
            {
                args[i] = JsonSerializer.Deserialize(element.GetRawText(), paramType, options);
            }
            else if (parameters[i].HasDefaultValue)
            {
                args[i] = parameters[i].DefaultValue;
            }
            else
            {
                throw new JsonException($"No se encontró el valor requerido para '{paramName}' en {typeof(T).Name}");
            }
        }


        var instance = (T)constructor.Invoke(args);

        var properties = typeof(T).GetProperties();

        var root = doc.RootElement;

        foreach (var property in properties)
        {
            if (root.TryGetProperty(property.Name, out var value))
            {
                property.SetValue(instance, value.GetString());
            }
        }

        return instance;
    }

    /// <summary> . </summary>
    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
        // Escribimos el JSON manualmente para evitar la recursión infinita
        writer.WriteStartObject();

        // Obtenemos las propiedades públicas de la clase T
        var properties = value.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public)
            .Where(p => p.CanRead)
            .ToList();

        foreach (var property in properties)
        {
            // Obtenemos el valor de la propiedad
            var propertyValue = property.GetValue(value);

            // Escribimos el nombre de la propiedad en el JSON
            writer.WritePropertyName(property.Name);

            // Verificamos si la propiedad tiene un valor
            if (propertyValue != null)
            {
                // Si la propiedad es de tipo string, la escribimos directamente como una cadena
                if (propertyValue is string stringValue)
                {
                    writer.WriteStringValue(stringValue);
                }
                else
                {
                    // Si la propiedad es de cualquier otro tipo, la serializamos usando JsonSerializer
                    JsonSerializer.Serialize(writer, propertyValue, property.PropertyType, options);
                }
            }
            else
            {
                // Si la propiedad es nula, escribimos un valor nulo
                writer.WriteNullValue();
            }
        }

        writer.WriteEndObject();
    }

}

