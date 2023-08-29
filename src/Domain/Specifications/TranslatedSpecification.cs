using SharedKernel.Domain.Entities.Globalization;

namespace SharedKernel.Domain.Specifications;

/// <summary>  </summary>
public class TranslatedSpecification<TEntityKey, TEntity, TTranslation, TLanguage>
        : Specification<TTranslation> where TTranslation : class,
        IEntityIsTranslatable<TEntityKey, TEntity, TTranslation, TLanguage>,
        IEntityTranslated<TEntityKey, TEntity, TLanguage> where TEntityKey : notnull
{
    /// <summary>  </summary>
    public override Expression<Func<TTranslation, bool>> SatisfiedBy()
    {
        var language = Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName;
        return t => t.LanguageId == language;
    }
}

/// <summary>  </summary>
public class TranslatedSpecification<TEntityKey, TEntity, TTranslation>
    : TranslatedSpecification<TEntityKey, TEntity, TTranslation, Language> where TTranslation : class,
    IEntityIsTranslatable<TEntityKey, TEntity, TTranslation, Language>,
    IEntityTranslated<TEntityKey, TEntity, Language> where TEntityKey : notnull
{
}
