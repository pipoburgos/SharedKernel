﻿using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Infrastructure.Mongo.Data;
using SharedKernel.Integration.Tests.Data.CommonRepositoryTesting;
using SharedKernel.Integration.Tests.Data.Mongo.DbContexts;
using SharedKernel.Integration.Tests.Data.Mongo.Repositories;
using Xunit;

namespace SharedKernel.Integration.Tests.Data.Mongo;

[Collection("DockerHook")]
public class MongoUserUnitOfWorkTests : UserUnitOfWorkTests<MongoUserRepository, ISharedKernelMongoUnitOfWork>
{
    protected override string GetJsonFile()
    {
        return "Data/Mongo/appsettings.mongo.json";
    }

    protected override IServiceCollection ConfigureServices(IServiceCollection services)
    {
        return services
            .AddMongoUnitOfWork<ISharedKernelMongoUnitOfWork, SharedKernelMongoDbContext>(Configuration)
            .AddTransient<MongoUserRepository>();
    }
}
