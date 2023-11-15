using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SharedKernel.Domain.Entities.Globalization;

namespace SharedKernel.Infrastructure.EntityFrameworkCore.Data.Configurations;

/// <summary>
/// 
/// </summary>
public class LanguageConfiguration : IEntityTypeConfiguration<Language>
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="builder"></param>
    public void Configure(EntityTypeBuilder<Language> builder)
    {
        builder.Property(a => a.Name).IsRequired().HasMaxLength(50);
    }
}