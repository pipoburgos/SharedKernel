namespace SharedKernel.Domain.Entities.Globalization
{
    public interface IEntityTranslated<out TEntityKey, out TEntity, out TLanguage, out TLanguageKey>
    {
        bool Translated { get; }

        TEntityKey EntityId { get; }
        TEntity Entity { get; }

        TLanguageKey LanguageId { get; }
        TLanguage Language { get; }
    }

    public interface IEntityTranslated<out TEntityKey, out TEntity, out TLanguage>
        : IEntityTranslated<TEntityKey, TEntity, TLanguage, string>
    {
    }

    public interface IEntityTranslated<out TEntityKey, out TEntity>
        : IEntityTranslated<TEntityKey, TEntity, Language>
    {
    }
}
