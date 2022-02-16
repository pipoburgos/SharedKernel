using BankAccounts.Application.Shared.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Infrastructure.Data.EntityFrameworkCore.DbContexts;

namespace BankAccounts.Infrastructure.Shared.Data
{
    internal class BankAccountDbContext : DbContextBase, IBankAccountUnitOfWork
    {
        public BankAccountDbContext(DbContextOptions<BankAccountDbContext> options, IAuditableService auditable = null)
            : base(options, "dbo", typeof(BankAccountDbContext).Assembly, auditable) { }
    }
}
