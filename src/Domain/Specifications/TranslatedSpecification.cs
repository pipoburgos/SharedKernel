using System;
using System.Linq.Expressions;
using System.Threading;
using SharedKernel.Domain.Entities.Globalization;
using SharedKernel.Domain.Specifications.Common;

namespace SharedKernel.Domain.Specifications
{
    public class TranslatedSpecification<TEntityKey, TEntity, TTranslation, TLanguage>
        : Specification<TTranslation> where TTranslation : class,
        IEntityIsTranslatable<TEntityKey, TEntity, TTranslation, TLanguage>,
        IEntityTranslated<TEntityKey, TEntity, TLanguage>
    {
        public override Expression<Func<TTranslation, bool>> SatisfiedBy()
        {
            var language = Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName;
            return t => t.LanguageId == language;
        }
    }

    public class TranslatedSpecification<TEntityKey, TEntity, TTranslation>
        : TranslatedSpecification<TEntityKey, TEntity, TTranslation, Language> where TTranslation : class,
        IEntityIsTranslatable<TEntityKey, TEntity, TTranslation, Language>,
        IEntityTranslated<TEntityKey, TEntity, Language>
    {
    }
}
