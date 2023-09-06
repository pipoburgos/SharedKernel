using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Infrastructure.AsyncKeyedLock.System.Threading;
using SharedKernel.Testing.Infrastructure;

namespace SharedKernel.Integration.Tests.System.Threading.AsyncKeyedLock;

public class AsyncKeyedLockApp : InfrastructureTestCase<FakeStartup>
{
    protected override IServiceCollection ConfigureServices(IServiceCollection services)
    {
        return services.AddAsyncKeyedLockMutex();
    }
}
