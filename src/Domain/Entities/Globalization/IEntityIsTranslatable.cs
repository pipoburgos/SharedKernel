namespace SharedKernel.Domain.Entities.Globalization;

/// <summary>  </summary>
public interface IEntityIsTranslatable<out TTranslation>
{
    /// <summary>  </summary>
    IEnumerable<TTranslation> Translations { get; }
}

/// <summary>  </summary>
public interface IEntityIsTranslatable<TEntityKey, TEntity, out TTranslation, TLanguage, TLanguageKey> :
    IEntityIsTranslatable<TTranslation>
    where TTranslation : IEntityTranslated<TEntityKey, TEntity, TLanguage, TLanguageKey> where TEntityKey : notnull
{
}

/// <summary>  </summary>
public interface IEntityIsTranslatable<TEntityKey, TEntity, out TTranslation, TLanguage> :
    IEntityIsTranslatable<TEntityKey, TEntity, TTranslation, TLanguage, string>
    where TTranslation : IEntityTranslated<TEntityKey, TEntity, TLanguage> where TEntityKey : notnull
{
}

/// <summary>  </summary>
public interface IEntityIsTranslatable<TEntityKey, TEntity, out TTranslation> :
    IEntityIsTranslatable<TEntityKey, TEntity, TTranslation, Language>
    where TTranslation : IEntityTranslated<TEntityKey, TEntity, Language> where TEntityKey : notnull
{
}
