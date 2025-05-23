﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Domain.Tests.Users;
using SharedKernel.Infrastructure.Dapper.Data.Queries;
using SharedKernel.Infrastructure.Dapper.SqlServer.Data;
using SharedKernel.Infrastructure.Logging;
using SharedKernel.Integration.Tests.Data.EntityFrameworkCore.DbContexts;
using SharedKernel.Testing.Infrastructure;

namespace SharedKernel.Integration.Tests.Data.Dapper.Queries;


public class SqlServerDapperQueryProviderTests : InfrastructureTestCase<FakeStartup>
{
    protected override string GetJsonFile()
    {
        return "Data/Dapper/Queries/appsettings.sqlServer.json";
    }

    protected override IServiceCollection ConfigureServices(IServiceCollection services)
    {
        var connection = Configuration.GetConnectionString("DapperConnectionString");

        return services
            .AddDbContext<SharedKernelEntityFrameworkDbContext>(options => options.UseSqlServer(connection!), ServiceLifetime.Transient)
            .AddDbContextFactory<SharedKernelEntityFrameworkDbContext>()
            .AddSharedKernelLogging()
            .AddSharedKernelDapperSqlServer(Configuration.GetConnectionString("DapperConnectionString")!);
    }

    [Fact]
    public async Task ExecuteQuery()
    {
        var dbContext = await Regenerate();
        var user = UserMother.Create();
        dbContext.Set<User>().Add(user);
        await dbContext.SaveChangesAsync();

        var result = await GetRequiredServiceOnNewScope<DapperQueryProvider>()
            .ExecuteQueryFirstOrDefaultAsync<int>($"SELECT COUNT(*) FROM skr.[User] WHERE Id = '{user.Id}'");

        result.Should().Be(1);

        await dbContext.DisposeAsync();
    }

    private async Task<DbContext> Regenerate(CancellationToken cancellationToken = default)
    {
        var dbContext = GetRequiredServiceOnNewScope<SharedKernelEntityFrameworkDbContext>();
        await dbContext.Database.EnsureDeletedAsync(cancellationToken);
        await dbContext.Database.MigrateAsync(cancellationToken);

        return dbContext;
    }
}