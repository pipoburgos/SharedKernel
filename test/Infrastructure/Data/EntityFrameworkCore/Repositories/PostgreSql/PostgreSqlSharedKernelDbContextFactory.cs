using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using SharedKernel.Integration.Tests.Data.EntityFrameworkCore.DbContexts;

namespace SharedKernel.Integration.Tests.Data.EntityFrameworkCore.Repositories.PostgreSql
{
    public class PostgreSqlSharedKernelDbContextFactory : IDesignTimeDbContextFactory<PostgreSqlSharedKernelDbContext>
    {
        public PostgreSqlSharedKernelDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<SharedKernelDbContext>();
            optionsBuilder.UseNpgsql("Server=localhost;Port=22226;Database=postgres_testing;User Id=admin;Password=password;TrustServerCertificate=True;Application Name=SharedKernel;")
                .EnableSensitiveDataLogging();

            return new PostgreSqlSharedKernelDbContext(optionsBuilder.Options);
        }
    }
}
