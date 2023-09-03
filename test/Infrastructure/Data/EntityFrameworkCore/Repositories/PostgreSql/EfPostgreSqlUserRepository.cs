using SharedKernel.Domain.Tests.Users;
using SharedKernel.Infrastructure.EntityFrameworkCore.Data.Repositories;

namespace SharedKernel.Integration.Tests.Data.EntityFrameworkCore.Repositories.PostgreSql;

public class EfPostgreSqlUserRepository : EntityFrameworkCoreRepositoryAsync<User, Guid>
{
    public EfPostgreSqlUserRepository(PostgreSqlSharedKernelDbContext sharedKernelDbContext) : base(sharedKernelDbContext)
    {
    }
}
