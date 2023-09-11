using SharedKernel.Domain.Tests.BankAccounts;
using SharedKernel.Infrastructure.Elasticsearch.Data.Repositories;
using SharedKernel.Integration.Tests.Data.Elasticsearch.DbContexts;

namespace SharedKernel.Integration.Tests.Data.Elasticsearch.Repositories;

public class ElasticsearchBankAccountRepository : ElasticsearchRepository<BankAccount, Guid>
{
    public ElasticsearchBankAccountRepository(SharedKernelElasticsearchDbContext sharedKernelElasticsearchDbContext) :
        base(sharedKernelElasticsearchDbContext)
    {
    }
}
