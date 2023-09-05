using Elasticsearch.Net;
using SharedKernel.Application.Serializers;
using SharedKernel.Domain.Tests.BankAccounts;
using SharedKernel.Infrastructure.Elasticsearch.Data.Repositories;

namespace SharedKernel.Integration.Tests.Data.Elasticsearch.Repositories;

public class ElasticsearchBankAccountRepository : ElasticsearchRepositoryAsync<BankAccount, Guid>
{
    public ElasticsearchBankAccountRepository(SharedKernelUnitOfWork unitOfWork, ElasticLowLevelClient client,
        IJsonSerializer jsonSerializer) : base(unitOfWork, client, jsonSerializer)
    {
    }
}
