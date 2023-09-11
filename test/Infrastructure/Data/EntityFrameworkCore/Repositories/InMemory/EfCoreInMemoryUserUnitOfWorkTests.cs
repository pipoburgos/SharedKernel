using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Infrastructure.EntityFrameworkCore.Data;
using SharedKernel.Integration.Tests.Data.CommonRepositoryTesting;
using SharedKernel.Integration.Tests.Data.EntityFrameworkCore.DbContexts;

namespace SharedKernel.Integration.Tests.Data.EntityFrameworkCore.Repositories.InMemory;

public class EfCoreInMemoryUserUnitOfWorkTests : UserUnitOfWorkTests<EfCoreUserRepository, ISharedKernelEntityFrameworkUnitOfWork>
{
    protected override IServiceCollection ConfigureServices(IServiceCollection services)
    {
        return services
            .AddEntityFrameworkCoreInMemoryUnitOfWork<ISharedKernelEntityFrameworkUnitOfWork,
                SharedKernelEntityFrameworkDbContext>(Guid.NewGuid().ToString())
            .AddTransient<EfCoreUserRepository>();
    }
}

