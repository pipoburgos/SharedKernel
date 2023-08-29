namespace SharedKernel.Domain.Entities.Globalization;

/// <summary>  </summary>
public abstract class EntityTranslated<TEntityKey, TEntity, TLanguage, TLanguageKey> :
    IEntityTranslated<TEntityKey, TEntity, TLanguage, TLanguageKey> where TEntity : IEntity<TEntityKey> where TEntityKey : notnull
{
    /// <summary>  </summary>
    protected EntityTranslated() { }

    /// <summary>  </summary>
    protected EntityTranslated(TEntityKey entityId, TEntity entity, TLanguageKey languageId, TLanguage language, bool translated = true)
    {
        Translated = translated;
        EntityId = entityId;
        Entity = entity;
        LanguageId = languageId;
        Language = language;
    }

    /// <summary>  </summary>
    public bool Translated { get; protected set; }

    /// <summary>  </summary>
    public TEntityKey EntityId { get; protected set; } = default!;

    /// <summary>  </summary>
    public TEntity Entity { get; private set; } = default!;

    /// <summary>  </summary>
    public TLanguageKey LanguageId { get; protected set; } = default!;

    /// <summary>  </summary>
    public TLanguage Language { get; private set; } = default!;
}

/// <summary>  </summary>
public abstract class EntityTranslated<TEntityKey, TEntity, TLanguage> : EntityTranslated<TEntityKey, TEntity, TLanguage, string>,
    IEntityTranslated<TEntityKey, TEntity, TLanguage> where TEntity : IEntity<TEntityKey> where TEntityKey : notnull
{
}

/// <summary>  </summary>
public abstract class EntityTranslated<TEntityKey, TEntity> : EntityTranslated<TEntityKey, TEntity, Language>,
    IEntityTranslated<TEntityKey, TEntity> where TEntity : IEntity<TEntityKey> where TEntityKey : notnull
{
}
