using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Infrastructure.FileSystem.Data;
using SharedKernel.Infrastructure.Newtonsoft;
using SharedKernel.Integration.Tests.Data.CommonRepositoryTesting;
using SharedKernel.Integration.Tests.Data.FileSystem.DbContexts;
using SharedKernel.Integration.Tests.Data.FileSystem.Repositories;

namespace SharedKernel.Integration.Tests.Data.FileSystem;

public class FileSystemBankAccountRepositoryTests : BankAccountRepositoryCommonTestTests<FileSystemBankAccountRepository>
{
    protected override string GetJsonFile()
    {
        return "Data/FileSystem/appsettings.fileSystem.json";
    }

    protected override IServiceCollection ConfigureServices(IServiceCollection services)
    {
        return services
            .AddFileSystemDbContext<SharedKernelFileSystemDbContext>()
            .AddNewtonsoftSerializer()
            .AddTransient<FileSystemBankAccountRepository>();
    }
}
