using BankAccounts.Domain.Documents;
using SharedKernel.Infrastructure.Redis.Data.Repositories;

namespace BankAccounts.Infrastructure.Documents;

internal sealed class DocumentRepository : RedisRepository<Document, Guid>, IDocumentRepository
{
    public DocumentRepository(BankAccountRedisDbContext redisDbContext) : base(redisDbContext)
    {
    }
}
