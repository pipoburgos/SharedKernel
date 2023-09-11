using SharedKernel.Domain.Tests.BankAccounts;
using SharedKernel.Infrastructure.EntityFrameworkCore.Data.Repositories;
using SharedKernel.Integration.Tests.Data.EntityFrameworkCore.DbContexts;

namespace SharedKernel.Integration.Tests.Data.EntityFrameworkCore.Repositories;

public class EfCoreBankAccountRepository : EntityFrameworkCoreRepositoryAsync<BankAccount, Guid>
{
    public EfCoreBankAccountRepository(SharedKernelEntityFrameworkDbContext sharedKernelDbContext) : base(sharedKernelDbContext)
    {
    }
}
