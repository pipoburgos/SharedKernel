using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Application.RetryPolicies;
using SharedKernel.Application.Settings;
using SharedKernel.Infrastructure.Settings;

namespace SharedKernel.Infrastructure.RetryPolicies
{
    /// <summary>
    /// Retriever extensions
    /// </summary>
    public static class RetryPoliciesServiceExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddPollyRetry(this IServiceCollection services, IConfiguration configuration)
        {
            var retrieverOptions = new RetrieverOptions();
            configuration.GetSection(nameof(RetrieverOptions)).Bind(retrieverOptions);
            services.Configure<RetrieverOptions>(configuration.GetSection(nameof(RetrieverOptions)));

            return services
                .AddTransient<IRetriever, PollyRetrieverWrap>()
                .AddTransient(typeof(IOptionsService<>), typeof(OptionsService<>));
        }
    }
}
