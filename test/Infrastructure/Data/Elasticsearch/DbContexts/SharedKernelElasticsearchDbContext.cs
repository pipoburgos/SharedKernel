﻿using Elastic.Clients.Elasticsearch;
using SharedKernel.Application.Validator;
using SharedKernel.Infrastructure.Data.Services;
using SharedKernel.Infrastructure.Elasticsearch.Data.DbContexts;

namespace SharedKernel.Integration.Tests.Data.Elasticsearch.DbContexts;

public class SharedKernelElasticsearchDbContext : ElasticsearchDbContext, ISharedKernelElasticsearchUnitOfWork
{
    public SharedKernelElasticsearchDbContext(ElasticsearchClient client, IEntityAuditableService auditableService,
        IClassValidatorService classValidatorService) : base(client, auditableService, classValidatorService)
    {
    }
}
