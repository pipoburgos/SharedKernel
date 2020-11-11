using FluentValidation.AspNetCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using SharedKernel.Api.Gateway.ServiceCollectionExtensions;
using SharedKernel.Infrastructure;
using SharedKernel.Infrastructure.Validators;

namespace SharedKernel.Api.ServiceCollectionExtensions
{
    public static class SharedKernelApiExtensions
    {
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
    }
}
