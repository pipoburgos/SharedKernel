namespace SharedKernel.Domain.Entities.Globalization;

/// <summary>  </summary>
public class LanguageTranslated : EntityTranslated<string, Language>
{
    /// <summary>  </summary>
    protected LanguageTranslated() { }

    /// <summary>  </summary>
    protected LanguageTranslated(string entityId, string languageId, bool translated, string name)
    {
        EntityId = entityId;
        LanguageId = languageId;
        Translated = translated;
        Name = name;
    }

    /// <summary>  </summary>
    public static LanguageTranslated Create(string entityId, string languageId, bool translated, string name)
    {
        return new LanguageTranslated(entityId, languageId, translated, name);
    }

    /// <summary>  </summary>
    public string Name { get; private set; } = null!;
}
