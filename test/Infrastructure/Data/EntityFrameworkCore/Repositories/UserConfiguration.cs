using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SharedKernel.Domain.Tests.Users;

namespace SharedKernel.Infraestructure.Tests.Data.EntityFrameworkCore.Repositories
{
    internal class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(x => x.Id).ValueGeneratedNever();

            builder.Property(u => u.Name).HasMaxLength(256);
        }
    }
}
