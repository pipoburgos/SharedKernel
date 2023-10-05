namespace SharedKernel.Infrastructure.EntityFrameworkCore.NamingConventions.Internal;

/// <summary>  </summary>
public class UpperCaseNameRewriter : INameRewriter
{
    private readonly CultureInfo _culture;

    /// <summary>  </summary>
    public UpperCaseNameRewriter(CultureInfo culture)
    {
        _culture = culture;
    }

    /// <summary>  </summary>
    public string RewriteName(string name)
        => name.ToUpper(_culture);
}
