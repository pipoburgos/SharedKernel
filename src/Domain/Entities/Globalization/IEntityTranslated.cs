namespace SharedKernel.Domain.Entities.Globalization;

/// <summary>  </summary>
public interface IEntityTranslated<out TEntityKey, out TEntity, out TLanguage, out TLanguageKey> where TEntityKey : notnull
{
    /// <summary>
    /// 
    /// </summary>
    bool Translated { get; }

    /// <summary>
    /// 
    /// </summary>
    TEntityKey EntityId { get; }

    /// <summary>
    /// 
    /// </summary>
    TEntity Entity { get; }

    /// <summary>
    /// 
    /// </summary>
    TLanguageKey LanguageId { get; }

    /// <summary>
    /// 
    /// </summary>
    TLanguage Language { get; }
}

/// <summary>  </summary>
public interface IEntityTranslated<out TEntityKey, out TEntity, out TLanguage>
    : IEntityTranslated<TEntityKey, TEntity, TLanguage, string> where TEntityKey : notnull
{
}

/// <summary>  </summary>
public interface IEntityTranslated<out TEntityKey, out TEntity>
    : IEntityTranslated<TEntityKey, TEntity, Language> where TEntityKey : notnull
{
}
