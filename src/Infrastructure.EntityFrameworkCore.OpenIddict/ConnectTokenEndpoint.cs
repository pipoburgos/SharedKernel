using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;
using System.Collections.Immutable;
using System.Security.Claims;

namespace SharedKernel.Infrastructure.EntityFrameworkCore.OpenIddict;

/// <summary> . </summary>
public abstract class ConnectTokenEndpoint : ControllerBase
{
    private readonly IOpenIddictApplicationManager _applicationManager;
    private readonly UserManager<IdentityUser<Guid>> _userManager;

    /// <summary> . </summary>
    protected ConnectTokenEndpoint(IOpenIddictApplicationManager applicationManager, UserManager<IdentityUser<Guid>> userManager)
    {
        _applicationManager = applicationManager;
        _userManager = userManager;
    }

    /// <summary> . </summary>
    protected async Task<IResult> Handle(string scope, List<string> roles, CancellationToken cancellationToken)
    {
        var request = HttpContext.GetOpenIddictServerRequest() ??
                      throw new UnauthorizedAccessException("The OpenID Connect request cannot be retrieved.");

        if (request.IsRefreshTokenGrantType())
        {
            var principalRefresh =
                await HttpContext.AuthenticateAsync(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);

            return Results.SignIn(principalRefresh.Principal!,
                authenticationScheme: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        }


        if (!request.IsPasswordGrantType())
            throw new UnauthorizedAccessException("The specified grant type is not implemented.");

        var application = await _applicationManager.FindByClientIdAsync(request.ClientId!, cancellationToken) ??
                          throw new InvalidOperationException(
                              "The application details cannot be found in the database.");

        var result = await _applicationManager
            .ValidateClientSecretAsync(application, request.ClientSecret ?? string.Empty, cancellationToken);

        if (!result)
            throw new UnauthorizedAccessException("Application not found.");

        ClaimsIdentity identity = new("Bearer", nameType: ClaimTypes.Sid, roleType: OpenIddictConstants.Claims.Role);

        var user = await _userManager.FindByNameAsync(request.Username ?? string.Empty);
        if (user == null || !await _userManager.CheckPasswordAsync(user, request.Password ?? string.Empty))
            throw new UnauthorizedAccessException("The credentials are not correct.");

        foreach (var role in roles)
        {
            identity.SetClaim(OpenIddictConstants.Claims.Role, role);
        }

        identity.SetClaim(OpenIddictConstants.Claims.Subject, request.ClientId);
        identity.SetClaim(ClaimTypes.Sid, user.Id.ToString());
        var scopes = request.GetScopes();
        identity.SetScopes(scopes);

        // ValidateAudience
        identity.SetResources((ImmutableArray<string>)[scope]);

        identity.SetDestinations(_ => [OpenIddictConstants.Destinations.AccessToken]);

        ClaimsPrincipal principal = new(identity);

        return Results.SignIn(principal, authenticationScheme: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
    }
}
