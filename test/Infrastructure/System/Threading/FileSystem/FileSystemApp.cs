using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Infrastructure.FileSystem.System.Threading;
using SharedKernel.Testing.Infrastructure;

namespace SharedKernel.Integration.Tests.System.Threading.FileSystem;

public class FileSystemApp : InfrastructureTestCase<FakeStartup>
{
    protected override IServiceCollection ConfigureServices(IServiceCollection services)
    {
        return services
            .AddSharedKernelFileSystemMutex(new DirectoryInfo($"{AppDomain.CurrentDomain.BaseDirectory}/tmp"));
    }
}
