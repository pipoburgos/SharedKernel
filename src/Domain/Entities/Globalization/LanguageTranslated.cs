namespace SharedKernel.Domain.Entities.Globalization
{
    /// <summary>
    /// 
    /// </summary>
    public class LanguageTranslated : EntityTranslated<string, Language>
    {
        private LanguageTranslated() {}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityId"></param>
        /// <param name="languageId"></param>
        /// <param name="translated"></param>
        /// <param name="name"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 
        /// </summary>
        public string Name { get; private set; }

        #endregion
    }
}
