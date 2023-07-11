using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Linq;

namespace SharedKernel.Api.ServiceCollectionExtensions.OpenApi.DocumentFilters
{
    /// <summary>
    /// 
    /// </summary>
    public class TagReOrderDocumentFilter : IDocumentFilter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="swaggerDoc"></param>
        /// <param name="context"></param>
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            swaggerDoc.Tags = swaggerDoc.Tags
                .OrderBy(tag => tag.Name)
                .ToList();
        }
    }
}
