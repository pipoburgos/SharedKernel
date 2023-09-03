using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Infrastructure.EntityFrameworkCore.SqlServer;
using SharedKernel.Integration.Tests.Data.CommonRepositoryTesting;
using SharedKernel.Integration.Tests.Data.EntityFrameworkCore.DbContexts;
using Xunit;

namespace SharedKernel.Integration.Tests.Data.EntityFrameworkCore.Repositories.SqlServer;

[Collection("DockerHook")]
public class EfCoreSqlServerBankAccountRepositoryTests : BankAccountRepositoryCommonTestTests<EfCoreBankAccountRepository>
{
    protected override string GetJsonFile()
    {
        return "Data/EntityFrameworkCore/Repositories/SqlServer/appsettings.sqlServer.json";
    }

    protected override void Regenerate()
    {
        var dbContext = GetRequiredServiceOnNewScope<SharedKernelDbContext>();
        //dbContext.Database.EnsureDeleted();
        dbContext.Database.Migrate();
    }

    protected override IServiceCollection ConfigureServices(IServiceCollection services)
    {
        var connection = Configuration.GetConnectionString("RepositoryConnectionString")!;
        return services
            .AddEntityFrameworkCoreSqlServerUnitOfWorkAsync<ISharedKernelUnitOfWork, SharedKernelDbContext>(connection)
            .AddTransient<EfCoreBankAccountRepository>();
    }
}
