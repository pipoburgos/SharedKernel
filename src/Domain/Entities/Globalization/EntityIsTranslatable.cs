namespace SharedKernel.Domain.Entities.Globalization;

/// <summary> . </summary>
public abstract class EntityIsTranslatable<TEntityId, TEntity, TTranslation, TLanguage, TLanguageKey> :
    EntityAuditableLogicalRemove<TEntityId>,
    IEntityIsTranslatable<TEntityId, TEntity, TTranslation, TLanguage, TLanguageKey>
    where TTranslation : class, IEntityTranslated<TEntityId, TEntity, TLanguage, TLanguageKey>
    where TEntityId : notnull
{
    private readonly List<TTranslation> _translations;

    /// <summary> Entity constructor for ORMs. </summary>
    protected EntityIsTranslatable()
    {
        _translations = new List<TTranslation>();
    }

    /// <summary> . </summary>
    public IEnumerable<TTranslation> Translations => _translations.AsEnumerable();

    /// <summary> . </summary>
    public void AddTranslation(TTranslation translation)
    {
        _translations.Add(translation);
    }
}

/// <summary> . </summary>
public abstract class EntityIsTranslatable<TEntityId, TEntity, TTranslation, TLanguage> :
    EntityIsTranslatable<TEntityId, TEntity, TTranslation, TLanguage, string>,
    IEntityIsTranslatable<TEntityId, TEntity, TTranslation, TLanguage>
    where TTranslation : class, IEntityTranslated<TEntityId, TEntity, TLanguage> where TEntityId : notnull
{
}

/// <summary> . </summary>
public abstract class EntityIsTranslatable<TEntityId, TEntity, TTranslation> :
    EntityIsTranslatable<TEntityId, TEntity, TTranslation, Language>,
    IEntityIsTranslatable<TEntityId, TEntity, TTranslation>
    where TTranslation : class, IEntityTranslated<TEntityId, TEntity> where TEntityId : notnull
{
}
