using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SharedKernel.Domain.Entities.Globalization;

namespace SharedKernel.Infrastructure.Data.EntityFrameworkCore.Configurations
{
    public class EntityIsTranslatableConfigurationBase<TEntityKey, TEntity, TTranslation, TLanguage, TLanguageKey> :
        IEntityTypeConfiguration<TEntity>
        where TEntity : EntityIsTranslatable<TEntityKey, TEntity, TTranslation, TLanguage, TLanguageKey>
        where TTranslation : class, IEntityTranslated<TEntityKey, TEntity, TLanguage, TLanguageKey>
    {
        public virtual void Configure(EntityTypeBuilder<TEntity> builder)
        {
            builder.HasMany<TTranslation>().WithOne(x => x.Entity).OnDelete(DeleteBehavior.Restrict);
        }
    }

    public class EntityIsTranslatableConfigurationBase<TEntityKey, TEntity, TTranslation> :
        EntityIsTranslatableConfigurationBase<TEntityKey, TEntity, TTranslation, Language, string>
        where TEntity : EntityIsTranslatable<TEntityKey, TEntity, TTranslation, Language, string>
        where TTranslation : class, IEntityTranslated<TEntityKey, TEntity, Language, string>
    {
    }
}
