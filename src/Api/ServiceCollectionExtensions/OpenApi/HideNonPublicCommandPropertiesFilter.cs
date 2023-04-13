using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Linq;
using SharedKernel.Application.Cqrs.Commands;

namespace SharedKernel.Api.ServiceCollectionExtensions.OpenApi
{
    /// <summary>  </summary>
    public class HideNonPublicCommandPropertiesFilter : ISchemaFilter
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
