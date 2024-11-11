namespace SharedKernel.Application.Auth.Roles.Services;

/// <summary> . </summary>
public interface IRoleManager
{
    /// <summary> . </summary>
    Task<bool> Exists(string role, CancellationToken cancellationToken = default);

    /// <summary> . </summary>
    Task<bool> Create(Guid id, string role, CancellationToken cancellationToken = default);
}