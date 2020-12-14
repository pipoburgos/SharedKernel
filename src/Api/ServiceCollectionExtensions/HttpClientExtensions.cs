using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace SharedKernel.Api.ServiceCollectionExtensions
{
    public static class HttpClientExtensions
    {
        public static IServiceCollection AddClient<TClient>(this IServiceCollection services, IConfiguration configuration, string section) where TClient : class
        {
            services.AddHttpClient<TClient>(section)
                .ConfigureHttpClient(c => c.BaseAddress = new Uri(configuration.GetSection(section).Value))
                .AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>();

            return services;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class HttpClientAuthorizationDelegatingHandler : DelegatingHandler
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="httpContextAccessor"></param>
        public HttpClientAuthorizationDelegatingHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
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
