using SharedKernel.Domain.Tests.Users;
using SharedKernel.Infrastructure.Elasticsearch.Data.Repositories;

namespace SharedKernel.Integration.Tests.Data.Elasticsearch.Repositories;

public class ElasticsearchUserRepository : ElasticsearchRepository<User, Guid>
{
    public ElasticsearchUserRepository(SharedKernelUnitOfWork2 unitOfWork) : base(unitOfWork)
    {
    }
}
