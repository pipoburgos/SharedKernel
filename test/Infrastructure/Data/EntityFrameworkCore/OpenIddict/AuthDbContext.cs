using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Application.Auth.UnitOfWork;
using SharedKernel.Domain.RailwayOrientedProgramming;

namespace SharedKernel.Integration.Tests.Data.EntityFrameworkCore.OpenIddict;

internal class AuthDbContext : IdentityDbContext<IdentityUser<Guid>, IdentityRole<Guid>, Guid>, IDataProtectionKeyContext, IAuthUnitOfWork
{
    public Guid Id { get; }

    public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
    {
        Id = Guid.NewGuid();
    }

    public DbSet<DataProtectionKey> DataProtectionKeys { get; set; }

    public Result<int> SaveChangesResult()
    {
        var count = base.SaveChanges();
        return count.Success();
    }

    public int Rollback()
    {
        return 0;
    }

    public Result<int> RollbackResult()
    {
        return 0.Success();
    }

    public async Task<Result<int>> SaveChangesResultAsync(CancellationToken cancellationToken)
    {
        var count = await base.SaveChangesAsync(cancellationToken);
        return count.Success();
    }

    public Task<int> RollbackAsync(CancellationToken cancellationToken)
    {
        return Task.FromResult(0);
    }

    public Task<Result<int>> RollbackResultAsync(CancellationToken cancellationToken)
    {
        return Task.FromResult(0.Success());
    }
}
