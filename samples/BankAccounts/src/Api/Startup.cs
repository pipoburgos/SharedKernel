using BankAccounts.Infrastructure.Shared;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SharedKernel.Api.Middlewares;
using SharedKernel.Api.ServiceCollectionExtensions;
using SharedKernel.Api.ServiceCollectionExtensions.OpenApi;
using SharedKernel.Application.Security;
using SharedKernel.Infrastructure.Cqrs.Commands;
using SharedKernel.Infrastructure.Cqrs.Queries;
using SharedKernel.Infrastructure.NetJson;
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
    private IServiceCollection _services;

    /// <summary> Constructor. </summary>
    public Startup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    /// <summary> Configurar la lista de servicios para poder crear el contenedor de dependencias </summary>
    public void ConfigureServices(IServiceCollection services)
    {
        _services = services
            .AddInMemoryCommandBus()
            .AddRedisCommandBusAsync(_configuration)
            .AddNetJsonSerializer()
            .AddInMemoryQueryBus()
            .AddRedisEventBus(_configuration)
            .AddRedisDistributedCache(_configuration)
            .AddRedisMutex(_configuration)
            .AddBankAccounts(_configuration, "BankAccountConnection")
            .AddSharedKernelOpenApi(_configuration)
            .AddSharedKernelApi(CorsPolicy, _configuration.GetSection("Origins").Get<string[]>(), o =>
            {
                //var policyBuilder = new AuthorizationPolicyBuilder().RequireAuthenticatedUser();

                //policyBuilder.AddAuthenticationSchemes(_webHostEnvironment.EnvironmentName.Contains("Testing")
                //    ? "FakeBearer"
                //    : JwtBearerDefaults.AuthenticationScheme);

                //var policy = policyBuilder.Build();

                //o.Filters.Add(new AuthorizeFilter(policy));
                o.Conventions.Add(new ControllerDocumentationConvention());
            });
    }

    /// <summary> Configurar los middlewares </summary>
    public void Configure(IApplicationBuilder app, IOptions<OpenApiOptions> openApiOptions, IOptions<OpenIdOptions> openIdOptions)
    {
        app
            .UseSharedKernelServicesPage(_services)
            .UseSharedKernelExceptionHandler("BankAccounts",
                exceptionHandler => $"An error has occurred, check with the administrator ({exceptionHandler.Error.Message})")
            .UseCors(CorsPolicy)
            .UseRouting()
            .UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/health", new HealthCheckOptions
                {
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });
            })
            .UseSharedKernelOpenApi(openApiOptions, openIdOptions);
    }
}
