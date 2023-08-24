namespace SharedKernel.Domain.Entities.Globalization
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TTranslation"></typeparam>
    public interface IEntityIsTranslatable<out TTranslation>
    {
        /// <summary>
        /// 
        /// </summary>
        IEnumerable<TTranslation> Translations { get; }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TEntityKey"></typeparam>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TTranslation"></typeparam>
    /// <typeparam name="TLanguage"></typeparam>
    /// <typeparam name="TLanguageKey"></typeparam>
    public interface IEntityIsTranslatable<TEntityKey, TEntity, out TTranslation, TLanguage,
        TLanguageKey> : IEntityIsTranslatable<TTranslation>
        where TTranslation : IEntityTranslated<TEntityKey, TEntity, TLanguage, TLanguageKey>
    {
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TEntityKey"></typeparam>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TTranslation"></typeparam>
    /// <typeparam name="TLanguage"></typeparam>
    public interface IEntityIsTranslatable<TEntityKey, TEntity, out TTranslation, TLanguage>
        : IEntityIsTranslatable<TEntityKey, TEntity, TTranslation, TLanguage, string>
        where TTranslation : IEntityTranslated<TEntityKey, TEntity, TLanguage>
    {
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TEntityKey"></typeparam>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TTranslation"></typeparam>
    public interface IEntityIsTranslatable<TEntityKey, TEntity, out TTranslation>
        : IEntityIsTranslatable<TEntityKey, TEntity, TTranslation, Language>
        where TTranslation : IEntityTranslated<TEntityKey, TEntity, Language>
    {
    }
}
