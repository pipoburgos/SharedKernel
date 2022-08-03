using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SharedKernel.Infrastructure.HealthChecks
{
    /// <summary>  </summary>
    public class HttpClientHealthCheck : IHealthCheck
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IEnumerable<HttpClientHealthCheckConfiguration> _httpClientHealthCheck;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="httpClientFactory"></param>
        /// <param name="httpClientHealthCheck"></param>
        public HttpClientHealthCheck(IHttpClientFactory httpClientFactory, IEnumerable<HttpClientHealthCheckConfiguration> httpClientHealthCheck)
        {
            _httpClientFactory = httpClientFactory;
            _httpClientHealthCheck = httpClientHealthCheck;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns></returns>
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context,
            CancellationToken cancellationToken = default)
        {
            try
            {
                foreach (var httpClientHealthCheck in _httpClientHealthCheck)
                {
                    var response = await _httpClientFactory
                        .CreateClient(httpClientHealthCheck.ClientName)
                        .GetAsync(httpClientHealthCheck.Endpoint, cancellationToken);

                    if (!response.IsSuccessStatusCode)
                        return HealthCheckResult.Unhealthy();
                }
            }
            catch (Exception ex)
            {
                return HealthCheckResult.Unhealthy(ex.Message);
            }

            return HealthCheckResult.Healthy();
        }
    }
}