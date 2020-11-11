using System.Collections.Generic;
using SharedKernel.Domain.Entities.Globalization;

namespace SharedKernel.Domain.Aggregates
{
    public abstract class AggregateRootIsTranslatable<TEntityKey, TEntity, TTranslation, TLanguage, TLanguageKey> :
        AggregateRootAuditableLogicalRemove<TEntityKey>,
        IEntityIsTranslatable<TEntityKey, TEntity, TTranslation, TLanguage, TLanguageKey>
        where TTranslation : class, IEntityTranslated<TEntityKey, TEntity, TLanguage, TLanguageKey>
    {
        private List<TTranslation> _translations;

        #region Navigation Properties

        public IEnumerable<TTranslation> Translations => _translations;

        #endregion

        protected void AddTranslation(TTranslation translation)
        {
            if(_translations == default)
                _translations = new List<TTranslation>();

            _translations.Add(translation);
        }
    }

    public abstract class AggregateRootIsTranslatable<TEntityKey, TEntity, TTranslation, TLanguage> :
        AggregateRootIsTranslatable<TEntityKey, TEntity, TTranslation, TLanguage, string>,
        IEntityIsTranslatable<TEntityKey, TEntity, TTranslation, TLanguage>
        where TTranslation : class, IEntityTranslated<TEntityKey, TEntity, TLanguage>
    {
    }

    public abstract class AggregateRootIsTranslatable<TEntityKey, TEntity, TTranslation> :
        AggregateRootIsTranslatable<TEntityKey, TEntity, TTranslation, Language>,
        IEntityIsTranslatable<TEntityKey, TEntity, TTranslation>
        where TTranslation : class, IEntityTranslated<TEntityKey, TEntity>
    {
    }
}