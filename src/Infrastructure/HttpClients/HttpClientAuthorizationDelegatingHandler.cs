#if !NET461 && !NETSTANDARD2_1
using Microsoft.AspNetCore.Http;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SharedKernel.Infrastructure.HttpClients
{
    /// <summary>  </summary>
    public class HttpClientAuthorizationDelegatingHandler : DelegatingHandler
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        /// <summary>  </summary>
        public HttpClientAuthorizationDelegatingHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>  </summary>
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            if (_httpContextAccessor.HttpContext == null)
                return base.SendAsync(request, cancellationToken);

            foreach (var header in _httpContextAccessor.HttpContext.Request.Headers)
            {
                foreach (var stringValue in header.Value)
                {
                    request.Headers.Add(header.Key, stringValue);
                }
            }

            return base.SendAsync(request, cancellationToken);
        }
    }
}
#endif