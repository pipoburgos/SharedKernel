using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Application.RetryPolicies;
using SharedKernel.Application.Settings;
using SharedKernel.Infrastructure.Settings;
using System;

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
        /// <param name="optionsConfiguration"></param>
        /// <returns></returns>
        public static IServiceCollection AddPollyRetry(this IServiceCollection services, IConfiguration configuration, Action<RetrieverOptions> optionsConfiguration = null)
        {
            var retrieverOptions = new RetrieverOptions();
            configuration.GetSection(nameof(RetrieverOptions)).Bind(retrieverOptions);
            services.Configure<RetrieverOptions>(configuration.GetSection(nameof(RetrieverOptions)));

            if(optionsConfiguration != default)
                optionsConfiguration(retrieverOptions);

            return services
                .AddTransient<IRetriever, PollyRetriever>()
                .AddTransient(typeof(IOptionsService<>), typeof(OptionsService<>));
        }
    }
}
