namespace BankAccounts.Domain.BankAccounts;

internal class Movement : Entity<Guid>
{
    protected Movement() { }

    protected Movement(Guid id, string concept, decimal amount, DateTime date) : base(id)
    {
        Concept = concept;
        Amount = amount;
        Date = date;
    }

    public static Result<Movement> Create(Guid id, string concept, decimal amount, DateTime date)
    {
        return new Movement(id, concept, amount, date);
    }

    public string Concept { get; private set; } = null!;

    public decimal Amount { get; private set; }

    public DateTime Date { get; private set; }

    public BankAccountId BankAccountId { get; private set; } = null!;
}
