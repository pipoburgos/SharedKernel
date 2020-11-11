using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using SharedKernel.Application.Logging;
using SharedKernel.Domain.Security;

namespace SharedKernel.Infrastructure.Security
{
    /// <summary>
    /// Autenticación del usuario
    /// </summary>
    public class HttpContextAccessorIdentityService : IIdentityService
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="httpContextAccessor"></param>
        /// <param name="logger"></param>
        public HttpContextAccessorIdentityService(
            IHttpContextAccessor httpContextAccessor,
            ICustomLogger<HttpContextAccessorIdentityService> logger)
        {
            if (httpContextAccessor == null)
            {
                logger.Info("IHttpContextAccessor not inyected");
                return;
            }

            User = httpContextAccessor.HttpContext?.User;

            var request = httpContextAccessor.HttpContext?.Request;

            if (request != default)
            {
                BasePath = $"{request.Scheme}://{request.Host}{request.PathBase}";

                UserAgent = request.Headers["User-Agent"].ToString();
            }

            RemoteIpAddress = httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString();
        }

        /// <summary>
        /// Identificador
        /// </summary>
        public Guid UserId => GetUserId();

        /// <summary>
        /// Usuario
        /// </summary>
        public ClaimsPrincipal User { get; }

        /// <summary>
        /// Contiene un rol
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        public bool IsInRole(string role)
        {
            return User.IsInRole(role);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool IsAuthenticated()
        {
            return User?.Identity?.IsAuthenticated == true;
        }

        /// <summary>
        /// 
        /// </summary>
        public string BasePath { get; }

        /// <summary>
        /// Navegador
        /// </summary>
        public string UserAgent { get; }

        /// <summary>
        /// Dirección ip remota
        /// </summary>
        public string RemoteIpAddress { get; }


        private Guid GetUserId()
        {
            var id = User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return string.IsNullOrWhiteSpace(id) ? Guid.Empty : new Guid(id);
        }
    }
}
