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
    public required string AppId { get; set; }

    /// <summary> . </summary>
    public required string AppSecret { get; set; }
}

/// <summary> . </summary>
public class Google
{
    /// <summary> . </summary>
    public required string ClientId { get; set; }

    /// <summary> . </summary>
    public required string ClientSecret { get; set; }
}