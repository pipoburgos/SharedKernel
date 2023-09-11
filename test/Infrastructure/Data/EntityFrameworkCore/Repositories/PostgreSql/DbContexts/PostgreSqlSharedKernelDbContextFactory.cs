using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace SharedKernel.Integration.Tests.Data.EntityFrameworkCore.Repositories.PostgreSql.DbContexts
{
    public class PostgreSqlSharedKernelDbContextFactory : IDesignTimeDbContextFactory<PostgreSqlSharedKernelDbContext>
    {
        public PostgreSqlSharedKernelDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<PostgreSqlSharedKernelDbContext>();
            optionsBuilder.UseNpgsql("Server=localhost;Port=22226;Database=postgres_testing;User Id=admin;Password=password;TrustServerCertificate=True;Application Name=SharedKernel;")
                .UseSnakeCaseNamingConvention()
                .EnableSensitiveDataLogging();

            return new PostgreSqlSharedKernelDbContext(optionsBuilder.Options);
        }
    }
}
