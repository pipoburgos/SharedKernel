using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Infrastructure.EntityFrameworkCore;
using SharedKernel.Integration.Tests.Data.CommonRepositoryTesting;
using SharedKernel.Integration.Tests.Data.EntityFrameworkCore.DbContexts;

namespace SharedKernel.Integration.Tests.Data.EntityFrameworkCore.Repositories.InMemory;

public class EntityFrameworkCoreInMemoryRepositoryTests : RepositoryCommonTestTests<UserEfCoreRepository>
{
    protected override IServiceCollection ConfigureServices(IServiceCollection services)
    {
        return services
            .AddEntityFrameworkCoreInMemoryUnitOfWorkAsync<ISharedKernelUnitOfWork, SharedKernelDbContext>(Guid.NewGuid().ToString())
            .AddTransient<UserEfCoreRepository>();
    }
}

