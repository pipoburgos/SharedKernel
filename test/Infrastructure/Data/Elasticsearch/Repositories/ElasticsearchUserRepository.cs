using SharedKernel.Domain.Tests.Users;
using SharedKernel.Infrastructure.Elasticsearch.Data.Repositories;
using SharedKernel.Integration.Tests.Data.Elasticsearch.DbContexts;

namespace SharedKernel.Integration.Tests.Data.Elasticsearch.Repositories;

public class ElasticsearchUserRepository : ElasticsearchRepository<User, Guid>
{
    public ElasticsearchUserRepository(SharedKernelElasticsearchDbContext sharedKernelElasticsearchDbContext) : base(
        sharedKernelElasticsearchDbContext)
    {
    }
}
