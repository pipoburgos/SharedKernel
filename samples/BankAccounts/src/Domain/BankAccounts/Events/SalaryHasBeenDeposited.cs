namespace BankAccounts.Domain.BankAccounts.Events;

internal class SalaryHasBeenDeposited : DomainEvent
{
    public SalaryHasBeenDeposited(Guid movementId, string aggregateId, string? eventId = default,
        string? occurredOn = default) : base(aggregateId, eventId, occurredOn)
    {
        MovementId = movementId;
    }

    public Guid MovementId { get; }

    public override string GetEventName()
    {
        return "bankAccounts.bankAccounts.salaryHasBeenDeposited";
    }

    public override DomainEvent FromPrimitives(string aggregateId, Dictionary<string, string> body, string eventId,
        string occurredOn)
    {
        return new SalaryHasBeenDeposited(Guid.Parse(body[nameof(MovementId)]), aggregateId, eventId, occurredOn);
    }
}