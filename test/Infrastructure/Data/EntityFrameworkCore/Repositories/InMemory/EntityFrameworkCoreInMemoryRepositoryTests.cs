using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Integration.Tests.Data.CommonRepositoryTesting;
using SharedKernel.Integration.Tests.Data.EntityFrameworkCore.DbContexts;

namespace SharedKernel.Integration.Tests.Data.EntityFrameworkCore.Repositories.InMemory;

public class EntityFrameworkCoreInMemoryRepositoryTests : RepositoryCommonTestTests<UserEfCoreRepository>
{
    protected override IServiceCollection ConfigureServices(IServiceCollection services)
    {
        services.AddTransient<UserEfCoreRepository>();
        services.AddDbContext<SharedKernelDbContext>(options =>
                options.UseInMemoryDatabase(Guid.NewGuid().ToString()), ServiceLifetime.Singleton);

        return services;
    }
}

