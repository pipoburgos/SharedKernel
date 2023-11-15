namespace SharedKernel.Infrastructure.EntityFrameworkCore.NamingConventions.Internal;

/// <summary>  </summary>
public class UpperSnakeCaseNameRewriter : SnakeCaseNameRewriter
{
    private readonly CultureInfo _culture;

    /// <summary>  </summary>
    public UpperSnakeCaseNameRewriter(CultureInfo culture) : base(culture)
    {
        _culture = culture;
    }

    /// <summary>  </summary>
    public override string RewriteName(string name)
        => base.RewriteName(name).ToUpper(_culture);
}
