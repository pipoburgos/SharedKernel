#if !NET461 && !NETSTANDARD2_1
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using SharedKernel.Infrastructure.HealthChecks;
using SharedKernel.Infrastructure.RetryPolicies;
using System;

namespace SharedKernel.Infrastructure.HttpClients
{
    /// <summary>
    /// Http client extensions
    /// </summary>
    public static class HttpClientExtensions
    {
        /// <summary> Add http client with bearer token. </summary>
        /// <typeparam name="TClient"></typeparam>
        /// <param name="services"></param>
        /// <param name="name"></param>
        /// <param name="uri"></param>
        /// <param name="configuration"></param>
        /// <param name="healthCheckEndpoint"></param>
        /// <returns></returns>
        public static IServiceCollection AddHttpClientBearerToken<TClient>(this IServiceCollection services,
            IConfiguration configuration, string name, string uri, string healthCheckEndpoint = "index.html")
            where TClient : class
        {
            return services
                .AddHttpClient<TClient>(name)
                .ConfigureHttpClient(services, configuration, name, uri, healthCheckEndpoint);
        }

        /// <summary> Add http client with bearer token. </summary>
        /// <param name="services"></param>
        /// <param name="name"></param>
        /// <param name="uri"></param>
        /// <param name="configuration"></param>
        /// <param name="healthCheckEndpoint"></param>
        /// <returns></returns>
        public static IServiceCollection AddHttpClientBearerToken(this IServiceCollection services,
            IConfiguration configuration, string name, string uri, string healthCheckEndpoint = "index.html")
        {
            return services
                .AddHttpClient(name)
                .ConfigureHttpClient(services, configuration, name, uri, healthCheckEndpoint);
        }

        /// <summary>  </summary>
        public static IServiceCollection ConfigureHttpClient(this IHttpClientBuilder clientBuilder, IServiceCollection services,
            IConfiguration configuration, string name, string uri, string healthCheckEndpoint = "index.html")
        {
            var retrieverOptions = RetrieverOptions(services, configuration, name, healthCheckEndpoint);

            clientBuilder
                .ConfigureHttpClient(c => c.BaseAddress = new Uri(uri))
                .AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>()
                .AddTransientHttpErrorPolicy(policyBuilder =>
                    policyBuilder.WaitAndRetryAsync(retrieverOptions.RetryCount, retrieverOptions.RetryAttempt()));

            return services;
        }

        /// <summary>  </summary>
        public static RetrieverOptions RetrieverOptions(IServiceCollection services, IConfiguration configuration, string name,
            string healthCheckEndpoint)
        {
            var retrieverOptions = new RetrieverOptions();
            configuration.GetSection("RetrieverOptions").Bind(retrieverOptions);

            services.AddHealthChecks().AddCheck<HttpClientHealthCheck>(name);

            services
                .AddTransient<HttpClientAuthorizationDelegatingHandler>()
                .AddTransient(_ => new HttpClientHealthCheckConfiguration(name, healthCheckEndpoint));
            return retrieverOptions;
        }
    }
}
#endif