using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.JsonWebTokens;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;
using System.Security.Claims;

namespace SharedKernel.Infrastructure.EntityFrameworkCore.OpenIddict;

/// <summary> . </summary>
public abstract class ConnectTokenEndpoint
{
    /// <summary> . </summary>
    public static async Task<SignInHttpResult> Handle(
        HttpContext httpContext,
        IOpenIddictApplicationManager applicationManager,
        UserManager<IdentityUser<Guid>> userManager,
        CancellationToken cancellationToken,
        string[] resources)
    {
        var request = httpContext.GetOpenIddictServerRequest();

        if (request == null)
            throw new Exception("The OpenID Connect request cannot be retrieved.");

        if (request.IsRefreshTokenGrantType())
        {
            var principalRefresh = await httpContext
                .AuthenticateAsync(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);

            return TypedResults.SignIn(principalRefresh.Principal!,
                authenticationScheme: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        }


        if (!request.IsPasswordGrantType())
            throw new Exception("The specified grant type is not implemented.");

        var application = await applicationManager.FindByClientIdAsync(request.ClientId!, cancellationToken)
                              ?? throw new InvalidOperationException("The application details cannot be found in the database.");

        var result = await applicationManager
            .ValidateClientSecretAsync(application, request.ClientSecret ?? string.Empty, cancellationToken);

        if (!result)
            throw new Exception("Application not found.");

        ClaimsIdentity identity = new("Bearer",
            nameType: OpenIddictConstants.Claims.Subject, roleType: OpenIddictConstants.Claims.Role);

        var user = await userManager.FindByNameAsync(request.Username ?? string.Empty);
        if (user == null || !await userManager.CheckPasswordAsync(user, request.Password ?? string.Empty))
            throw new Exception("Las credenciales no son correctas.");

        identity.AddClaim(OpenIddictConstants.Claims.Subject, user.Id.ToString());
        identity.AddClaim(OpenIddictConstants.Claims.PreferredUsername, user.UserName ?? string.Empty);
        identity.AddClaim(OpenIddictConstants.Claims.Email, user.Email ?? string.Empty);

        identity.AddClaim(new Claim("amr", "pwd"));

        // =========================
        // 🧾 CLIENT / CONTEXT INFO
        // =========================
        identity.AddClaim(new Claim("client_id", request.ClientId ?? string.Empty));

        var userClaims = await userManager.GetClaimsAsync(user);

        foreach (var claim in userClaims)
        {
            identity.AddClaim(claim);
        }
        var sessionControlId = Guid.NewGuid();
        identity.AddClaim(new Claim(JwtRegisteredClaimNames.Sid, sessionControlId.ToString()));

        var roles = await userManager.GetRolesAsync(user);

        foreach (var role in roles)
        {
            identity.AddClaim(new Claim(OpenIddictConstants.Claims.Role, role));
        }

        var scopes = request.GetScopes();
        identity.SetScopes(scopes);

        // ValidateAudience
        identity.SetResources(resources);

        identity.SetDestinations(_ => [OpenIddictConstants.Destinations.AccessToken]);

        ClaimsPrincipal principal = new(identity);

        return TypedResults.SignIn(principal, authenticationScheme: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
    }
}
