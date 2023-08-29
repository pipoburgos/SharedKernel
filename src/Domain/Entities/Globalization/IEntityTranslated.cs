namespace SharedKernel.Domain.Entities.Globalization;

/// <summary>  </summary>
public interface IEntityTranslated<out TEntityId, out TEntity, out TLanguage, out TLanguageKey> where TEntityId : notnull
{
    /// <summary>
    /// 
    /// </summary>
    bool Translated { get; }

    /// <summary>
    /// 
    /// </summary>
    TEntityId EntityId { get; }

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
public interface IEntityTranslated<out TEntityId, out TEntity, out TLanguage>
    : IEntityTranslated<TEntityId, TEntity, TLanguage, string> where TEntityId : notnull
{
}

/// <summary>  </summary>
public interface IEntityTranslated<out TEntityId, out TEntity>
    : IEntityTranslated<TEntityId, TEntity, Language> where TEntityId : notnull
{
}
