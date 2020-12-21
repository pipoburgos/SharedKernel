namespace SharedKernel.Domain.Entities.Globalization
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TEntityKey"></typeparam>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TLanguage"></typeparam>
    /// <typeparam name="TLanguageKey"></typeparam>
    public interface IEntityTranslated<out TEntityKey, out TEntity, out TLanguage, out TLanguageKey>
    {
        /// <summary>
        /// 
        /// </summary>
        bool Translated { get; }

        /// <summary>
        /// 
        /// </summary>
        TEntityKey EntityId { get; }

        /// <summary>
        /// 
        /// </summary>
        TEntity Entity { get; }

        /// <summary>
        /// 
        /// </summary>
        TLanguageKey LanguageId { get; }

        /// <summary>
        /// 
        /// </summary>
        TLanguage Language { get; }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TEntityKey"></typeparam>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TLanguage"></typeparam>
    public interface IEntityTranslated<out TEntityKey, out TEntity, out TLanguage>
        : IEntityTranslated<TEntityKey, TEntity, TLanguage, string>
    {
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TEntityKey"></typeparam>
    /// <typeparam name="TEntity"></typeparam>
    public interface IEntityTranslated<out TEntityKey, out TEntity>
        : IEntityTranslated<TEntityKey, TEntity, Language>
    {
    }
}
