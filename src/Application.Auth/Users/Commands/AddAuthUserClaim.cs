using SharedKernel.Application.Cqrs.Commands;
using System.Security.Claims;

namespace SharedKernel.Application.Auth.Users.Commands;

/// <summary> . </summary>
public sealed class AddAuthUserClaim : ICommandRequest
{
    /// <summary> . </summary>
    public AddAuthUserClaim(Guid id, Claim claim)
    {
        Id = id;
        Claim = claim;
    }

    /// <summary> . </summary>
    public Guid Id { get; }

    /// <summary> . </summary>
    public Claim Claim { get; }
}