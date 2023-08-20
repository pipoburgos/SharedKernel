using BankAccounts.Application.Shared.UnitOfWork;
using SharedKernel.Infrastructure.EntityFrameworkCore.Data.Configurations;
using SharedKernel.Infrastructure.EntityFrameworkCore.Data.DbContexts;

namespace BankAccounts.Infrastructure.Shared.Data;

internal class BankAccountDbContext : DbContextBase, IBankAccountUnitOfWork
{
    public BankAccountDbContext(DbContextOptions<BankAccountDbContext> options,
        IValidatableObjectService validatableObjectService, IAuditableService auditable = default)
        : base(options, "dbo", typeof(BankAccountDbContext).Assembly, validatableObjectService, auditable)
    { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfiguration(new ErrorRequestConfiguration());
    }
}
