using Elasticsearch.Net;
using SharedKernel.Application.Serializers;
using SharedKernel.Domain.Tests.Users;
using SharedKernel.Infrastructure.Elasticsearch.Data.Repositories;

namespace SharedKernel.Integration.Tests.Data.Elasticsearch.Repositories;

public class ElasticsearchUserRepository : ElasticsearchRepository<User, Guid>
{
    public ElasticsearchUserRepository(SharedKernelUnitOfWork unitOfWork, ElasticLowLevelClient client,
        IJsonSerializer jsonSerializer) : base(unitOfWork, client, jsonSerializer)
    {
    }
}
