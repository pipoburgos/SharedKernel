using SharedKernel.Domain.Entities.Globalization;

namespace SharedKernel.Domain.Aggregates;

/// <summary> . </summary>
public abstract class AggregateRootIsTranslatable<TEntityId, TEntity, TTranslation, TLanguage, TLanguageKey> :
    AggregateRootAuditableLogicalRemove<TEntityId>, IEntityIsTranslatable<TEntityId, TEntity, TTranslation, TLanguage, TLanguageKey>
    where TTranslation : class, IEntityTranslated<TEntityId, TEntity, TLanguage, TLanguageKey>
    where TEntityId : notnull
{
    private readonly List<TTranslation> _translations;

    /// <summary> . </summary>
    protected AggregateRootIsTranslatable()
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
public abstract class AggregateRootIsTranslatable<TEntityId, TEntity, TTranslation, TLanguage> :
    AggregateRootIsTranslatable<TEntityId, TEntity, TTranslation, TLanguage, string>, IEntityIsTranslatable<TEntityId, TEntity, TTranslation, TLanguage>
    where TTranslation : class, IEntityTranslated<TEntityId, TEntity, TLanguage> where TEntityId : notnull
{
}

/// <summary> . </summary>
public abstract class AggregateRootIsTranslatable<TEntityId, TEntity, TTranslation> :
    AggregateRootIsTranslatable<TEntityId, TEntity, TTranslation, Language>, IEntityIsTranslatable<TEntityId, TEntity, TTranslation>
    where TTranslation : class, IEntityTranslated<TEntityId, TEntity> where TEntityId : notnull
{
}
