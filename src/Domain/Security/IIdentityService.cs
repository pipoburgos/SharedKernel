using System;
#if !NET40
using System.Security.Claims;
#endif

namespace SharedKernel.Domain.Security
{
    /// <summary>
    /// From: https://github.com/dotnet-architecture/eShopOnContainers/blob/dev/src/Services/Ordering/Ordering.API/Infrastructure/Services/IIdentityService.cs
    /// </summary>
    public interface IIdentityService
    {
        Guid UserId { get; }

#if !NET40
        ClaimsPrincipal User { get; }
#endif


        bool IsInRole(string role);

        bool IsAuthenticated();

        string BasePath { get; }

        /// <summary>
        ///  http://www.useragentstring.com/
        /// </summary>
        string UserAgent { get; }

        string RemoteIpAddress { get; }
    }
}
