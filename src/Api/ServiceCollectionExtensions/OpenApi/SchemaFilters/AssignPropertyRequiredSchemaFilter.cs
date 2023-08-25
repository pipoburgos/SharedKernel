using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.ObjectModel;
using System.Reflection;

namespace SharedKernel.Api.ServiceCollectionExtensions.OpenApi.SchemaFilters
{
    /// <summary>  </summary>
    public class AssignPropertyRequiredSchemaFilter : ISchemaFilter
    {
        /// <summary>  </summary>
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (schema.Properties == null || schema.Properties.Count == 0)
            {
                return;
            }

            var typeProperties = context.Type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var property in schema.Properties)
            {
                if (IsSourceTypePropertyNullable(typeProperties, property.Key))// || property.Value.Nullable)
                {
                    continue;
                }

                // "null", "boolean", "object", "array", "number", or "string"), or "integer" which matches any number with a zero fractional part.
                // see also: https://json-schema.org/latest/json-schema-validation.html#rfc.section.6.1.1
                switch (property.Value.Type)
                {
                    case "boolean":
                    case "integer":
                    case "number":
                        AddPropertyToRequired(schema, property.Key);
                        break;
                    case "string":
                        switch (property.Value.Format)
                        {
                            case "date-time":
                            case "uuid":
                                AddPropertyToRequired(schema, property.Key);
                                break;
                                //default:
                                //    AddPropertyToRequired(schema, property.Key);
                                //    break;
                        }
                        break;
                    default:
                        if (schema.Type != "object")
                            AddPropertyToRequired(schema, property.Key);
                        break;
                }
            }
        }

        private bool IsSourceTypePropertyNullable(PropertyInfo[] typeProperties, string propertyName)
        {
            return typeProperties.Any(info =>
                info.Name.Equals(propertyName, StringComparison.OrdinalIgnoreCase) && IsNullable(info));
        }

        private void AddPropertyToRequired(OpenApiSchema schema, string propertyName)
        {
            schema.Required ??= new HashSet<string>();
            schema.Required.Add(propertyName);
        }

        private static bool IsNullable(PropertyInfo property) => IsNullableHelper(property.PropertyType,
            property.DeclaringType, property.CustomAttributes);

        //private static bool IsNullable(FieldInfo field) =>
        //    IsNullableHelper(field.FieldType, field.DeclaringType, field.CustomAttributes);

        //private static bool IsNullable(ParameterInfo parameter) =>
        //    IsNullableHelper(parameter.ParameterType, parameter.Member, parameter.CustomAttributes);

        private static bool IsNullableHelper(Type memberType, MemberInfo? declaringType,
            IEnumerable<CustomAttributeData> customAttributes)
        {
            if (memberType.IsValueType)
                return Nullable.GetUnderlyingType(memberType) != null;

            var nullable = customAttributes
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

            for (var type = declaringType; type != null; type = type.DeclaringType)
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
}
