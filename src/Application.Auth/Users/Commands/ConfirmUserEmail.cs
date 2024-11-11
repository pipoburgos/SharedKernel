using SharedKernel.Application.Cqrs.Commands;

namespace SharedKernel.Application.Auth.Users.Commands;

/// <summary> . </summary>
public sealed class ConfirmUserEmail : ICommandRequest
{
    /// <summary> . </summary>
    public ConfirmUserEmail(Guid userId, string token)
    {
        UserId = userId;
        Token = token;
    }

    /// <summary> . </summary>
    public Guid UserId { get; }

    /// <summary> . </summary>
    public string Token { get; }
}