using System.Collections.Generic;

namespace SharedKernel.Domain.Entities.Globalization
{
    public interface IEntityIsTranslatable<out TTranslation>
    {
        IEnumerable<TTranslation> Translations { get; }
    }

    public interface
        IEntityIsTranslatable<TEntityKey, TEntity, out TTranslation, TLanguage,
            TLanguageKey> : IEntityIsTranslatable<TTranslation>
        where TTranslation : IEntityTranslated<TEntityKey, TEntity, TLanguage, TLanguageKey>
    {
    }

    public interface IEntityIsTranslatable<TEntityKey, TEntity, out TTranslation, TLanguage>
        : IEntityIsTranslatable<TEntityKey, TEntity, TTranslation, TLanguage, string>
        where TTranslation : IEntityTranslated<TEntityKey, TEntity, TLanguage>
    {
    }

    public interface IEntityIsTranslatable<TEntityKey, TEntity, out TTranslation>
        : IEntityIsTranslatable<TEntityKey, TEntity, TTranslation, Language>
        where TTranslation : IEntityTranslated<TEntityKey, TEntity, Language>
    {
    }
}
