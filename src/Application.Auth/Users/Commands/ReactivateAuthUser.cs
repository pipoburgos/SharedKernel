using SharedKernel.Application.Cqrs.Commands;
using System.Security.Claims;

namespace SharedKernel.Application.Auth.Users.Commands;

/// <summary> . </summary>
public sealed record ReactivateAuthUser : ICommandRequest
{
    public ReactivateAuthUser(Guid id, IEnumerable<Claim> claims, IEnumerable<string> roles)
    {
        Id = id;
        Claims = claims;
        Roles = roles;
    }

    /// <summary> . </summary>
    public Guid Id { get; }

    /// <summary> . </summary>
    public IEnumerable<Claim> Claims { get; }

    /// <summary> . </summary>
    public IEnumerable<string> Roles { get; }
}
