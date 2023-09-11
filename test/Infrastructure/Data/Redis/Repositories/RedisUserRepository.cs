using SharedKernel.Domain.Tests.Users;
using SharedKernel.Infrastructure.Redis.Data.Repositories;
using SharedKernel.Integration.Tests.Data.Redis.DbContexts;

namespace SharedKernel.Integration.Tests.Data.Redis.Repositories;

public class RedisUserRepository : RedisRepository<User, Guid>
{
    public RedisUserRepository(SharedKernelRedisDbContext sharedKernelRedisDbContext) :
        base(sharedKernelRedisDbContext)
    {
    }
}
