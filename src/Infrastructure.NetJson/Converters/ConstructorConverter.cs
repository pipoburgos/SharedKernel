using System.Collections;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SharedKernel.Infrastructure.NetJson.Converters;

/// <summary> . </summary>
public class ConstructorConverter<T> : JsonConverter<T>
{
    private readonly JsonNamingPolicy? _namingPolicy;

    /// <summary> . </summary>
    public ConstructorConverter(JsonNamingPolicy? namingPolicy = null)
    {
        _namingPolicy = namingPolicy;
    }

    /// <summary> . </summary>
    public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using var doc = JsonDocument.ParseValue(ref reader);
        var root = doc.RootElement;

        var constructor = typeof(T)
            .GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
            .OrderByDescending(c => c.GetParameters().Length)
            .FirstOrDefault();

        if (constructor == null)
        {
            throw new JsonException($"No se encontró un constructor válido en {typeof(T).Name}");
        }

        var parameters = constructor.GetParameters();
        var args = new object?[parameters.Length];

        for (var i = 0; i < parameters.Length; i++)
        {
            var jsonPropertyName = _namingPolicy?.ConvertName(parameters[i].Name!) ?? parameters[i].Name!;

            var paramType = parameters[i].ParameterType;

            if (root.TryGetProperty(jsonPropertyName, out var element))
            {
                if (paramType == typeof(Guid) && element.ValueKind == JsonValueKind.String && Guid.TryParse(element.GetString(), out var guidValue))
                {
                    args[i] = guidValue;
                }
                else if (paramType == typeof(DateTime) && element.ValueKind == JsonValueKind.String && DateTime.TryParse(element.GetString(), out var dateTimeValue))
                {
                    // Convertir a UTC
                    args[i] = dateTimeValue.ToUniversalTime();
                }
                else if (paramType.IsArray && element.ValueKind == JsonValueKind.Array)
                {
                    // Si el parámetro es un array, procesamos el array
                    var arrayType = paramType.GetElementType(); // Tipo de los elementos dentro del array
                    if (arrayType != null)
                    {
                        var arrayList = new List<object>();
                        foreach (var item in element.EnumerateArray())
                        {
                            // Deserializamos cada elemento del array en su tipo adecuado
                            arrayList.Add(JsonSerializer.Deserialize(item.GetRawText(), arrayType, options));
                        }
                        // Asignamos el array resultante
                        args[i] = arrayList.ToArray();
                    }
                    else
                    {
                        throw new JsonException("No se pudo determinar el tipo de los elementos del array para deserializar.");
                    }
                }
                else if (element.ValueKind == JsonValueKind.String)
                {
                    // Si el valor es una cadena, lo asignamos directamente
                    args[i] = element.GetString();
                }
                else
                {
                    // Deserialización general en el tipo correspondiente
                    args[i] = JsonSerializer.Deserialize(element.GetRawText(), paramType, options);
                }

            }
            else
            {
                args[i] = null;
            }
        }

        var instance = (T)constructor.Invoke(args);

        PopulateFieldsAndProperties(instance, root, options);

        return instance;
    }

    private void PopulateFieldsAndProperties<T>(T instance, JsonElement root, JsonSerializerOptions? options)
    {
        var type = typeof(T);

        // Obtener todas las propiedades (públicas y privadas)
        var properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
            .Where(p => p.CanWrite);

        foreach (var prop in properties)
        {
            var jsonPropertyName = _namingPolicy?.ConvertName(prop.Name) ?? prop.Name;

            if (root.TryGetProperty(jsonPropertyName, out var element))
            {
                var value = DeserializeJsonValue(element, prop.PropertyType, options);
                prop.SetValue(instance, value);
            }
        }

        // Obtener todos los campos (incluyendo privados)
        var fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

        foreach (var field in fields)
        {
            var jsonPropertyName = _namingPolicy?.ConvertName(field.Name) ?? field.Name;

            if (root.TryGetProperty(jsonPropertyName, out var element))
            {
                var value = DeserializeJsonValue(element, field.FieldType, options);
                field.SetValue(instance, value);
            }
        }
    }

    private static object? DeserializeJsonValue(JsonElement element, Type targetType, JsonSerializerOptions? options)
    {
        if (targetType == typeof(Guid) && element.ValueKind == JsonValueKind.String && Guid.TryParse(element.GetString(), out var guidValue))
        {
            return guidValue;
        }

        if (targetType == typeof(DateTime) && element.ValueKind == JsonValueKind.String)
        {
            // Intenta parsear el valor de la fecha en UTC
            if (DateTime.TryParse(element.GetString(), out var dateTimeValue))
            {
                // Convertir a UTC si no lo está ya
                return dateTimeValue.ToUniversalTime();
            }
            // Si el parseo falla, se puede manejar el error o lanzar una excepción si es necesario

            throw new JsonException($"Error al deserializar la fecha: {element.GetString()}");
        }

        if (element.ValueKind == JsonValueKind.Array)
        {
            // Si el parámetro es un array, procesamos el array
            var arrayType = targetType.GetElementType(); // Tipo de los elementos dentro del array

            if (arrayType == null)
                return GetDefaultValue(targetType);

            var arrayList = new List<object>();
            foreach (var item in element.EnumerateArray())
            {
                // Deserializamos cada elemento del array en su tipo adecuado
                arrayList.Add(JsonSerializer.Deserialize(item.GetRawText(), arrayType, options));
            }
            // Asignamos el array resultante
            return arrayList.ToArray();
        }

        if (targetType == typeof(string) && element.ValueKind == JsonValueKind.String)
        {
            return element.GetString();
        }

        return JsonSerializer.Deserialize(element.GetRawText(), targetType, options);
    }

    private static object GetDefaultValue(Type targetType)
    {
        var defaultValue = targetType.IsGenericType && targetType.GetGenericTypeDefinition() == typeof(List<>)
            ? Activator.CreateInstance(targetType)  // Crea una lista vacía, como List<string>
            : targetType.IsValueType ? Activator.CreateInstance(targetType) : null;  // Para otros tipos


        return defaultValue;
    }

    /// <summary> . </summary>
    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
        if (value == null)
            return;

        writer.WriteStartObject();

        if (value is IDictionary dic)
        {
            foreach (var key in dic.Keys)
            {
                writer.WritePropertyName(key?.ToString() ?? "null"); // Asegurarse de que la clave esté en formato correcto
                JsonSerializer.Serialize(writer, dic[key], options);
            }
        }
        else
        {
            var properties = value.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Where(p => p.CanRead)
                .ToList();

            foreach (var property in properties)
            {
                //if (property.PropertyType.IsClass && !property.PropertyType.IsValueType)
                //{
                //    JsonSerializer.Serialize(writer, property.GetValue(value), property.PropertyType, options);
                //}
                //else
                //{
                var propertyValue = property.GetValue(value);

                var jsonPropertyName = _namingPolicy?.ConvertName(property.Name) ?? property.Name;

                writer.WritePropertyName(jsonPropertyName);

                if (propertyValue != null)
                {
                    if (propertyValue is string stringValue)
                    {
                        writer.WriteStringValue(stringValue);
                    }
                    else if (propertyValue is IDictionary dictionary)
                    {
                        // Manejo especial para Dictionary
                        writer.WriteStartObject();
                        foreach (var key in dictionary.Keys)
                        {
                            writer.WritePropertyName(key?.ToString() ?? "null"); // Asegurarse de que la clave esté en formato correcto
                            JsonSerializer.Serialize(writer, dictionary[key], options);
                        }
                        writer.WriteEndObject();
                    }
                    else
                    {
                        JsonSerializer.Serialize(writer, propertyValue, property.PropertyType, options);
                    }
                }
                else
                {
                    writer.WriteNullValue();
                }
                //}
            }
        }
        writer.WriteEndObject();
    }

}
