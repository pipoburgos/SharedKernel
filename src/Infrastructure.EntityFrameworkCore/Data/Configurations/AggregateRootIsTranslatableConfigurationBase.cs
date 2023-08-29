using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SharedKernel.Domain.Aggregates;
using SharedKernel.Domain.Entities.Globalization;

namespace SharedKernel.Infrastructure.EntityFrameworkCore.Data.Configurations;

/// <summary>  </summary>
public class AggregateRootIsTranslatableConfigurationBase<TEntityId, TEntity, TTranslation, TLanguage, TLanguageKey> :
        IEntityTypeConfiguration<TEntity>
        where TEntity : AggregateRootIsTranslatable<TEntityId, TEntity, TTranslation, TLanguage, TLanguageKey>
        where TTranslation : class, IEntityTranslated<TEntityId, TEntity, TLanguage, TLanguageKey> where TEntityId : notnull
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="builder"></param>
    public virtual void Configure(EntityTypeBuilder<TEntity> builder)
    {
        builder.HasMany<TTranslation>().WithOne(x => x.Entity).OnDelete(DeleteBehavior.Restrict);
    }
}

/// <summary>  </summary>
public class AggregateRootIsTranslatableConfigurationBase<TEntityId, TEntity, TTranslation> :
    AggregateRootIsTranslatableConfigurationBase<TEntityId, TEntity, TTranslation, Language, string>
    where TEntity : AggregateRootIsTranslatable<TEntityId, TEntity, TTranslation, Language, string>
    where TTranslation : class, IEntityTranslated<TEntityId, TEntity, Language, string> where TEntityId : notnull
{
}
