using Microsoft.Extensions.Caching.Distributed;
using SharedKernel.Application.Serializers;
using SharedKernel.Domain.Tests.Users;
using SharedKernel.Infrastructure.Redis.Data.Repositories;

namespace SharedKernel.Integration.Tests.Data.Redis;

public class RedisUserRepository : RedisRepositoryAsync<User, Guid>
{
    public RedisUserRepository(SharedKernelUnitOfWork unitOfWork, IDistributedCache distributedCache,
        IJsonSerializer jsonSerializer) : base(unitOfWork, distributedCache, jsonSerializer)
    {
    }
}
