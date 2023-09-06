using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Infrastructure.FileSystem.System.Threading;
using SharedKernel.Testing.Infrastructure;

namespace SharedKernel.Integration.Tests.System.Threading.FileSystem;

public class FileSystemApp : InfrastructureTestCase<FakeStartup>
{
    protected override string GetJsonFile()
    {
        return "Data/Redis/appsettings.redis.json";
    }

    protected override IServiceCollection ConfigureServices(IServiceCollection services)
    {
        return services
            .AddFileSystemMutex(new DirectoryInfo($"{AppDomain.CurrentDomain.BaseDirectory}/tmp"));
    }
}
