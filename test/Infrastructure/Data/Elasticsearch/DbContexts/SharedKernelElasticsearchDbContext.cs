using Elastic.Clients.Elasticsearch;
using Elasticsearch.Net;
using SharedKernel.Application.Serializers;
using SharedKernel.Application.Validator;
using SharedKernel.Infrastructure.Data.Services;
using SharedKernel.Infrastructure.Elasticsearch.Data.DbContexts;

namespace SharedKernel.Integration.Tests.Data.Elasticsearch.DbContexts;

public class SharedKernelElasticsearchDbContext : ElasticsearchDbContext, ISharedKernelElasticsearchUnitOfWork
{
    public SharedKernelElasticsearchDbContext(ElasticsearchClient client, ElasticLowLevelClient lowLevelClient, IJsonSerializer jsonSerializer, IEntityAuditableService auditableService,
        IClassValidatorService classValidatorService) : base(client, lowLevelClient, jsonSerializer, auditableService, classValidatorService)
    {
    }
}
