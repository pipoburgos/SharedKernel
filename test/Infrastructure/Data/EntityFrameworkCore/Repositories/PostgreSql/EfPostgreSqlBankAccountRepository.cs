using SharedKernel.Domain.Tests.BankAccounts;
using SharedKernel.Infrastructure.EntityFrameworkCore.Data.Repositories;

namespace SharedKernel.Integration.Tests.Data.EntityFrameworkCore.Repositories.PostgreSql;

public class EfPostgreSqlBankAccountRepository : EntityFrameworkCoreRepositoryAsync<BankAccount, Guid>
{
    public EfPostgreSqlBankAccountRepository(PostgreSqlSharedKernelDbContext sharedKernelDbContext) : base(sharedKernelDbContext)
    {
    }
}
