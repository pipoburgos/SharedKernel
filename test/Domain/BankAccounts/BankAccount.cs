using SharedKernel.Domain.Aggregates;
using SharedKernel.Domain.RailwayOrientedProgramming;

// ReSharper disable FieldCanBeMadeReadOnly.Local

namespace SharedKernel.Domain.Tests.BankAccounts;

public class BankAccount : AggregateRootAuditableLogicalRemove<Guid>
{
    protected BankAccount() { }

    protected BankAccount(Guid id, decimal amount, DateTime date) : base(id)
    {
        Amount = amount;
        Date = date;
    }

    public static Result<BankAccount> Create(Guid id, decimal amount, DateTime date)
    {
        return new BankAccount(id, amount, date);
    }

    public decimal Amount { get; private set; }

    public DateTime Date { get; private set; }
}