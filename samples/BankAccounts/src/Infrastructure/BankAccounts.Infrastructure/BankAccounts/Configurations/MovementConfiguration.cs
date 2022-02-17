using BankAccounts.Domain.BankAccounts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BankAccounts.Infrastructure.BankAccounts.Configurations
{
    internal class MovementConfiguration : IEntityTypeConfiguration<Movement>
    {
        public void Configure(EntityTypeBuilder<Movement> builder)
        {
            builder.Property(e => e.Id)
                .ValueGeneratedNever();

            builder.Property(e => e.Concept)
                .HasMaxLength(100);

            builder.Property(e => e.Date);

            builder.HasOne<BankAccount>()
                .WithMany(e => e.Movements)
                .HasForeignKey(e => e.BankAccountId)
                .HasConstraintName("FK_BankAccount");
        }
    }
}
