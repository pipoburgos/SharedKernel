using SharedKernel.Domain.Events;

namespace SharedKernel.Application.Auth.Users.Events;

/// <summary> . </summary>
public sealed class UserLoggedIn : DomainEvent
{
    /// <summary> . </summary>
    public UserLoggedIn(Guid userId, Guid sessionControlId)
    {
        UserId = userId;
        SessionControlId = sessionControlId;
    }

    /// <summary> . </summary>
    public Guid UserId { get; }

    /// <summary> . </summary>
    public Guid SessionControlId { get; }

    /// <summary> . </summary>
    public override string GetEventName()
    {
        return "authentication.userLoggedIn";
    }

    /// <summary> . </summary>
    public override DomainEvent FromPrimitives(string aggregateId, Dictionary<string, string> body, string eventId,
        string occurredOn)
    {
        return new UserLoggedIn(Guid.Parse(body[nameof(UserId)]), Guid.Parse(body[nameof(SessionControlId)]));
    }
}