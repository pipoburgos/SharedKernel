﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Infrastructure.EntityFrameworkCore.SqlServer.Data;
using SharedKernel.Infrastructure.EntityFrameworkCore.SqlServer.System.Threading;
using SharedKernel.Integration.Tests.Data.EntityFrameworkCore.DbContexts;
using SharedKernel.Testing.Infrastructure;

namespace SharedKernel.Integration.Tests.System.Threading.SqlServer;

public class SqlServerApp : InfrastructureTestCase<FakeStartup>
{
    protected override string GetJsonFile()
    {
        return "System/Threading/SqlServer/appsettings.sqlServer.json";
    }

    public override void BeforeStart()
    {
        var dbContext = GetRequiredServiceOnNewScope<SharedKernelEntityFrameworkDbContext>();
        //dbContext.Database.EnsureDeleted();
        dbContext.Database.Migrate();
    }

    protected override IServiceCollection ConfigureServices(IServiceCollection services)
    {
        var connection = Configuration.GetConnectionString("RepositoryConnectionString")!;

        return services
            .AddSharedKernelEntityFrameworkCoreSqlServerUnitOfWork<ISharedKernelEntityFrameworkUnitOfWork, SharedKernelEntityFrameworkDbContext>(connection)
            .AddSharedKernelSqlServerMutex(connection);
    }
}
