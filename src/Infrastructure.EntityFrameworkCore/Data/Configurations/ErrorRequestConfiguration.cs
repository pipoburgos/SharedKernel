using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SharedKernel.Infrastructure.Requests.Middlewares.Failover;

namespace SharedKernel.Infrastructure.EntityFrameworkCore.Data.Configurations;

/// <summary>  </summary>
public class ErrorRequestConfiguration : IEntityTypeConfiguration<ErrorRequest>
{
    /// <summary>  </summary>
    public void Configure(EntityTypeBuilder<ErrorRequest> builder)
    {
        builder.Property(a => a.Id).ValueGeneratedNever().IsRequired().HasMaxLength(50);

        builder.Property(a => a.OccurredOn).IsRequired().HasMaxLength(50);

        builder.Property(a => a.Request).IsRequired();

        builder.Property(a => a.Exception).IsRequired();
    }
}
