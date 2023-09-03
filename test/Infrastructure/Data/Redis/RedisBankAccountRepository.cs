using Microsoft.Extensions.Caching.Distributed;
using SharedKernel.Application.Serializers;
using SharedKernel.Domain.Tests.BankAccounts;
using SharedKernel.Infrastructure.Redis.Data.Repositories;

namespace SharedKernel.Integration.Tests.Data.Redis;

public class RedisBankAccountRepository : RedisRepositoryAsync<BankAccount, Guid>
{
    public RedisBankAccountRepository(SharedKernelUnitOfWork unitOfWork, IDistributedCache distributedCache,
        IJsonSerializer jsonSerializer) : base(unitOfWork, distributedCache, jsonSerializer)
    {
    }
}
