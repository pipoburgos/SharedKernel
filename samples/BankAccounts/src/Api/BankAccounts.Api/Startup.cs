using BankAccounts.Infrastructure;
using BankAccounts.Infrastructure.Shared;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SharedKernel.Api.Middlewares;
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
        public Startup(
            IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary> Configurar la lista de servicios para poder crear el contenedor de dependencias </summary>
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddInMemoryCommandBus()
                .AddInMemoryQueryBus()
                .AddInMemoryEventBus(_configuration, options => options.RetryCount = 12)
                .AddInMemoryCache()
                .AddFluentValidation(options => options.AutomaticValidationEnabled = false)
                .AddBankAccounts(_configuration, "BankAccountConnection")
                .AddSharedKernelApi<InjectableLibrary>("cors",
                    _configuration.GetSection("Origins").Get<string[]>())
                .AddSharedKernelOpenApi(_configuration)
                .AddControllers()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                });
        }

        /// <summary> Configurar los middlewares </summary>
        public void Configure(IApplicationBuilder app, IOptions<OpenApiOptions> openApiOptions, IOptions<OpenIdOptions> openIdOptions)
        {
            app
                .UseErrors()
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
