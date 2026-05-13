using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

namespace SharedKernel.Api.ServiceCollectionExtensions.OpenApi.SchemaFilters;

public class RequiredFromConstructorSchemaFilter : ISchemaFilter
{
    public void Apply(IOpenApiSchema schema, SchemaFilterContext context)
    {
        if (schema.Properties == null) return;

        var constructors = context.Type.GetConstructors();

        var requiredFromCtor = constructors
            .SelectMany(c => c.GetParameters())
            .Where(p => p.ParameterType == typeof(string) && !IsNullable(p))
            .Select(p => p.Name)
            .ToList();

        var props = schema.Properties
            .Where(prop => requiredFromCtor.Any(x => string.Equals(x, prop.Key, StringComparison.OrdinalIgnoreCase)))
            .ToList();

        foreach (var prop in props)
        {
            schema.Required?.Add(prop.Key);
        }
    }

    private static bool IsNullable(ParameterInfo p)
    {
        var ctx = new NullabilityInfoContext();
        return ctx.Create(p).WriteState == NullabilityState.Nullable;
    }
}
