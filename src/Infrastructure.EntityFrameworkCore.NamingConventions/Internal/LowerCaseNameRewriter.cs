namespace SharedKernel.Infrastructure.EntityFrameworkCore.NamingConventions.Internal;

/// <summary>  </summary>
public class LowerCaseNameRewriter : INameRewriter
{
    private readonly CultureInfo _culture;

    /// <summary>  </summary>
    public LowerCaseNameRewriter(CultureInfo culture)
    {
        _culture = culture;
    }

    /// <summary>  </summary>
    public string RewriteName(string name)
        => name.ToLower(_culture);
}
