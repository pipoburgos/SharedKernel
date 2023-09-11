using SharedKernel.Domain.Tests.BankAccounts;
using SharedKernel.Infrastructure.Redis.Data.Repositories;
using SharedKernel.Integration.Tests.Data.Redis.DbContexts;

namespace SharedKernel.Integration.Tests.Data.Redis.Repositories;

public class RedisBankAccountRepository : RedisRepository<BankAccount, Guid>
{
    public RedisBankAccountRepository(SharedKernelRedisDbContext sharedKernelRedisDbContext) : base(
        sharedKernelRedisDbContext)
    {
    }
}
