using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

namespace SharedKernel.Api.Gateway.ServiceCollectionExtensions.OpenApi
{
    /// <summary>  </summary>
    public class AssignPropertyRequiredFilter : ISchemaFilter
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
                if (IsSourceTypePropertyNullable(typeProperties, property.Key) || property.Value.Nullable)
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
                        }
                        break;
                    default:
                        if (schema.Type != "object")
                            AddPropertyToRequired(schema, property.Key);
                        break;
                }
            }
        }

        private bool IsNullable(Type type)
        {
            return Nullable.GetUnderlyingType(type) != null;
        }

        private bool IsSourceTypePropertyNullable(PropertyInfo[] typeProperties, string propertyName)
        {
            return typeProperties.Any(info => info.Name.Equals(propertyName, StringComparison.OrdinalIgnoreCase)
                                            && IsNullable(info.PropertyType));
        }

        private void AddPropertyToRequired(OpenApiSchema schema, string propertyName)
        {
            schema.Required ??= new HashSet<string>();

            schema.Required.Add(propertyName);
        }
    }
}
