using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Linq;

namespace SharedKernel.Api.ServiceCollectionExtensions.OpenApi
{
    /// <summary>  </summary>
    public class HideNonPublicPropertiesFilter : ISchemaFilter
    {
        /// <summary>  </summary>
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (schema.Properties == null || schema.Properties.Count == 0)
            {
                return;
            }

            foreach (var property in schema.Properties.Where(p => p.Value.ReadOnly))
            {
                schema.Properties.Remove(property);
            }
        }
    }
}
