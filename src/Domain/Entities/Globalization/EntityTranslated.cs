namespace SharedKernel.Domain.Entities.Globalization
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TEntityKey"></typeparam>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TLanguage"></typeparam>
    /// <typeparam name="TLanguageKey"></typeparam>
    public abstract class EntityTranslated<TEntityKey, TEntity, TLanguage, TLanguageKey> :
        IEntityTranslated<TEntityKey, TEntity,
            TLanguage, TLanguageKey>
        where TEntity : IEntity<TEntityKey>
    {
        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public bool Translated { get; protected set; }

        #endregion

        #region Navigation Properties

        /// <summary>
        /// 
        /// </summary>
        public TEntityKey EntityId { get; protected set; }

        /// <summary>
        /// 
        /// </summary>
        public TEntity Entity { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public TLanguageKey LanguageId { get; protected set; }

        /// <summary>
        /// 
        /// </summary>
        public TLanguage Language { get; private set; }

        #endregion
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TEntityKey"></typeparam>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TLanguage"></typeparam>
    public abstract class EntityTranslated<TEntityKey, TEntity, TLanguage> :
        EntityTranslated<TEntityKey, TEntity, TLanguage, string>,
        IEntityTranslated<TEntityKey, TEntity, TLanguage>
        where TEntity : IEntity<TEntityKey>
    {
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TEntityKey"></typeparam>
    /// <typeparam name="TEntity"></typeparam>
    public abstract class EntityTranslated<TEntityKey, TEntity> :
        EntityTranslated<TEntityKey, TEntity, Language>,
        IEntityTranslated<TEntityKey, TEntity>
        where TEntity : IEntity<TEntityKey>

    {
    }
}