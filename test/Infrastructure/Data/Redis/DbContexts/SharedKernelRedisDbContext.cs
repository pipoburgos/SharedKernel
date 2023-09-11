using Microsoft.Extensions.Caching.Distributed;
using SharedKernel.Application.Serializers;
using SharedKernel.Application.Validator;
using SharedKernel.Infrastructure.Data.Services;
using SharedKernel.Infrastructure.Redis.Data.DbContexts;

namespace SharedKernel.Integration.Tests.Data.Redis.DbContexts;

public class SharedKernelRedisDbContext : RedisDbContext, ISharedKernelRedisUnitOfWork
{
    public SharedKernelRedisDbContext(IDistributedCache distributedCache, IJsonSerializer jsonSerializer,
        IEntityAuditableService auditableService, IClassValidatorService classValidatorService) : base(distributedCache,
        jsonSerializer, auditableService, classValidatorService)
    {
    }
}
