namespace SharedKernel.Domain.Entities.Globalization;

/// <summary>  </summary>
public abstract class EntityTranslated<TEntityId, TEntity, TLanguage, TLanguageKey> :
    IEntityTranslated<TEntityId, TEntity, TLanguage, TLanguageKey> where TEntity : IEntity<TEntityId> where TEntityId : notnull
{
    /// <summary>  </summary>
    protected EntityTranslated() { }

    /// <summary>  </summary>
    protected EntityTranslated(TEntityId entityId, TEntity entity, TLanguageKey languageId, TLanguage language, bool translated = true)
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
    public TEntityId EntityId { get; protected set; } = default!;

    /// <summary>  </summary>
    public TEntity Entity { get; private set; } = default!;

    /// <summary>  </summary>
    public TLanguageKey LanguageId { get; protected set; } = default!;

    /// <summary>  </summary>
    public TLanguage Language { get; private set; } = default!;
}

/// <summary>  </summary>
public abstract class EntityTranslated<TEntityId, TEntity, TLanguage> : EntityTranslated<TEntityId, TEntity, TLanguage, string>,
    IEntityTranslated<TEntityId, TEntity, TLanguage> where TEntity : IEntity<TEntityId> where TEntityId : notnull
{
}

/// <summary>  </summary>
public abstract class EntityTranslated<TEntityId, TEntity> : EntityTranslated<TEntityId, TEntity, Language>,
    IEntityTranslated<TEntityId, TEntity> where TEntity : IEntity<TEntityId> where TEntityId : notnull
{
}
