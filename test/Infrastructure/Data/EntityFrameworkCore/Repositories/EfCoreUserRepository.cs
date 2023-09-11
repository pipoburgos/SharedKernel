using SharedKernel.Domain.Tests.Users;
using SharedKernel.Infrastructure.EntityFrameworkCore.Data.Repositories;
using SharedKernel.Integration.Tests.Data.EntityFrameworkCore.DbContexts;

namespace SharedKernel.Integration.Tests.Data.EntityFrameworkCore.Repositories;

public class EfCoreUserRepository : EntityFrameworkCoreRepositoryAsync<User, Guid>
{
    public EfCoreUserRepository(SharedKernelEntityFrameworkDbContext sharedKernelDbContext) : base(sharedKernelDbContext)
    {
    }
}
