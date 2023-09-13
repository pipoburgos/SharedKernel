using Microsoft.Extensions.Caching.Distributed;
using SharedKernel.Application.Serializers;
using SharedKernel.Application.Validator;
using SharedKernel.Infrastructure.Data.Services;
using SharedKernel.Infrastructure.Redis.Data.DbContexts;

namespace BankAccounts.Infrastructure.Shared.Data;
internal class BankAccountRedisDbContext : RedisDbContext
{
    public BankAccountRedisDbContext(IDistributedCache distributedCache, IJsonSerializer jsonSerializer,
        IEntityAuditableService auditableService, IClassValidatorService classValidatorService) : base(distributedCache,
        jsonSerializer, auditableService, classValidatorService)

    {
    }
}
