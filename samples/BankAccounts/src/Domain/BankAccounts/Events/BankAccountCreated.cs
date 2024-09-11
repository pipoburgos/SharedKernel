namespace BankAccounts.Domain.BankAccounts.Events;

/// <summary> . </summary>
public sealed class BankAccountCreated : DomainEvent
{
    /// <summary> . </summary>
    public BankAccountCreated(string aggregateId, string? eventId = default, string? occurredOn = default)
        : base(aggregateId, eventId, occurredOn)
    { }

    /// <summary> . </summary>
    public override string GetEventName()
    {
        return "bankAccounts.bankAccounts.bankAccountCreated";
    }

    /// <summary> . </summary>
    public override DomainEvent FromPrimitives(string aggregateId, Dictionary<string, string> body, string eventId,
        string occurredOn)
    {
        return new BankAccountCreated(aggregateId, eventId, occurredOn);
    }
}