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
        /// <summary>
        /// User logged identifier
        /// </summary>
        Guid UserId { get; }

#if !NET40
        ClaimsPrincipal User { get; }
#endif

        /// <summary>
        /// User logged is in role
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        bool IsInRole(string role);

        /// <summary>
        /// If user is logged in
        /// </summary>
        /// <returns></returns>
        bool IsAuthenticated();

        /// <summary>
        /// Site base url
        /// </summary>
        string BasePath { get; }

        /// <summary>
        /// Request user agent
        ///  http://www.useragentstring.com/
        /// </summary>
        string UserAgent { get; }

        /// <summary>
        /// Request user ip address
        /// </summary>
        string RemoteIpAddress { get; }
    }
}
