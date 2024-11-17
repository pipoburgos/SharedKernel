using BankAccounts.Infrastructure.Shared;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Mvc.Authorization;
using SharedKernel.Api.Middlewares;
using SharedKernel.Api.Newtonsoft;
using SharedKernel.Api.ServiceCollectionExtensions;
using SharedKernel.Api.ServiceCollectionExtensions.OpenApi;
using SharedKernel.Infrastructure.Cqrs.Commands;
using SharedKernel.Infrastructure.Cqrs.Queries;
using SharedKernel.Infrastructure.Newtonsoft;
using SharedKernel.Infrastructure.Redis.Caching;
using SharedKernel.Infrastructure.Redis.Cqrs.Commands;
using SharedKernel.Infrastructure.Redis.Events;
using SharedKernel.Infrastructure.Redis.System.Threading;

namespace BankAccounts.Api;

/// <summary> Arraque api </summary>
public class Startup
{
    private const string CorsPolicy = "CorsPolicy";

    private readonly IConfiguration _configuration;
    private IServiceCollection? _services;

    /// <summary> Constructor. </summary>
    public Startup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    /// <summary> Configurar la lista de servicios para poder crear el contenedor de dependencias </summary>
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers(o =>
            {
                var authorizationPolicy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                    .Build();

                o.Filters.Add(new AuthorizeFilter(authorizationPolicy));
                o.Conventions.Add(new ControllerDocumentationConvention());
            })
            .AddSharedKernelNewtonsoftJson();

        _services = services
            .AddSharedKernelInMemoryCommandBus()
            .AddSharedKernelRedisCommandBusAsync(_configuration)
            .AddSharedKernelNewtonsoftSerializer()
            .AddSharedKernelInMemoryQueryBus()
            .AddSharedKernelRedisEventBus(_configuration)
            .AddSharedKernelRedisDistributedCache(_configuration)
            .AddSharedKernelRedisMutex(_configuration)
            .AddBankAccounts(_configuration, "BankAccountConnection")
            .AddSharedKernelOpenApi(_configuration)
            //.AddOpenApi(o =>
            //{
            //    o.OpenApiVersion = OpenApiSpecVersion.OpenApi3_0;

            //})
            .AddSharedKernelSwaggerGenNewtonsoftSupport()
            .AddSharedKernelAuth(_configuration)
            .AddSharedKernelApi(CorsPolicy, _configuration.GetSection("Origins").Get<string[]>());
    }

    /// <summary> Configurar los middlewares </summary>
    public void Configure(IApplicationBuilder app)
    {
        app
            .UseSharedKernelCurrentCulture("en-US", "es-ES")
            .UseSharedKernelServicesPage(_services ?? throw new ArgumentNullException(nameof(_services)))
            .UseSharedKernelExceptionHandler("BankAccounts",
                exceptionHandler =>
                    $"An error has occurred, check with the administrator ({exceptionHandler.Error.Message})",
                debug => Console.WriteLine(debug.Error))
            .UseCors(CorsPolicy)
            .UseRouting()
            .UseResponseCaching()
            .UseAuthentication()
            .UseAuthorization()
            .UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                //endpoints.MapOpenApi();
                endpoints.MapHealthChecks("/health", new HealthCheckOptions
                {
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
                });
            })
            .UseSharedKernelOpenApi();
    }
}
