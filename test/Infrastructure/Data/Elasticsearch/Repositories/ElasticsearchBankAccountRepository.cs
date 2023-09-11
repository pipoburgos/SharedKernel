using SharedKernel.Domain.Tests.BankAccounts;
using SharedKernel.Infrastructure.Elasticsearch.Data.Repositories;

namespace SharedKernel.Integration.Tests.Data.Elasticsearch.Repositories;

public class ElasticsearchBankAccountRepository : ElasticsearchRepository<BankAccount, Guid>
{
    public ElasticsearchBankAccountRepository(SharedKernelUnitOfWork2 unitOfWork) : base(unitOfWork)
    {
    }
}
