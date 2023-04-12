using SharedKernel.Application.Security;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace SharedKernel.Application.Tests.Security
{
    internal class IdentityServiceMock : IIdentityService
    {
        public Guid UserId => Guid.NewGuid();
        public ClaimsPrincipal User { get; set; }
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
        public Dictionary<string, IEnumerable<string>> Headers => new Dictionary<string, IEnumerable<string>>();
    }
}
