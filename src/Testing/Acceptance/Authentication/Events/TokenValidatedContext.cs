using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace SharedKernel.Testing.Acceptance.Authentication.Events
{
    /// <summary>
    /// TokenValidatedContext
    /// </summary>
    public class TokenValidatedContext : ResultContext<FakeJwtBearerOptions>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="scheme"></param>
        /// <param name="options"></param>
        public TokenValidatedContext(
            HttpContext context,
            AuthenticationScheme scheme,
            FakeJwtBearerOptions options)
            : base(context, scheme, options) { }

        //public SecurityToken SecurityToken { get; private set; }
    }
}
