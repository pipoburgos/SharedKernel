using BankAccounts.Application.Shared.UnitOfWork;
using BankAccounts.Infrastructure.BankAccounts.Configurations;
using SharedKernel.Application.Serializers;
using SharedKernel.Application.Validator;
using SharedKernel.Infrastructure.EntityFrameworkCore.Communication.Email;
using SharedKernel.Infrastructure.EntityFrameworkCore.Data.DbContexts;
using SharedKernel.Infrastructure.EntityFrameworkCore.Data.Services;
using SharedKernel.Infrastructure.EntityFrameworkCore.Requests.Middlewares;

namespace BankAccounts.Infrastructure.Shared.Data;

internal class BankAccountDbContext : EntityFrameworkDbContext, IBankAccountUnitOfWork
{
    public BankAccountDbContext(DbContextOptions<BankAccountDbContext> options,
        IJsonSerializer jsonSerializer,
        IClassValidatorService? classValidatorService, IAuditableService? auditable = default)
        : base(options, "dbo", typeof(BankAccountDbContext).Assembly, jsonSerializer, classValidatorService, auditable)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfiguration(new ErrorRequestConfiguration());
        modelBuilder.ApplyConfiguration(new OutboxMailConfiguration(JsonSerializer!));
        modelBuilder.ApplyConfiguration(new UserConfiguration(JsonSerializer!));
    }
}
