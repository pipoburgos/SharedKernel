using Elasticsearch.Net;
using SharedKernel.Application.Serializers;
using SharedKernel.Domain.Aggregates;
using SharedKernel.Infrastructure.Data.UnitOfWorks;

namespace SharedKernel.Infrastructure.Elasticsearch.Data.Repositories;

/// <summary>  </summary>
public abstract class ElasticsearchRepositoryAsync<TAggregateRoot, TId> : ElasticsearchRepository<TAggregateRoot, TId>
    where TAggregateRoot : AggregateRoot<TId> where TId : notnull
{
    /// <summary>  </summary>
    protected ElasticsearchRepositoryAsync(UnitOfWork unitOfWork, ElasticLowLevelClient client,
        IJsonSerializer jsonSerializer) : base(unitOfWork, client, jsonSerializer)
    {
    }
}
