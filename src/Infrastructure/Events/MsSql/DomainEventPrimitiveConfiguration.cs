using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SharedKernel.Domain.Events;

namespace SharedKernel.Infrastructure.Events.MsSql
{
    /// <summary>  </summary>
    public class DomainEventPrimitiveConfiguration : IEntityTypeConfiguration<DomainEventPrimitive>
    {
        /// <summary>  </summary>
        public void Configure(EntityTypeBuilder<DomainEventPrimitive> builder)
        {

        }
    }
}
