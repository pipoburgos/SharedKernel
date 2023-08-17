using BankAccounts.Api.Shared;
using BankAccounts.Infrastructure.Shared;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SharedKernel.Api.Middlewares;
using SharedKernel.Api.ServiceCollectionExtensions;
using SharedKernel.Api.ServiceCollectionExtensions.OpenApi;
using SharedKernel.Application.Security;
using SharedKernel.Infrastructure.Caching;
using SharedKernel.Infrastructure.Cqrs.Commands;
using SharedKernel.Infrastructure.Cqrs.Queries;
using SharedKernel.Infrastructure.Events;
using SharedKernel.Infrastructure.Serializers;

namespace BankAccounts.Api
{
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
                .AddRedisCommandBusAsync(_configuration)
                .AddNetJsonSerializer()
                .AddInMemoryQueryBus()
                .AddRedisEventBus(_configuration)
                .AddRedisDistributedCache(_configuration)
                .AddBankAccounts(_configuration, "BankAccountConnection")
                .AddValidatorsFromAssemblyContaining<Startup>(lifetime: ServiceLifetime.Scoped)
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
                .UseApiErrors()
                .UseCors(CorsPolicy)
                .UseRouting()
                .UseEndpoints(endpoints =>
                {
                    endpoints.MapControllers();
                })
                .UseSharedKernelOpenApi(openApiOptions, openIdOptions);
        }
    }
}
