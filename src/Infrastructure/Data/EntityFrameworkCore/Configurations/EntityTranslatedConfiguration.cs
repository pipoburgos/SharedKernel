using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SharedKernel.Domain.Entities.Globalization;

namespace SharedKernel.Infrastructure.Data.EntityFrameworkCore.Configurations
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TEntityTranslated"></typeparam>
    /// <typeparam name="TEntityKey"></typeparam>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TLanguage"></typeparam>
    /// <typeparam name="TLanguageKey"></typeparam>
    public class EntityTranslatedConfiguration<TEntityTranslated, TEntityKey, TEntity, TLanguage, TLanguageKey> :
        IEntityTypeConfiguration<TEntityTranslated>
        where TEntityTranslated : class, IEntityTranslated<TEntityKey, TEntity, TLanguage, TLanguageKey>
        where TEntity : class, IEntityIsTranslatable<TEntityKey, TEntity, TEntityTranslated, TLanguage, TLanguageKey>
        where TLanguage : class
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
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

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TEntityTranslated"></typeparam>
    /// <typeparam name="TEntityKey"></typeparam>
    /// <typeparam name="TEntity"></typeparam>
    public class EntityTranslatedConfiguration<TEntityTranslated, TEntityKey, TEntity> :
        EntityTranslatedConfiguration<TEntityTranslated, TEntityKey, TEntity, Language, string>
        where TEntityTranslated : class, IEntityTranslated<TEntityKey, TEntity, Language, string>
        where TEntity : class, IEntityIsTranslatable<TEntityKey, TEntity, TEntityTranslated, Language, string>
    {
    }
}
