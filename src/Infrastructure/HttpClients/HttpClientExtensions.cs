#if !NET461 && !NETSTANDARD2_1
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using SharedKernel.Application.Settings;
using SharedKernel.Infrastructure.RetryPolicies;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

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
        /// <param name="options"></param>
        /// <returns></returns>
        public static IServiceCollection AddHttpClientBearerToken<TClient>(this IServiceCollection services,
            string name, string uri, IOptionsService<RetrieverOptions> options) where TClient : class

        {
            services.AddHttpClient<TClient>(name)
                .ConfigureHttpClient(c => c.BaseAddress = new Uri(uri))
                .AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>()
                .AddTransientHttpErrorPolicy(policyBuilder =>
                    policyBuilder.WaitAndRetryAsync(options.Value.RetryCount, options.Value.RetryAttempt()));

            return services;
        }

        /// <summary> Add http client with bearer token. </summary>
        /// <param name="services"></param>
        /// <param name="name"></param>
        /// <param name="uri"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static IServiceCollection AddHttpClientBearerToken(this IServiceCollection services,
            string name, string uri, IOptionsService<RetrieverOptions> options)

        {
            services.AddHttpClient(name)
                .ConfigureHttpClient(c => c.BaseAddress = new Uri(uri))
                .AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>()
                .AddTransientHttpErrorPolicy(policyBuilder =>
                    policyBuilder.WaitAndRetryAsync(options.Value.RetryCount, options.Value.RetryAttempt()));

            return services;
        }
    }

    internal class HttpClientAuthorizationDelegatingHandler : DelegatingHandler
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HttpClientAuthorizationDelegatingHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (_httpContextAccessor.HttpContext == null)
                return await base.SendAsync(request, cancellationToken);

            var authorizationHeader = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];

            if (!string.IsNullOrEmpty(authorizationHeader))
                request.Headers.Add("Authorization", new List<string> { authorizationHeader });

            const string accessToken = "access_token";

            var token = await _httpContextAccessor.HttpContext.GetTokenAsync(accessToken);

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
#endif