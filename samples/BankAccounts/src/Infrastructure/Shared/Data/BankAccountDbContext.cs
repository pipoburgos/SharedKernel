using BankAccounts.Application.Shared.UnitOfWork;
using SharedKernel.Application.Validator;
using SharedKernel.Infrastructure.EntityFrameworkCore.Data.Configurations;
using SharedKernel.Infrastructure.EntityFrameworkCore.Data.DbContexts;

namespace BankAccounts.Infrastructure.Shared.Data;

internal class BankAccountDbContext : EntityFrameworkDbContext, IBankAccountUnitOfWork
{
    public BankAccountDbContext(DbContextOptions<BankAccountDbContext> options,
        IClassValidatorService? classValidatorService, IAuditableService? auditable = default)
        : base(options, "dbo", typeof(BankAccountDbContext).Assembly, classValidatorService, auditable)
    { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfiguration(new ErrorRequestConfiguration());
    }
}
