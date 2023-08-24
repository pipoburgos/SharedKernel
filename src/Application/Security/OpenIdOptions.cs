namespace SharedKernel.Application.Security;

/// <summary> Authentication configuration. </summary>
public class OpenIdOptions
{
    /// <summary> Authority. </summary>
    public string Authority { get; set; }

    /// <summary>  </summary>
    public bool RequireHttpsMetadata { get; set; }

    /// <summary>  </summary>
    public string Audience { get; set; }

    /// <summary>  </summary>
    public string ClientId { get; set; }

    /// <summary> / </summary>
    public string ClientSecret { get; set; }

    /// <summary>  </summary>
    public IEnumerable<Scope> Scopes { get; set; }

    /// <summary>  </summary>
    public int AccessTokenSecondsLifetime { get; set; } = 300;
}

/// <summary>  </summary>
public class Scope
{
    /// <summary>  </summary>
    public string Name { get; set; }

    /// <summary>  </summary>
    public string DisplayName { get; set; }
}
