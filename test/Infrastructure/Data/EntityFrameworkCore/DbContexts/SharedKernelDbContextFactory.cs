using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace SharedKernel.Integration.Tests.Data.EntityFrameworkCore.DbContexts;

internal class SharedKernelDbContextFactory : IDesignTimeDbContextFactory<SharedKernelEntityFrameworkDbContext>
{
    public SharedKernelEntityFrameworkDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<SharedKernelEntityFrameworkDbContext>();
        optionsBuilder.UseSqlServer("Server=.;Database=SharedKernelTests;User ID=test;Password=test;MultipleActiveResultSets=true;Application Name=SharedKernel;")
            .EnableSensitiveDataLogging();

        return new SharedKernelEntityFrameworkDbContext(optionsBuilder.Options);
    }
}
