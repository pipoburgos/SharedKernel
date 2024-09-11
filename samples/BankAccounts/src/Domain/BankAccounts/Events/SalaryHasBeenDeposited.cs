namespace BankAccounts.Domain.BankAccounts.Events;

/// <summary> . </summary>
public sealed class SalaryHasBeenDeposited : DomainEvent
{
    /// <summary> . </summary>
    public SalaryHasBeenDeposited(Guid movementId, string aggregateId, string? eventId = default,
        string? occurredOn = default) : base(aggregateId, eventId, occurredOn)
    {
        MovementId = movementId;
    }

    /// <summary> . </summary>
    public Guid MovementId { get; }

    /// <summary> . </summary>
    public override string GetEventName()
    {
        return "bankAccounts.bankAccounts.salaryHasBeenDeposited";
    }

    /// <summary> . </summary>
    public override DomainEvent FromPrimitives(string aggregateId, Dictionary<string, string> body, string eventId,
        string occurredOn)
    {
        return new SalaryHasBeenDeposited(Guid.Parse(body[nameof(MovementId)]), aggregateId, eventId, occurredOn);
    }
}