using BankAccounts.Api.Shared;
using BankAccounts.Infrastructure.Shared;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SharedKernel.Api.ServiceCollectionExtensions;
using SharedKernel.Api.ServiceCollectionExtensions.OpenApi;
using SharedKernel.Application.Security;
using SharedKernel.Infrastructure.Caching;
using SharedKernel.Infrastructure.Cqrs.Commands;
using SharedKernel.Infrastructure.Cqrs.Queries;
using SharedKernel.Infrastructure.Events;

namespace BankAccounts.Api
{
    /// <summary> Arraque api </summary>
    public class Startup
    {
        private const string CorsPolicy = "CorsPolicy";

        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _webHostEnvironment;

        /// <summary> Constructor. </summary>
        public Startup(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            _configuration = configuration;
            _webHostEnvironment = webHostEnvironment;
        }

        /// <summary> Configurar la lista de servicios para poder crear el contenedor de dependencias </summary>
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddInMemoryCommandBus()
                .AddInMemoryQueryBus()
                .AddInMemoryEventBus(_configuration)
                .AddInMemoryCache()
                .AddBankAccounts(_configuration, "BankAccountConnection")
                .AddSharedKernelOpenApi(_configuration)
                .AddSharedKernelApi(CorsPolicy, _configuration.GetSection("Origins").Get<string[]>(), o =>
                {
                    var policyBuilder = new AuthorizationPolicyBuilder().RequireAuthenticatedUser();

                    policyBuilder.AddAuthenticationSchemes(_webHostEnvironment.EnvironmentName.Contains("Testing")
                        ? "FakeBearer"
                        : JwtBearerDefaults.AuthenticationScheme);

                    var policy = policyBuilder.Build();

                    o.Filters.Add(new AuthorizeFilter(policy));
                    o.Conventions.Add(new ControllerDocumentationConvention());
                });
        }

        /// <summary> Configurar los middlewares </summary>
        public void Configure(IApplicationBuilder app, IOptions<OpenApiOptions> openApiOptions, IOptions<OpenIdOptions> openIdOptions)
        {
            app
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
