using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using SharedKernel.Application.Security;

namespace SharedKernel.Integration.Tests.Events
{
    /// <summary> User authentication. </summary>
    public class HttpContextAccessorIdentityService : IIdentityService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        /// <summary> Constructor. </summary>
        /// <param name="httpContextAccessor"></param>
        public HttpContextAccessorIdentityService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary> Identifier. </summary>
        public Guid UserId => GetUserId();

        /// <summary>  </summary>
        public IEnumerable<string> GetKeyValues(string header)
        {
            var values = new StringValues();
            var ok = _httpContextAccessor?.HttpContext?.Request.Headers.TryGetValue(header, out values);
            return ok == true ? values : Enumerable.Empty<string>();
        }

        /// <summary>  </summary>
        public string GetKeyValue(string header)
        {
            var values = new StringValues();
            var ok = _httpContextAccessor?.HttpContext?.Request.Headers.TryGetValue(header, out values);
            return ok == true ? values : string.Empty;
        }

        /// <summary>  </summary>
        public void AddKeyValue(string key, string value)
        {
            if (_httpContextAccessor?.HttpContext?.Request.Headers.ContainsKey(key) == true)
                return;

            _httpContextAccessor?.HttpContext?.Request.Headers.Add(key, value);
        }

        /// <summary> User. </summary>
        public ClaimsPrincipal User
        {
            get => _httpContextAccessor?.HttpContext?.User;
            set
            {
                _httpContextAccessor.HttpContext ??= new DefaultHttpContext();
                _httpContextAccessor.HttpContext.User = value;
            }
        }

        /// <summary> Contains a role. </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        public bool IsInRole(string role)
        {
            return User.IsInRole(role);
        }

        /// <summary> </summary>
        /// <returns></returns>
        public virtual bool IsAuthenticated()
        {
            return User?.Identity?.IsAuthenticated == true;
        }

        /// <summary> </summary>
        public string BasePath => _httpContextAccessor.HttpContext?.Request == default
            ? default
            : $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}{_httpContextAccessor.HttpContext.Request.PathBase}";

        /// <summary> Browser. </summary>
        public string UserAgent => _httpContextAccessor.HttpContext?.Request.Headers["User-Agent"].ToString();

        /// <summary> Remote ip address </summary>
        public string RemoteIpAddress => _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString();

        /// <summary> Http headers. </summary>
        public Dictionary<string, IEnumerable<string>> Headers =>
            _httpContextAccessor?.HttpContext?.Request.Headers.ToDictionary(h => h.Key,
                a => a.Value.ToArray().AsEnumerable()) ?? new Dictionary<string, IEnumerable<string>>();

        /// <summary> Get user id from <see cref="ClaimTypes.Sid"/>. </summary>
        /// <returns></returns>
        protected virtual Guid GetUserId()
        {
            var id = User?.FindFirst(ClaimTypes.Sid)?.Value;

            return !string.IsNullOrWhiteSpace(id) && Guid.TryParse(id, out _) ? new Guid(id) : Guid.Empty;
        }
    }
}
