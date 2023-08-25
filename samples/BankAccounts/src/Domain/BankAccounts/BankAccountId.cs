namespace BankAccounts.Domain.BankAccounts;
internal class BankAccountId : ValueObject<BankAccountId>
{
    protected BankAccountId() { }

    protected BankAccountId(Guid value) : this()
    {
        Value = value;
    }

    public static BankAccountId Create(Guid value)
    {
        return new BankAccountId(value);
    }

    public Guid Value { get; private set; }
}