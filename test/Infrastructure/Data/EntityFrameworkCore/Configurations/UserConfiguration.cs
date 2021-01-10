using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;
using SharedKernel.Domain.Tests.Users;

namespace SharedKernel.Infraestructure.Tests.Data.EntityFrameworkCore.Configurations
{
    internal class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            // This line will map private members
            builder.Metadata.SetPropertyAccessMode(PropertyAccessMode.PreferField);

            // This line will map private navigation properties
            builder.Metadata.SetNavigationAccessMode(PropertyAccessMode.Field);


            // This Converter will perform the conversion to and from Json to the desired type
            builder.Property(e => e.Emails).HasConversion(
                v => JsonConvert.SerializeObject(v, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }),
                v => JsonConvert.DeserializeObject<List<string>>(v, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }));

            builder.Property(e => e.Addresses).HasColumnName("JsonAddresses").HasConversion(
                v => JsonConvert.SerializeObject(v, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }),
                v => JsonConvert.DeserializeObject<List<Address>>(v, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }));
        }
    }
}
