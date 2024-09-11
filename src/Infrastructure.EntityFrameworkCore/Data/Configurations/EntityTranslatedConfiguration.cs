using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SharedKernel.Domain.Entities.Globalization;

namespace SharedKernel.Infrastructure.EntityFrameworkCore.Data.Configurations;

/// <summary> . </summary>
public class EntityTranslatedConfiguration<TEntityTranslated, TEntityId, TEntity, TLanguage, TLanguageKey> :
    IEntityTypeConfiguration<TEntityTranslated>
    where TEntityTranslated : class, IEntityTranslated<TEntityId, TEntity, TLanguage, TLanguageKey>
    where TEntity : class, IEntityIsTranslatable<TEntityId, TEntity, TEntityTranslated, TLanguage, TLanguageKey>
    where TLanguage : class
    where TEntityId : notnull
{
    /// <summary> . </summary>
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

/// <summary> . </summary>
public class EntityTranslatedConfiguration<TEntityTranslated, TEntityId, TEntity> :
    EntityTranslatedConfiguration<TEntityTranslated, TEntityId, TEntity, Language, string>
    where TEntityTranslated : class, IEntityTranslated<TEntityId, TEntity, Language, string>
    where TEntity : class, IEntityIsTranslatable<TEntityId, TEntity, TEntityTranslated, Language, string>
    where TEntityId : notnull
{
}
