using SharedKernel.Domain.Tests.Users;
using SharedKernel.Infrastructure.EntityFrameworkCore.Data.Repositories;
using SharedKernel.Integration.Tests.Data.EntityFrameworkCore.Repositories.PostgreSql.DbContexts;

namespace SharedKernel.Integration.Tests.Data.EntityFrameworkCore.Repositories.PostgreSql.Repositories;

public class EfPostgreSqlUserRepository : EntityFrameworkCoreRepositoryAsync<User, Guid>
{
    public EfPostgreSqlUserRepository(PostgreSqlSharedKernelDbContext sharedKernelDbContext) : base(sharedKernelDbContext)
    {
    }
}
