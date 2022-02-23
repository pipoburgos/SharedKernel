using BankAccounts.Domain.BankAccounts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace BankAccounts.Infrastructure.BankAccounts.Configurations
{
    internal class UserConfiguration : IEntityTypeConfiguration<User>
    {
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
                    v => JsonConvert.SerializeObject(v,
                        new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }),
                    v => JsonConvert.DeserializeObject<IEnumerable<string>>(v,
                        new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }));
        }
    }
}
