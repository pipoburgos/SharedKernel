namespace SharedKernel.Domain.Entities.Globalization
{
    public abstract class
        EntityTranslated<TEntityKey, TEntity, TLanguage, TLanguageKey> : IEntityTranslated<TEntityKey, TEntity,
            TLanguage, TLanguageKey> where TEntity : IEntity<TEntityKey>
    {
        #region Properties

        public bool Translated { get; protected set; }

        #endregion

        #region Navigation Properties

        public TEntityKey EntityId { get; protected set; }
        public TEntity Entity { get; private set; }

        public TLanguageKey LanguageId { get; protected set; }
        public TLanguage Language { get; private set; }

        #endregion
    }

    public abstract class
        EntityTranslated<TEntityKey, TEntity, TLanguage> : EntityTranslated<TEntityKey, TEntity, TLanguage, string>,
            IEntityTranslated<TEntityKey, TEntity, TLanguage> where TEntity : IEntity<TEntityKey>
    {
    }

    public abstract class EntityTranslated<TEntityKey, TEntity> : EntityTranslated<TEntityKey, TEntity, Language>,
        IEntityTranslated<TEntityKey, TEntity> where TEntity : IEntity<TEntityKey>

    {
    }
}