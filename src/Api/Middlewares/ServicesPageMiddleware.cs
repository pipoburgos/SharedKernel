using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace SharedKernel.Api.Middlewares
{
    public static class ServicesPageMiddleware
    {
        public static IApplicationBuilder UseServicesPage(this IApplicationBuilder app, IServiceCollection services)
        {
            app.Map("/services", builder => builder.Run(async context =>
            {
                var sb = new StringBuilder();
                sb.Append($"<h1>Registered {services.Count} Services</h1>");
                sb.Append("<table><thead>");
                sb.Append("<tr><th>Lifetime</th><th>Class</th><th>Interface</th></tr>");

                sb.Append("</thead><tbody>");
                var total = services
                    .OrderBy(s => s.Lifetime)
                    .ThenBy(s => s.ImplementationType?.FullName)
                    .ThenBy(s => s.ServiceType?.FullName);

                foreach (var svc in total)
                {
                    sb.Append("<tr>");
                    sb.Append($"<td>{svc.Lifetime}</td>");
                    sb.Append($"<td>{svc.ImplementationType?.FullName}</td>");
                    sb.Append($"<td>{svc.ServiceType.FullName}</td>");
                    sb.Append("</tr>");
                }
                sb.Append("</tbody></table>");
                await context.Response.WriteAsync(sb.ToString());
            }));

            return app;
        }
    }
}
