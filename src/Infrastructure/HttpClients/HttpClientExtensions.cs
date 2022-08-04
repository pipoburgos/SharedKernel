#if !NET461 && !NETSTANDARD2_1
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Polly;
using SharedKernel.Infrastructure.RetryPolicies;
using System;
using System.Net.Http;

namespace SharedKernel.Infrastructure.HttpClients
{
    /// <summary>
    /// Http client extensions
    /// </summary>
    public static class HttpClientExtensions
    {
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
                .ConfigureHttpClient<HttpClientAuthorizationDelegatingHandler>(services, configuration, name, uri);
        }

        /// <summary> Add http client with bearer token. </summary>
        /// <param name="services"></param>
        /// <param name="name"></param>
        /// <param name="uri"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddHttpClientBearerToken<THandler>(this IServiceCollection services,
            IConfiguration configuration, string name, Uri uri) where THandler : DelegatingHandler
        {
            return services
                .AddHttpClient(name)
                .ConfigureHttpClient<THandler>(services, configuration, name, uri);
        }

        /// <summary> Add http client with bearer token. </summary>
        /// <typeparam name="TClient"></typeparam>
        /// <typeparam name="THandler"></typeparam>
        /// <param name="services"></param>
        /// <param name="name"></param>
        /// <param name="uri"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddHttpClientBearerToken<TClient, THandler>(this IServiceCollection services,
            IConfiguration configuration, string name, Uri uri) where THandler : DelegatingHandler
            where TClient : class
        {
            return services
                .AddHttpClient<TClient>(name)
                .ConfigureHttpClient<THandler>(services, configuration, name, uri);
        }

        /// <summary>  </summary>
        public static IServiceCollection ConfigureHttpClient<THandler>(this IHttpClientBuilder clientBuilder, IServiceCollection services,
            IConfiguration configuration, string name, Uri uri) where THandler : DelegatingHandler
        {
            var retrieverOptions = RetrieverOptions<THandler>(services, configuration, name, uri);

            clientBuilder
                .ConfigureHttpClient(c => c.BaseAddress = uri)
                .AddHttpMessageHandler<THandler>()
                .AddTransientHttpErrorPolicy(policyBuilder =>
                    policyBuilder.WaitAndRetryAsync(retrieverOptions.RetryCount, retrieverOptions.RetryAttempt()));

            return services;
        }

        /// <summary>  </summary>
        public static RetrieverOptions RetrieverOptions<THandler>(IServiceCollection services, IConfiguration configuration, string name,
            Uri uri) where THandler : DelegatingHandler
        {
            var retrieverOptions = new RetrieverOptions();
            configuration.GetSection("RetrieverOptions").Bind(retrieverOptions);

            services.AddHealthChecks()
                .AddUrlGroup(uri, name, HealthStatus.Degraded);

            services.AddTransient<THandler>();

            return retrieverOptions;
        }
    }
}
#endif