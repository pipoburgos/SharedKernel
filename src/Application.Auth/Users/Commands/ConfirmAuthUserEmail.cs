using SharedKernel.Application.Cqrs.Commands;

namespace SharedKernel.Application.Auth.Users.Commands;

/// <summary> . </summary>
public sealed class ConfirmAuthUserEmail : ICommandRequest
{
    /// <summary> . </summary>
    public ConfirmAuthUserEmail(Guid userId, string token)
    {
        UserId = userId;
        Token = token;
    }

    /// <summary> . </summary>
    public Guid UserId { get; }

    /// <summary> . </summary>
    public string Token { get; }
}