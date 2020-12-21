using System;
using System.Linq.Expressions;
using System.Threading;
using SharedKernel.Domain.Entities.Globalization;
using SharedKernel.Domain.Specifications.Common;

namespace SharedKernel.Domain.Specifications
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TEntityKey"></typeparam>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TTranslation"></typeparam>
    /// <typeparam name="TLanguage"></typeparam>
    public class TranslatedSpecification<TEntityKey, TEntity, TTranslation, TLanguage>
        : Specification<TTranslation> where TTranslation : class,
        IEntityIsTranslatable<TEntityKey, TEntity, TTranslation, TLanguage>,
        IEntityTranslated<TEntityKey, TEntity, TLanguage>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override Expression<Func<TTranslation, bool>> SatisfiedBy()
        {
            var language = Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName;
            return t => t.LanguageId == language;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TEntityKey"></typeparam>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TTranslation"></typeparam>
    public class TranslatedSpecification<TEntityKey, TEntity, TTranslation>
        : TranslatedSpecification<TEntityKey, TEntity, TTranslation, Language> where TTranslation : class,
        IEntityIsTranslatable<TEntityKey, TEntity, TTranslation, Language>,
        IEntityTranslated<TEntityKey, TEntity, Language>
    {
    }
}
