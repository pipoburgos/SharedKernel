#if NET8_0
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Infrastructure.EntityFrameworkCore.PostgreSQL.Data;
using SharedKernel.Integration.Tests.Data.CommonRepositoryTesting;
using SharedKernel.Integration.Tests.Data.EntityFrameworkCore.Repositories.PostgreSql.DbContexts;
using SharedKernel.Integration.Tests.Data.EntityFrameworkCore.Repositories.PostgreSql.Repositories;

namespace SharedKernel.Integration.Tests.Data.EntityFrameworkCore.Repositories.PostgreSql;


public class EfPostgreSqlUserUnitOfWorkTests : UserUnitOfWorkTests<EfPostgreSqlUserRepository, IPostgreSqlSharedKernelUnitOfWork>
{
    protected override string GetJsonFile()
    {
        return "Data/EntityFrameworkCore/Repositories/PostgreSql/appsettings.postgreSql.json";
    }

    public override void BeforeStart()
    {
        var dbContext = GetRequiredService<PostgreSqlSharedKernelDbContext>();
        //dbContext.Database.EnsureDeleted();
        dbContext.Database.Migrate();
    }

    protected override IServiceCollection ConfigureServices(IServiceCollection services)
    {
        var connection = Configuration.GetConnectionString("RepositoryConnectionString")!;
        return services
            .AddEntityFrameworkCorePostgreSqlUnitOfWork<IPostgreSqlSharedKernelUnitOfWork,
                PostgreSqlSharedKernelDbContext>(connection)
            .AddTransient<EfPostgreSqlUserRepository>();
    }
}
#endif