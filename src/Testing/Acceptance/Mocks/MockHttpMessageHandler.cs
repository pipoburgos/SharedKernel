using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SharedKernel.Testing.Acceptance.Mocks;

public class MockHttpMessageHandler : HttpMessageHandler
{
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        return Send(request, cancellationToken);
    }

    public new virtual Task<HttpResponseMessage> Send(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}