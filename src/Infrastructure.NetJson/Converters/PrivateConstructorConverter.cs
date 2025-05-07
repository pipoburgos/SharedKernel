using SharedKernel.Application.Reflection;
using SharedKernel.Domain.Extensions;
using System.Collections;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace SharedKernel.Infrastructure.NetJson.Converters;

/// <summary> . </summary>
public class PrivateConstructorConverter<T> : JsonConverter<T>
{
    private readonly JsonNamingPolicy? _namingPolicy;

    /// <summary> . </summary>
    public PrivateConstructorConverter(JsonNamingPolicy? namingPolicy = null)
    {
        _namingPolicy = namingPolicy;
    }

    /// <summary> . </summary>
    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
        var newOptions = new JsonSerializerOptions
        {
            DefaultBufferSize = options.DefaultBufferSize,
            DictionaryKeyPolicy = options.DictionaryKeyPolicy,
            Encoder = options.Encoder,
            IgnoreReadOnlyProperties = options.IgnoreReadOnlyProperties,
            IncludeFields = options.IncludeFields,
            MaxDepth = options.MaxDepth,
            PropertyNameCaseInsensitive = options.PropertyNameCaseInsensitive,
            PropertyNamingPolicy = options.PropertyNamingPolicy,
            ReadCommentHandling = options.ReadCommentHandling,
            ReferenceHandler = options.ReferenceHandler,
            WriteIndented = options.WriteIndented,
        };

        foreach (var converter in options.Converters.Where(c => c.GetType() != typeof(PrivateConstructorConverterFactory)))
        {
            if (converter != this)
            {
                newOptions.Converters.Add(converter);
            }
        }

        JsonSerializer.Serialize(writer, value, newOptions);
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
                if (paramType == typeof(Guid) && element.ValueKind == JsonValueKind.String &&
                    Guid.TryParse(element.GetString(), out var guidValue))
                {
                    args[i] = guidValue;
                }
                else if (paramType == typeof(DateTime) && element.ValueKind == JsonValueKind.String &&
                         DateTime.TryParse(element.GetString(), out var dateTimeValue))
                {
                    args[i] = dateTimeValue.ToUniversalTime();
                }
                else if (paramType.IsArray && element.ValueKind == JsonValueKind.Array)
                {
                    var arrayType = paramType.GetElementType();
                    if (arrayType != null)
                    {
                        var arrayList = new List<object?>();
                        foreach (var item in element.EnumerateArray())
                        {
                            arrayList.Add(JsonSerializer.Deserialize(item.GetRawText(), arrayType, options));
                        }
                        args[i] = arrayList.ToArray();
                    }
                    else
                    {
                        throw new JsonException("No se pudo determinar el tipo de los elementos del array para deserializar.");
                    }
                }
                else if (element.ValueKind == JsonValueKind.String)
                {
                    args[i] = element.GetString();
                }
                else
                {
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

    private void PopulateFieldsAndProperties(T instance, JsonElement root, JsonSerializerOptions? options)
    {
        var fields = typeof(T).GetAllFields();
        foreach (var field in fields)
        {
            var name = field.Name.Contains(">k__BackingField")
                ? Regex.Match(field.Name, @"<(.+?)>").Groups[1].Value
                : field.Name.Substring(1).CapitalizeFirstLetter();

            var jsonPropertyName = _namingPolicy?.ConvertName(name) ?? name;

            if (!root.TryGetProperty(jsonPropertyName, out var element))
                continue;

            var value = DeserializeJsonValue(element, field.FieldType, options);
            field.SetValue(instance, value);
        }
    }

    private static object? DeserializeJsonValue(JsonElement element, Type targetType, JsonSerializerOptions? options)
    {
        if (targetType == typeof(Guid) && element.ValueKind == JsonValueKind.String &&
            Guid.TryParse(element.GetString(), out var guidValue))
        {
            return guidValue;
        }

        if (targetType == typeof(DateTime) && element.ValueKind == JsonValueKind.String)
        {
            if (DateTime.TryParse(element.GetString(), out var dateTimeValue))
                return dateTimeValue.ToUniversalTime();

            throw new JsonException($"Error al deserializar la fecha: {element.GetString()}");
        }

        if (element.ValueKind == JsonValueKind.Array)
        {
            var isGenericList = targetType.IsGenericType &&
                                targetType.GetGenericTypeDefinition() == typeof(List<>);

            if (!isGenericList)
                return GetDefaultValue(targetType);

            var listType = targetType.GetGenericArguments()[0];
            var arrayList = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(listType))!;

            foreach (var item in element.EnumerateArray())
            {
                arrayList.Add(JsonSerializer.Deserialize(item.GetRawText(), listType, options));
            }

            return arrayList;
        }

        if (targetType == typeof(string) && element.ValueKind == JsonValueKind.String)
        {
            return element.GetString();
        }

        return JsonSerializer.Deserialize(element.GetRawText(), targetType, options);
    }

    private static object? GetDefaultValue(Type targetType)
    {
        var defaultValue = targetType.IsGenericType && targetType.GetGenericTypeDefinition() == typeof(List<>)
            ? Activator.CreateInstance(targetType)
            : targetType.IsValueType ? Activator.CreateInstance(targetType) : null;

        return defaultValue;
    }
}
