﻿using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Infrastructure.Newtonsoft;
using SharedKernel.Infrastructure.Redis.Data;
using SharedKernel.Integration.Tests.Data.CommonRepositoryTesting;
using SharedKernel.Integration.Tests.Data.Redis.DbContexts;
using SharedKernel.Integration.Tests.Data.Redis.Repositories;

namespace SharedKernel.Integration.Tests.Data.Redis;


public class RedisBankAccountRepositoryTests : BankAccountRepositoryCommonTestTests<RedisBankAccountRepository>
{
    protected override string GetJsonFile()
    {
        return "Data/Redis/appsettings.redis.json";
    }

    protected override IServiceCollection ConfigureServices(IServiceCollection services)
    {
        return services
            .AddSharedKernelRedisDbContext<SharedKernelRedisDbContext>(Configuration)
            .AddSharedKernelNewtonsoftSerializer()
            .AddTransient<RedisBankAccountRepository>();
    }
}
