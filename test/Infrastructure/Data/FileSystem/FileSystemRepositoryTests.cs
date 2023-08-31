using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Infrastructure.Data.FileSystem;
using SharedKernel.Infrastructure.Newtonsoft;
using SharedKernel.Integration.Tests.Data.CommonRepositoryTesting;

namespace SharedKernel.Integration.Tests.Data.FileSystem;

public class FileSystemRepositoryTests : RepositoryCommonTestTests<FileSystemUserRepository>
{
    protected override string GetJsonFile()
    {
        return "Data/FileSystem/appsettings.fileSystem.json";
    }

    protected override IServiceCollection ConfigureServices(IServiceCollection services)
    {
        return services
            .AddFileSystemUnitOfWork<ISharedKernelUnitOfWork, SharedKernelUnitOfWork>()
            .AddNewtonsoftSerializer()
            .AddTransient<FileSystemUserRepository>();
    }
}
