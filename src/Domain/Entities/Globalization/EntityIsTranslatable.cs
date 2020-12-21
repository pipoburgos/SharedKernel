using System.Collections.Generic;

namespace SharedKernel.Domain.Entities.Globalization
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TEntityKey"></typeparam>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TTranslation"></typeparam>
    /// <typeparam name="TLanguage"></typeparam>
    /// <typeparam name="TLanguageKey"></typeparam>
    public abstract class EntityIsTranslatable<TEntityKey, TEntity, TTranslation, TLanguage, TLanguageKey> :
        EntityAuditableLogicalRemove<TEntityKey>,
        IEntityIsTranslatable<TEntityKey, TEntity, TTranslation, TLanguage, TLanguageKey>
        where TTranslation : class, IEntityTranslated<TEntityKey, TEntity, TLanguage, TLanguageKey>
    {
        private List<TTranslation> _translations;

        #region Navigation Properties

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<TTranslation> Translations => _translations;

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="translation"></param>
        protected void AddTranslation(TTranslation translation)
        {
            if(_translations == default)
                _translations = new List<TTranslation>();

            _translations.Add(translation);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TEntityKey"></typeparam>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TTranslation"></typeparam>
    /// <typeparam name="TLanguage"></typeparam>
    public abstract class EntityIsTranslatable<TEntityKey, TEntity, TTranslation, TLanguage> :
        EntityIsTranslatable<TEntityKey, TEntity, TTranslation, TLanguage, string>,
        IEntityIsTranslatable<TEntityKey, TEntity, TTranslation, TLanguage>
        where TTranslation : class, IEntityTranslated<TEntityKey, TEntity, TLanguage>
    {
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TEntityKey"></typeparam>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TTranslation"></typeparam>
    public abstract class EntityIsTranslatable<TEntityKey, TEntity, TTranslation> :
        EntityIsTranslatable<TEntityKey, TEntity, TTranslation, Language>,
        IEntityIsTranslatable<TEntityKey, TEntity, TTranslation>
        where TTranslation : class, IEntityTranslated<TEntityKey, TEntity>
    {
    }
}