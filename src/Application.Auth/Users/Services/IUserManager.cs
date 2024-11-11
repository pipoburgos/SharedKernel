using System.Security.Claims;

namespace SharedKernel.Application.Auth.Users.Services;

/// <summary> . </summary>
public interface IUserManager
{
    /// <summary> . </summary>
    Task CreateAsync(Guid id, string userName, string email, string password, IEnumerable<string> roles,
        IEnumerable<Claim> claims, bool emailConfirmed = false, CancellationToken cancellationToken = default);

    /// <summary> . </summary>
    Task<string> GenerateEmailConfirmationTokenAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary> . </summary>
    Task ConfirmEmailAsync(Guid id, string token, CancellationToken cancellationToken = default);

    /// <summary> . </summary>
    Task<List<Claim>> GetClaimsAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary> . </summary>
    Task AddClaimsAsync(Guid id, IEnumerable<Claim> claims, CancellationToken cancellationToken = default);

    /// <summary> . </summary>
    Task AddClaimAsync(Guid id, Claim claim, CancellationToken cancellationToken = default);

    /// <summary> . </summary>
    Task AddToRolesAsync(Guid id, IEnumerable<string> roles, CancellationToken cancellationToken = default);

    /// <summary> . </summary>
    Task RemoveClaimAsync(Guid id, Claim claim, CancellationToken cancellationToken = default);

    /// <summary> . </summary>
    Task RemoveAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary> . </summary>
    Task ReactivateAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary> . </summary>
    Task<string> GeneratePasswordResetTokenAsync(string email, CancellationToken cancellationToken = default);

    /// <summary> . </summary>
    Task ResetPasswordAsync(Guid id, string token, string newPassword, CancellationToken cancellationToken = default);

    /// <summary> . </summary>
    Task UpdateAsync(Guid id, string phoneNumber, CancellationToken cancellationToken = default);

    /// <summary> . </summary>
    Task<List<string>> GetRoles(Guid id, CancellationToken cancellationToken = default);
}