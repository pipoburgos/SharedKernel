using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace SharedKernel.Integration.Tests.Data.EntityFrameworkCore.OpenIddict;

internal class AuthDbContextFactory : IDesignTimeDbContextFactory<AuthDbContext>
{
    public AuthDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AuthDbContext>();
        optionsBuilder.UseSqlServer("Server=.;Database=SharedKernelTests;User ID=test;Password=test;MultipleActiveResultSets=true;Application Name=SharedKernel;")
            .EnableSensitiveDataLogging();

        return new AuthDbContext(optionsBuilder.Options);
    }
}
