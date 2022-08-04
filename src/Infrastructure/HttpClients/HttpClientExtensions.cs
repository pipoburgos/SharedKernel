#if !NET461 && !NETSTANDARD2_1
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Polly;
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
        /// <returns></returns>
        public static IServiceCollection AddHttpClientBearerToken<TClient>(this IServiceCollection services,
            IConfiguration configuration, string name, Uri uri)
            where TClient : class
        {
            return services
                .AddHttpClient<TClient>(name)
                .ConfigureHttpClient(services, configuration, name, uri);
        }

        /// <summary> Add http client with bearer token. </summary>
        /// <param name="services"></param>
        /// <param name="name"></param>
        /// <param name="uri"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddHttpClientBearerToken(this IServiceCollection services,
            IConfiguration configuration, string name, Uri uri)
        {
            return services
                .AddHttpClient(name)
                .ConfigureHttpClient(services, configuration, name, uri);
        }

        /// <summary>  </summary>
        public static IServiceCollection ConfigureHttpClient(this IHttpClientBuilder clientBuilder, IServiceCollection services,
            IConfiguration configuration, string name, Uri uri)
        {
            var retrieverOptions = RetrieverOptions(services, configuration, name, uri);

            clientBuilder
                .ConfigureHttpClient(c => c.BaseAddress = uri)
                .AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>()
                .AddTransientHttpErrorPolicy(policyBuilder =>
                    policyBuilder.WaitAndRetryAsync(retrieverOptions.RetryCount, retrieverOptions.RetryAttempt()));

            return services;
        }

        /// <summary>  </summary>
        public static RetrieverOptions RetrieverOptions(IServiceCollection services, IConfiguration configuration, string name,
            Uri uri)
        {
            var retrieverOptions = new RetrieverOptions();
            configuration.GetSection("RetrieverOptions").Bind(retrieverOptions);

            services.AddHealthChecks()
                .AddUrlGroup(uri, name, HealthStatus.Degraded);

            services.AddTransient<HttpClientAuthorizationDelegatingHandler>();

            return retrieverOptions;
        }
    }
}
#endif