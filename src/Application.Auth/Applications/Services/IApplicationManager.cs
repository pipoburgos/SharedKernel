namespace SharedKernel.Application.Auth.Applications.Services;

/// <summary> . </summary>
public interface IApplicationManager
{
    /// <summary> . </summary>
    Task CreateClientCredentialsWithPassword(string clientId, string clientName, string clientSecret, string scope,
        CancellationToken cancellationToken);
}
