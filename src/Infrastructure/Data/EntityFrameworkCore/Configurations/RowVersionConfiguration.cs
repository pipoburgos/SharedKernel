using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SharedKernel.Domain.Entities;

namespace SharedKernel.Infrastructure.Data.EntityFrameworkCore.Configurations
{
    public class RowVersionConfiguration : IEntityTypeConfiguration<IRowVersion>
    {
        public void Configure(EntityTypeBuilder<IRowVersion> builder)
        {
            builder.Property(a => a.RowVersion).IsRowVersion();
        }
    }
}
