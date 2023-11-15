using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using SharedKernel.Api.Gateway.ServiceCollectionExtensions.OpenApi;

namespace SharedKernel.Api.Gateway.ServiceCollectionExtensions;

/// <summary>
/// 
/// </summary>
public static class SharedKernelGatewayApiExtensions
{
    /// <summary>
    /// 
    /// </summary>
    public static readonly string MyAllowSpecificOrigins = "CorsPolicy";

    /// <summary>
    /// 
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static IServiceCollection AddGatewayApi(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddApi(configuration)
            .AddControllers()
            .AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
            });

        return services;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static IServiceCollection AddApi(this IServiceCollection services, IConfiguration configuration)
    {
        var origins = configuration.GetSection("Origins").Get<string[]>();

        services.AddCors(options =>
        {
            options.AddPolicy(MyAllowSpecificOrigins,
                builder => builder
                    .WithOrigins(origins!)
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
        });

        services.AddMvc();

        services.AddSignalR();

        services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();

        services.AddAuth(configuration);

        services.AddSharedKernelOpenApi(configuration);

        // configures IIS out-of-proc settings (see https://github.com/aspnet/AspNetCore/issues/14882)
        services.Configure<IISOptions>(iis =>
        {
            iis.AuthenticationDisplayName = "Windows";
            iis.AutomaticAuthentication = false;
        });

        // configures IIS in-proc settings
        services.Configure<IISServerOptions>(iis =>
        {
            iis.AuthenticationDisplayName = "Windows";
            iis.AutomaticAuthentication = false;
        });

        return services;
    }
}