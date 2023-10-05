#if !NET40
using System.Security.Claims;
#endif

namespace SharedKernel.Application.Security;

/// <summary>
/// From: https://github.com/dotnet-architecture/eShopOnContainers/blob/dev/src/Services/Ordering/Ordering.API/Infrastructure/Services/IIdentityService.cs
/// </summary>
public interface IIdentityService
{
    /// <summary> User logged identifier. </summary>
    Guid UserId { get; }

    /// <summary>  </summary>
    IEnumerable<string> GetKeyValues(string header);

    /// <summary>  </summary>
    string GetKeyValue(string header);

    /// <summary>  </summary>
    void AddKeyValue(string key, string value);

#if !NET40
    /// <summary> Users claims. </summary>
    ClaimsPrincipal? User { get; set; }
#endif
}
