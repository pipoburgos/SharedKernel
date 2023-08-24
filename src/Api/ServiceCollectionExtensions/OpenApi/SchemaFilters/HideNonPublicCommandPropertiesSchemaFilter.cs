using Microsoft.OpenApi.Models;
using SharedKernel.Application.Cqrs.Commands;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace SharedKernel.Api.ServiceCollectionExtensions.OpenApi.SchemaFilters
{
    /// <summary>  </summary>
    public class HideNonPublicCommandPropertiesSchemaFilter : ISchemaFilter
    {
        /// <summary>  </summary>
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (schema.Properties == null || schema.Properties.Count == 0)
            {
                return;
            }

            if (!typeof(ICommandRequest).IsAssignableFrom(context.Type))
                return;

            foreach (var property in schema.Properties.Where(p => p.Value.ReadOnly))
            {
                schema.Properties.Remove(property);
            }
        }
    }
}
