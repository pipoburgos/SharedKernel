#if !NET40
using System.Security.Claims;
#endif

namespace SharedKernel.Application.Security;

/// <summary> </summary>
public class DefaultIdentityService : IIdentityService
{
    /// <summary> </summary>
    public Guid UserId => default;

    /// <summary> </summary>
    public IEnumerable<string> GetKeyValues(string header) => [];

    /// <summary> </summary>
    public string GetKeyValue(string header) => string.Empty;

    /// <summary> </summary>
    public void AddKeyValue(string key, string value) { }

#if !NET40
    /// <summary> </summary>
    public ClaimsPrincipal? User { get; set; }
#endif
}