using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SharedKernel.Domain.Tests.BankAccounts;

namespace SharedKernel.Integration.Tests.Data.EntityFrameworkCore.Configurations;

internal class BankAccountConfiguration : IEntityTypeConfiguration<BankAccount>
{
    public void Configure(EntityTypeBuilder<BankAccount> builder)
    {
    }
}
