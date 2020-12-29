using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SharedKernel.Domain.Aggregates;
using SharedKernel.Domain.Entities.Globalization;

namespace SharedKernel.Infrastructure.Data.EntityFrameworkCore.Configurations
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TEntityKey"></typeparam>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TTranslation"></typeparam>
    /// <typeparam name="TLanguage"></typeparam>
    /// <typeparam name="TLanguageKey"></typeparam>
    public class AggregateRootIsTranslatableConfigurationBase<TEntityKey, TEntity, TTranslation, TLanguage, TLanguageKey> :
        IEntityTypeConfiguration<TEntity>
        where TEntity : AggregateRootIsTranslatable<TEntityKey, TEntity, TTranslation, TLanguage, TLanguageKey>
        where TTranslation : class, IEntityTranslated<TEntityKey, TEntity, TLanguage, TLanguageKey>
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

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TEntityKey"></typeparam>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TTranslation"></typeparam>
    public class AggregateRootIsTranslatableConfigurationBase<TEntityKey, TEntity, TTranslation> :
        AggregateRootIsTranslatableConfigurationBase<TEntityKey, TEntity, TTranslation, Language, string>
        where TEntity : AggregateRootIsTranslatable<TEntityKey, TEntity, TTranslation, Language, string>
        where TTranslation : class, IEntityTranslated<TEntityKey, TEntity, Language, string>
    {
    }
}
