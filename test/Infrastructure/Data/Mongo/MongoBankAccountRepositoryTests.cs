﻿using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Infrastructure.Mongo.Data;
using SharedKernel.Integration.Tests.Data.CommonRepositoryTesting;
using SharedKernel.Integration.Tests.Data.Mongo.DbContexts;
using SharedKernel.Integration.Tests.Data.Mongo.Repositories;

namespace SharedKernel.Integration.Tests.Data.Mongo;

[Collection("DockerHook")]
public class MongoBankAccountRepositoryTests : BankAccountRepositoryCommonTestTests<MongoBankAccountRepository>
{
    protected override string GetJsonFile()
    {
        return "Data/Mongo/appsettings.mongo.json";
    }

    protected override IServiceCollection ConfigureServices(IServiceCollection services)
    {
        return services
            .AddMongoDbContext<SharedKernelMongoDbContext>(Configuration)
            .AddTransient<MongoBankAccountRepository>();
    }
}
