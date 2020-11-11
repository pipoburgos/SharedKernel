using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SharedKernel.Domain.Entities;

namespace SharedKernel.Infrastructure.Data.EntityFrameworkCore.Configurations
{
    class AuditChangeConfiguration : IEntityTypeConfiguration<AuditChange>
    {
        public void Configure(EntityTypeBuilder<AuditChange> builder)
        {
            builder.Property(a => a.Id).ValueGeneratedNever();

            builder.Property(a => a.RegistryId).IsRequired().HasMaxLength(36);

            builder.Property(a => a.Table).IsRequired().HasMaxLength(100);

            builder.Property(a => a.Property).IsRequired().HasMaxLength(250);

            builder.Property(a => a.OriginalValue).IsRequired().HasMaxLength(4000);

            builder.Property(a => a.CurrentValue).IsRequired().HasMaxLength(4000);

            builder.Property(a => a.Date).IsRequired();

            builder.Property(a => a.State).IsRequired();
        }
    }
}
