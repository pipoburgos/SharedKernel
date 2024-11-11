using SharedKernel.Application.Cqrs.Commands;
using System.Security.Claims;

namespace SharedKernel.Application.Auth.Users.Commands;

/// <summary> . </summary>
public sealed class AddUserClaim : ICommandRequest
{
    /// <summary> . </summary>
    public AddUserClaim(Guid id, Claim claim)
    {
        Id = id;
        Claim = claim;
    }

    /// <summary> . </summary>
    public Guid Id { get; }

    /// <summary> . </summary>
    public Claim Claim { get; }
}