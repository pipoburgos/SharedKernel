using System;
using System.Security.Claims;
using SharedKernel.Domain.Security;

namespace SharedKernel.Domain.Tests
{
    internal class IdentityServiceMock : IIdentityService
    {
        public Guid UserId => Guid.NewGuid();
        public ClaimsPrincipal User => null;
        public bool IsInRole(string role)
        {
            return true;
        }

        public bool IsAuthenticated()
        {
            return true;
        }

        public string BasePath => "http:a.com";
        public string UserAgent => "browser";
        public string RemoteIpAddress => "127.0.0.1";
    }
}
