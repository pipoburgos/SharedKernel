using SharedKernel.Domain.Entities.Globalization;

namespace SharedKernel.Domain.Aggregates;

/// <summary>  </summary>
public abstract class AggregateRootIsTranslatable<TEntityKey, TEntity, TTranslation, TLanguage, TLanguageKey> :
    AggregateRootAuditableLogicalRemove<TEntityKey>, IEntityIsTranslatable<TEntityKey, TEntity, TTranslation, TLanguage, TLanguageKey>
    where TTranslation : class, IEntityTranslated<TEntityKey, TEntity, TLanguage, TLanguageKey>
    where TEntityKey : notnull
{
    private readonly List<TTranslation> _translations;

    /// <summary>  </summary>
    protected AggregateRootIsTranslatable()
    {
        _translations = new List<TTranslation>();
    }

    /// <summary>  </summary>
    public IEnumerable<TTranslation> Translations => _translations.AsEnumerable();

    /// <summary>  </summary>
    public void AddTranslation(TTranslation translation)
    {
        _translations.Add(translation);
    }
}

/// <summary>  </summary>
public abstract class AggregateRootIsTranslatable<TEntityKey, TEntity, TTranslation, TLanguage> :
    AggregateRootIsTranslatable<TEntityKey, TEntity, TTranslation, TLanguage, string>, IEntityIsTranslatable<TEntityKey, TEntity, TTranslation, TLanguage>
    where TTranslation : class, IEntityTranslated<TEntityKey, TEntity, TLanguage> where TEntityKey : notnull
{
}

/// <summary>  </summary>
public abstract class AggregateRootIsTranslatable<TEntityKey, TEntity, TTranslation> :
    AggregateRootIsTranslatable<TEntityKey, TEntity, TTranslation, Language>, IEntityIsTranslatable<TEntityKey, TEntity, TTranslation>
    where TTranslation : class, IEntityTranslated<TEntityKey, TEntity> where TEntityKey : notnull
{
}
