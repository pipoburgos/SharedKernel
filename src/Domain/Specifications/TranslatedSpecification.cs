using SharedKernel.Domain.Entities.Globalization;

namespace SharedKernel.Domain.Specifications;

/// <summary> . </summary>
public class TranslatedSpecification<TEntityId, TEntity, TTranslation, TLanguage>
        : Specification<TTranslation> where TTranslation : class,
        IEntityIsTranslatable<TEntityId, TEntity, TTranslation, TLanguage>,
        IEntityTranslated<TEntityId, TEntity, TLanguage> where TEntityId : notnull
{
    /// <summary> . </summary>
    public override Expression<Func<TTranslation, bool>> SatisfiedBy()
    {
        var language = Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName;
        return t => t.LanguageId == language;
    }
}

/// <summary> . </summary>
public class TranslatedSpecification<TEntityId, TEntity, TTranslation>
    : TranslatedSpecification<TEntityId, TEntity, TTranslation, Language> where TTranslation : class,
    IEntityIsTranslatable<TEntityId, TEntity, TTranslation, Language>,
    IEntityTranslated<TEntityId, TEntity, Language> where TEntityId : notnull
{
}
