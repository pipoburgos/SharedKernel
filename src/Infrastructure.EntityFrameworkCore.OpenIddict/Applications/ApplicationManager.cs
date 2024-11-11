using OpenIddict.Abstractions;
using SharedKernel.Application.Auth.Applications.Services;

namespace SharedKernel.Infrastructure.EntityFrameworkCore.OpenIddict.Applications;

/// <summary> . </summary>
public class ApplicationManager : IApplicationManager
{
    private readonly IOpenIddictApplicationManager _openIddictApplicationManager;

    /// <summary> . </summary>
    public ApplicationManager(IOpenIddictApplicationManager openIddictApplicationManager)
    {
        _openIddictApplicationManager = openIddictApplicationManager;
    }

    /// <summary> . </summary>
    public Task CreateClientCredentialsWithPassword(string clientId, string clientName, string clientSecret,
        string scope, CancellationToken cancellationToken)
    {
        var descriptor = new OpenIddictApplicationDescriptor
        {
            ClientId = clientId,
            DisplayName = clientName,
            ClientSecret = clientSecret,
            Permissions =
            {
                OpenIddictConstants.Permissions.Endpoints.Token,
                OpenIddictConstants.Permissions.GrantTypes.ClientCredentials,
                OpenIddictConstants.Permissions.GrantTypes.Password,
                OpenIddictConstants.Permissions.GrantTypes.RefreshToken,
                OpenIddictConstants.Permissions.Prefixes.Scope + scope,
                OpenIddictConstants.Permissions.ResponseTypes.Code,
                OpenIddictConstants.Scopes.OfflineAccess,
                OpenIddictConstants.Scopes.Profile,
                OpenIddictConstants.Scopes.OpenId,
                scope,
            },
        };

        return _openIddictApplicationManager.CreateAsync(descriptor, cancellationToken).AsTask();
    }
}
