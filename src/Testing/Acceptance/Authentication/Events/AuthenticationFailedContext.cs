using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace SharedKernel.Testing.Acceptance.Authentication.Events;

/// <summary> AuthenticationFailedContext. </summary>
public class AuthenticationFailedContext : ResultContext<FakeJwtBearerOptions>
{
    /// <summary> AuthenticationFailedContext. </summary>
    public AuthenticationFailedContext(
        HttpContext context,
        AuthenticationScheme scheme,
        FakeJwtBearerOptions options)
        : base(context, scheme, options) { }

    /// <summary> Exception. </summary>
    public Exception Exception { get; set; } = null!;
}
