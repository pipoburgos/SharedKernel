using BankAccounts.Domain.BankAccounts;

namespace BankAccounts.Infrastructure.BankAccounts.Configurations
{
    internal class BankAccountConfiguration : IEntityTypeConfiguration<BankAccount>
    {
        public void Configure(EntityTypeBuilder<BankAccount> builder)
        {
            builder
                .Property(x => x.Id)
                .HasConversion(x => x.Value, l => BankAccountId.Create(l))
                .IsRequired();

            builder.OwnsOne(ba => ba.InternationalBankAccountNumber, ba =>
            {
                ba.Property(e => e.CountryCheckDigit).HasMaxLength(4);
                ba.Property(e => e.EntityCode).HasMaxLength(4);
                ba.Property(e => e.OfficeNumber).HasMaxLength(4);
                ba.Property(e => e.ControlDigit).HasMaxLength(2);
                ba.Property(e => e.AccountNumber).HasMaxLength(10).HasColumnName("Number");
            });
        }
    }
}
