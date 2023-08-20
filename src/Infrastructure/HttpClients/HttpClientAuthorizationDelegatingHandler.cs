using SharedKernel.Application.Security;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SharedKernel.Infrastructure.HttpClients
{
    /// <summary>  </summary>
    public class HttpClientAuthorizationDelegatingHandler : DelegatingHandler
    {
        private readonly IIdentityService _identityService;

        /// <summary>  </summary>
        public HttpClientAuthorizationDelegatingHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        /// <summary>  </summary>
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            var authorization = _identityService.GetKeyValue("Authorization");
            if (!string.IsNullOrWhiteSpace(authorization))
                request.Headers.Add("Authorization", authorization);

            return base.SendAsync(request, cancellationToken);
        }
    }
}
