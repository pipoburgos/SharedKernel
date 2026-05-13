using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.ObjectModel;
using System.Reflection;

namespace SharedKernel.Api.ServiceCollectionExtensions.OpenApi.SchemaFilters;

/// <summary> . </summary>
public class AssignPropertyRequiredSchemaFilter : ISchemaFilter
{
    /// <summary> . </summary>
    public void Apply(IOpenApiSchema schema, SchemaFilterContext context)
    {
        if (schema.Properties == null || schema.Properties.Count == 0)
            return;

        var typeProperties = context.Type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
        foreach (var property in schema.Properties)
        {
            if (typeProperties.Any(info =>
                    info.Name.Equals(property.Key, StringComparison.OrdinalIgnoreCase) && IsNullable(info)))
                continue;

            // "null", "boolean", "object", "array", "number", or "string"), or "integer" which matches any number with a zero fractional part.
            // see also: https://json-schema.org/latest/json-schema-validation.html#rfc.section.6.1.1
            switch (property.Value.Type)
            {
                case JsonSchemaType.Boolean:
                case JsonSchemaType.Integer:
                case JsonSchemaType.Number:
                    schema.Required?.Add(property.Key);
                    break;
                case JsonSchemaType.String:
                    switch (property.Value.Format)
                    {
                        case "date-time":
                        case "uuid":
                            schema.Required?.Add(property.Key);
                            break;
                    }
                    break;
                default:
                    if (schema.Type != JsonSchemaType.Object)
                        schema.Required?.Add(property.Key);
                    break;
            }
        }
    }

    private static bool IsNullable(PropertyInfo property)
    {
        if (property.PropertyType.IsValueType)
            return Nullable.GetUnderlyingType(property.PropertyType) != null;

        var nullable = property.CustomAttributes
            .FirstOrDefault(x => x.AttributeType.FullName == "System.Runtime.CompilerServices.NullableAttribute");

        if (nullable != null && nullable.ConstructorArguments.Count == 1)
        {
            var attributeArgument = nullable.ConstructorArguments[0];
            if (attributeArgument.ArgumentType == typeof(byte[]))
            {
                var args = (ReadOnlyCollection<CustomAttributeTypedArgument>)attributeArgument.Value!;
                if (args.Count > 0 && args[0].ArgumentType == typeof(byte))
                {
                    return (byte)args[0].Value! == 2;
                }
            }
            else if (attributeArgument.ArgumentType == typeof(byte))
            {
                return (byte)attributeArgument.Value! == 2;
            }
        }

        for (var type = property.DeclaringType; type != null; type = type.DeclaringType)
        {
            var context = type.CustomAttributes
                .FirstOrDefault(x =>
                    x.AttributeType.FullName == "System.Runtime.CompilerServices.NullableContextAttribute");
            if (context != null &&
                context.ConstructorArguments.Count == 1 &&
                context.ConstructorArguments[0].ArgumentType == typeof(byte))
            {
                return (byte)context.ConstructorArguments[0].Value! == 2;
            }
        }

        // Couldn't find a suitable attribute
        return false;
    }
}