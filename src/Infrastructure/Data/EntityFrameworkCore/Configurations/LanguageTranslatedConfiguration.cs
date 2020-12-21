using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SharedKernel.Domain.Entities.Globalization;

namespace SharedKernel.Infrastructure.Data.EntityFrameworkCore.Configurations
{
    /// <summary>
    /// 
    /// </summary>
    public class LanguageTranslatedConfiguration : IEntityTypeConfiguration<LanguageTranslated>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        public void Configure(EntityTypeBuilder<LanguageTranslated> builder)
        {
            builder.Property(a => a.Name).IsRequired().HasMaxLength(50);
        }
    }
}
