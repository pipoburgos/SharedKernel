using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Prometheus;
using SharedKernel.Infrastructure.Validators;

namespace SharedKernel.Api.ServiceCollectionExtensions
{
    /// <summary>
    /// Shared kernel api extensions
    /// </summary>
    public static class SharedKernelApiExtensions
    {
        /// <summary>
        /// Adds Options, Metrics, Cors, Api versioning, Api controllers, Fluent api validators and Newtonsoft to service collection
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <param name="policyName">The policy name of a configured policy.</param>
        /// <param name="origins">All domains who calls the api</param>
        /// <returns></returns>
        public static IServiceCollection AddSharedKernelApi<TValidator>(this IServiceCollection services, string policyName, string[] origins)
        {
            services
                .AddOptions()
                .AddMetrics()
                .AddCors(options =>
                {
                    options.AddPolicy(policyName,
                        builder => builder
                            .WithOrigins(origins)
                            .AllowAnyMethod()
                            .AllowAnyHeader()
                            .AllowCredentials());
                })
                .AddApiVersioning(config =>
                {
                    // Specify the default API Version as 1.0
                    config.DefaultApiVersion = new ApiVersion(1, 0);
                    // If the client hasn't specified the API version in the request, use the default API version number
                    config.AssumeDefaultVersionWhenUnspecified = true;
                    // Advertise the API versions supported for the particular endpoint
                    config.ReportApiVersions = true;
                })
                .AddControllers(o =>
                {
                    o.Conventions.Add(new ControllerDocumentationConvention());
                })
                .AddFluentValidation(fv =>
                {
                    fv.RegisterValidatorsFromAssemblyContaining<TValidator>();
                    fv.RegisterValidatorsFromAssemblyContaining<PageOptionsValidator>();
                })
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                });

            return services;
        }

        /// <summary>
        /// Adds metrics with prometheus
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseSharedKernelMetrics(this IApplicationBuilder app)
        {
            return app
                .UseMetricServer()
                .UseHttpMetrics();
        }
    }
}
