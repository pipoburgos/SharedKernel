using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace SharedKernel.Integration.Tests.Data.EntityFrameworkCore.DbContexts
{
    public class SharedKernelDbContextFactory : IDesignTimeDbContextFactory<SharedKernelDbContext>
    {
        public SharedKernelDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<SharedKernelDbContext>();
            optionsBuilder.UseSqlServer("Server=.;Database=SharedKernelTests;User ID=test;Password=test;MultipleActiveResultSets=true;Application Name=SharedKernel;")
                .EnableSensitiveDataLogging();

            return new SharedKernelDbContext(optionsBuilder.Options);
        }
    }
}
