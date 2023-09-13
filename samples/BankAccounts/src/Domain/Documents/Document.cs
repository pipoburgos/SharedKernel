namespace BankAccounts.Domain.Documents;

internal class Document : AggregateRoot<Guid>
{
    public Document(Guid id, string text) : base(id)
    {
        Text = text;
    }

    public static Document Create(Guid id, string text)
    {
        return new Document(id, text);
    }

    public string Text { get; private set; }
}
