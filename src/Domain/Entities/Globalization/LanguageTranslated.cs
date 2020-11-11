namespace SharedKernel.Domain.Entities.Globalization
{
    public class LanguageTranslated : EntityTranslated<string, Language>
    {
        private LanguageTranslated() {}

        public static LanguageTranslated Create(string entityId, string languageId, bool translated, string name)
        {
            return new LanguageTranslated
            {
                EntityId = entityId,
                LanguageId = languageId,
                Translated = translated,
                Name = name
            };
        }

        #region Properties

        public string Name { get; private set; }

        #endregion
    }
}
