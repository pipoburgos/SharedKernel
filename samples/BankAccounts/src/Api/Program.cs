using BankAccounts.Api;
using BankAccounts.Infrastructure.Shared;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Serilog;
using SharedKernel.Api.Endpoints;
using SharedKernel.Api.Middlewares;
using SharedKernel.Api.Newtonsoft;
using SharedKernel.Api.ServiceCollectionExtensions;
using SharedKernel.Api.ServiceCollectionExtensions.OpenApi;
using SharedKernel.Infrastructure.Cqrs.Commands;
using SharedKernel.Infrastructure.Cqrs.Queries;
using SharedKernel.Infrastructure.NetJson;
using SharedKernel.Infrastructure.Newtonsoft;
using SharedKernel.Infrastructure.Redis.Caching;
using SharedKernel.Infrastructure.Redis.Cqrs.Commands;
using SharedKernel.Infrastructure.Redis.Events;
using SharedKernel.Infrastructure.Redis.System.Threading;

const string corsPolicy = "CorsPolicy";

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, configuration) =>
{
    configuration.ReadFrom.Configuration(context.Configuration);
});

builder.Services
    .AddAuthorization(options =>
    {
        options.FallbackPolicy = new AuthorizationPolicyBuilder()
            .RequireAuthenticatedUser()
            .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
            .Build();
    })
    .AddSharedKernelMicrosoftOpenApi(2, ["v1", "v2"])
    .AddSharedKernelInMemoryCommandBus()
    .AddSharedKernelRedisCommandBusAsync(builder.Configuration)
    .AddSharedKernelNewtonsoftSerializer()
    .AddSharedKernelNetJsonSerializer()
    .AddSharedKernelInMemoryQueryBus()
    .AddSharedKernelRedisEventBus(builder.Configuration)
    .AddSharedKernelRedisDistributedCache(builder.Configuration)
    .AddSharedKernelRedisMutex(builder.Configuration)
    .AddBankAccounts(builder.Configuration, "BankAccountConnection")
    .AddSharedKernelSwashbuckle(builder.Configuration)
    .AddSharedKernelEndpoints(typeof(BankAccountsApiAssembly).Assembly)
    .AddSharedKernelSwaggerGenNewtonsoftSupport()
    .AddSharedKernelAuth(builder.Configuration)
    .AddSharedKernelApi(corsPolicy, builder.Configuration.GetSection("Origins").Get<string[]>());

var app = builder.Build();

app
    .UseSharedKernelCurrentCulture("en-US", "es-ES", "en", "es")
    .UseSharedKernelServicesPage(builder.Services)
    .UseSharedKernelExceptionHandler("BankAccounts",
        exceptionHandler =>
            $"An error has occurred, check with the administrator ({exceptionHandler.Error.Message})",
        debug => Console.WriteLine(debug.Error))
    .UseCors(corsPolicy)
    .UseRouting()
    .UseResponseCaching()
    .UseSharedKernelSwashbuckle(c =>
    {
        c.SwaggerEndpoint("swagger/v1.json", "Swashbuckle v1");
        c.SwaggerEndpoint("swagger/v2.json", "Swashbuckle v2");
        c.SwaggerEndpoint("openapi/v1.json", "Openapi v1");
        c.SwaggerEndpoint("openapi/v2.json", "Openapi v2");
    })
    .UseAuthentication()
    .UseAuthorization()
    .UseEndpoints(endpoints =>
    {
        endpoints.MapEndpoints();
        endpoints.MapHealthChecks("/health", new HealthCheckOptions
        {
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
        });
    });

await app.RunAsync();

public partial class Program;