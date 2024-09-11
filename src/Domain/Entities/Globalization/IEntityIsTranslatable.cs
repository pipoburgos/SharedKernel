namespace SharedKernel.Domain.Entities.Globalization;

/// <summary> . </summary>
public interface IEntityIsTranslatable<out TTranslation>
{
    /// <summary> . </summary>
    IEnumerable<TTranslation> Translations { get; }
}

/// <summary> . </summary>
public interface IEntityIsTranslatable<TEntityId, TEntity, out TTranslation, TLanguage, TLanguageKey> :
    IEntityIsTranslatable<TTranslation>
    where TTranslation : IEntityTranslated<TEntityId, TEntity, TLanguage, TLanguageKey> where TEntityId : notnull
{
}

/// <summary> . </summary>
public interface IEntityIsTranslatable<TEntityId, TEntity, out TTranslation, TLanguage> :
    IEntityIsTranslatable<TEntityId, TEntity, TTranslation, TLanguage, string>
    where TTranslation : IEntityTranslated<TEntityId, TEntity, TLanguage> where TEntityId : notnull
{
}

/// <summary> . </summary>
public interface IEntityIsTranslatable<TEntityId, TEntity, out TTranslation> :
    IEntityIsTranslatable<TEntityId, TEntity, TTranslation, Language>
    where TTranslation : IEntityTranslated<TEntityId, TEntity, Language> where TEntityId : notnull
{
}
