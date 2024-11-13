namespace SharedKernel.Infrastructure.EntityFrameworkCore.OpenIddict;

/// <summary> . </summary>
public class ExternalLogins
{
    /// <summary> . </summary>
    public Facebook? Facebook { get; set; }

    /// <summary> . </summary>
    public Google? Google { get; set; }
}

/// <summary> . </summary>
public class Facebook
{
    /// <summary> . </summary>
    public string AppId { get; set; } = null!;

    /// <summary> . </summary>
    public string AppSecret { get; set; } = null!;
}

/// <summary> . </summary>
public class Google
{
    /// <summary> . </summary>
    public string ClientId { get; set; } = null!;

    /// <summary> . </summary>
    public string ClientSecret { get; set; } = null!;
}