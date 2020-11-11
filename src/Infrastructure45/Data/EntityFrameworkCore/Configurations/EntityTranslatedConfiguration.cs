using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SharedKernel.Domain.Entities.Globalization;

namespace SharedKernel.Infrastructure.Data.EntityFrameworkCore.Configurations
{
    public class EntityTranslatedConfiguration<TEntityTranslated, TEntityKey, TEntity, TLanguage, TLanguageKey> :
        IEntityTypeConfiguration<TEntityTranslated>
        where TEntityTranslated : class, IEntityTranslated<TEntityKey, TEntity, TLanguage, TLanguageKey>
        where TEntity : class, IEntityIsTranslatable<TEntityKey, TEntity, TEntityTranslated, TLanguage, TLanguageKey>
        where TLanguage : class
    {
        public virtual void Configure(EntityTypeBuilder<TEntityTranslated> builder)
        {
            builder.HasKey(a => new { a.EntityId, a.LanguageId });

            builder.HasOne(x => x.Entity)
                .WithMany(x => x.Translations)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Language)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);
        }
    }

    public class EntityTranslatedConfiguration<TEntityTranslated, TEntityKey, TEntity> :
        EntityTranslatedConfiguration<TEntityTranslated, TEntityKey, TEntity, Language, string>
        where TEntityTranslated : class, IEntityTranslated<TEntityKey, TEntity, Language, string>
        where TEntity : class, IEntityIsTranslatable<TEntityKey, TEntity, TEntityTranslated, Language, string>
    {
    }
}
