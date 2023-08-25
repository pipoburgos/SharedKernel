namespace SharedKernel.Infrastructure.ActiveDirectory;

/// <summary>  </summary>
public class ActiveDirectorySettings
{
    /// <summary>  </summary>
    public string? Path { get; set; }

    /// <summary>  </summary>
    public string CommonNamesKey { get; set; } = null!;

    /// <summary>  </summary>
    public string OrganizationalUnitsKey { get; set; } = null!;
}
