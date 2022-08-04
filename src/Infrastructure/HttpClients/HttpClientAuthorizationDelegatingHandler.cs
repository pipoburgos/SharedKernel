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

            var header = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            if (!string.IsNullOrWhiteSpace(header))
                request.Headers.Add("Authorization", header.ToString());

            return base.SendAsync(request, cancellationToken);
        }
    }
}
#endif