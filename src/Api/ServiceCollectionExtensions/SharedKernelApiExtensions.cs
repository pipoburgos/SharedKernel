using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using SharedKernel.Infrastructure;
using SharedKernel.Infrastructure.Validators;

namespace SharedKernel.Api.ServiceCollectionExtensions
{
    public static class SharedKernelApiExtensions
    {
        public static string MyAllowSpecificOrigins = "CorsPolicy";

        public static IServiceCollection AddSharedKernelApi<TValidatorsAssembly>(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddOptions()
                .AddApi(configuration)
                .AddSharedKernelComponent(configuration)
                .AddInMemmoryCommandBus()
                .AddInMemmoryQueryBus()
                .AddRabbitMqEventBus(configuration)
                .AddControllers()
                .AddFluentValidation(fv =>
                {
                    fv.RegisterValidatorsFromAssemblyContaining<TValidatorsAssembly>();
                    fv.RegisterValidatorsFromAssemblyContaining<PageOptionsValidator>();
                })
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                });


            return services;
        }

        public static IServiceCollection AddGatewayApi<TAssembly>(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddApi(configuration)
                .AddControllers()
                .AddFluentValidation(fv =>
                {
                    fv.RegisterValidatorsFromAssemblyContaining<TAssembly>();
                })
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                });

            return services;
        }

        public static IServiceCollection AddApi(this IServiceCollection services, IConfiguration configuration)
        {
            var origins = configuration.GetSection("Origins").Get<string[]>();

            services.AddCors(options =>
            {
                options.AddPolicy(MyAllowSpecificOrigins,
                    builder => builder
                        .WithOrigins(origins)
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials());
            });

            services.AddMvc();

            services.AddSignalR();

            services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();

            services.AddAuth(configuration);

            services.AddOpenApi(configuration);

            services.AddApiVersioning(config =>
            {
                // Specify the default API Version as 1.0
                config.DefaultApiVersion = new ApiVersion(1, 0);
                // If the client hasn't specified the API version in the request, use the default API version number
                config.AssumeDefaultVersionWhenUnspecified = true;
                // Advertise the API versions supported for the particular endpoint
                config.ReportApiVersions = true;
            });

            return services;
        }
    }
}
