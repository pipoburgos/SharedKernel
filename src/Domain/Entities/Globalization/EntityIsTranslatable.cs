using System.Collections.Generic;

namespace SharedKernel.Domain.Entities.Globalization
{
    public abstract class EntityIsTranslatable<TEntityKey, TEntity, TTranslation, TLanguage, TLanguageKey> :
        EntityAuditableLogicalRemove<TEntityKey>,
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

    public abstract class EntityIsTranslatable<TEntityKey, TEntity, TTranslation, TLanguage> :
        EntityIsTranslatable<TEntityKey, TEntity, TTranslation, TLanguage, string>,
        IEntityIsTranslatable<TEntityKey, TEntity, TTranslation, TLanguage>
        where TTranslation : class, IEntityTranslated<TEntityKey, TEntity, TLanguage>
    {
    }

    public abstract class EntityIsTranslatable<TEntityKey, TEntity, TTranslation> :
        EntityIsTranslatable<TEntityKey, TEntity, TTranslation, Language>,
        IEntityIsTranslatable<TEntityKey, TEntity, TTranslation>
        where TTranslation : class, IEntityTranslated<TEntityKey, TEntity>
    {
    }
}