using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SharedKernel.Domain.Entities.Globalization;

namespace SharedKernel.Infrastructure.EntityFrameworkCore.Data.Configurations
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TEntityKey"></typeparam>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TTranslation"></typeparam>
    /// <typeparam name="TLanguage"></typeparam>
    /// <typeparam name="TLanguageKey"></typeparam>
    public class EntityIsTranslatableConfigurationBase<TEntityKey, TEntity, TTranslation, TLanguage, TLanguageKey> :
        IEntityTypeConfiguration<TEntity>
        where TEntity : EntityIsTranslatable<TEntityKey, TEntity, TTranslation, TLanguage, TLanguageKey>
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
    public class EntityIsTranslatableConfigurationBase<TEntityKey, TEntity, TTranslation> :
        EntityIsTranslatableConfigurationBase<TEntityKey, TEntity, TTranslation, Language, string>
        where TEntity : EntityIsTranslatable<TEntityKey, TEntity, TTranslation, Language, string>
        where TTranslation : class, IEntityTranslated<TEntityKey, TEntity, Language, string>
    {
    }
}
