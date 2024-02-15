using BankAccounts.Domain.BankAccounts;
using SharedKernel.Application.Serializers;
using SharedKernel.Domain.ValueObjects;

namespace BankAccounts.Infrastructure.BankAccounts.Configurations;

internal sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    private readonly IJsonSerializer _jsonSerializer;

    public UserConfiguration(IJsonSerializer jsonSerializer)
    {
        _jsonSerializer = jsonSerializer;
    }

    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.
            Property(e => e.Id)
            .ValueGeneratedNever();

        builder
            .HasMany<BankAccount>()
            .WithOne(e => e.Owner);

        builder
            .Property(e => e.Birthdate)
            .HasColumnType("date");

        builder
            .Property(e => e.Name)
            .HasMaxLength(100);

        builder
            .Property(e => e.Surname)
            .HasMaxLength(100);

        // This Converter will perform the conversion to and from Json to the desired type
        builder
            .Property(e => e.Emails)
            .HasConversion(
                v => _jsonSerializer.Serialize(v, NamingConvention.CamelCase),
                v => _jsonSerializer.Deserialize<IEnumerable<Email>>(v, NamingConvention.CamelCase));
    }
}
