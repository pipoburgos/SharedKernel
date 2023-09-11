using SharedKernel.Domain.Tests.BankAccounts;
using SharedKernel.Infrastructure.EntityFrameworkCore.Data.Repositories;
using SharedKernel.Integration.Tests.Data.EntityFrameworkCore.Repositories.PostgreSql.DbContexts;

namespace SharedKernel.Integration.Tests.Data.EntityFrameworkCore.Repositories.PostgreSql.Repositories;

public class EfPostgreSqlBankAccountRepository : EntityFrameworkCoreRepositoryAsync<BankAccount, Guid>
{
    public EfPostgreSqlBankAccountRepository(PostgreSqlSharedKernelDbContext sharedKernelDbContext) : base(sharedKernelDbContext)
    {
    }
}
