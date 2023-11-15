namespace SharedKernel.Infrastructure.EntityFrameworkCore.NamingConventions.Internal;

/// <summary>  </summary>
public class CamelCaseNameRewriter : INameRewriter
{
    private readonly CultureInfo _culture;

    /// <summary>  </summary>
    public CamelCaseNameRewriter(CultureInfo culture)
    {
        _culture = culture;
    }

    /// <summary>  </summary>
    public string RewriteName(string name)
        => string.IsNullOrEmpty(name) ? name: char.ToLower(name[0], _culture) + name.Substring(1);
}
