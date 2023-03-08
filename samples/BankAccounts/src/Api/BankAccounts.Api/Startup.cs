using BankAccounts.Api.Shared;
using BankAccounts.Infrastructure.Shared;
using Microsoft.AspNetCore.Builder;
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
        private readonly IConfiguration _configuration;

        /// <summary> Constructor. </summary>
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
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
                .AddSharedKernelApi("cors", _configuration.GetSection("Origins").Get<string[]>(),
                    o => o.Conventions.Add(new ControllerDocumentationConvention()))
                .AddSharedKernelOpenApi(_configuration);
        }

        /// <summary> Configurar los middlewares </summary>
        public void Configure(IApplicationBuilder app, IOptions<OpenApiOptions> openApiOptions, IOptions<OpenIdOptions> openIdOptions)
        {
            app
                .UseApiErrors()
                .UseCors("cors")
                .UseRouting()
                .UseEndpoints(endpoints =>
                {
                    endpoints.MapControllers();
                })
                .UseSharedKernelOpenApi(openApiOptions, openIdOptions);
        }
    }
}
