namespace BankAccounts.Domain.BankAccounts;

internal class User : Entity<Guid>
{
    private readonly List<Email> _emails;

    protected User()
    {
        Name = default!;
        Surname = default;
        Birthdate = default;
        _emails = new List<Email>();
    }

    protected User(Guid id, string name, string? surname, DateTime birthdate) : base(id)
    {
        Name = name;
        Surname = surname;
        Birthdate = birthdate;
        _emails = new List<Email>();
    }

    public static Result<User> Create(Guid id, string name, string? surname, DateTime dateOfBirth)
    {
        return new User(id, name, surname, dateOfBirth);
    }

    public string Name { get; private set; }

    public string? Surname { get; private set; }

    public DateTime Birthdate { get; private set; }

    public IEnumerable<Email> Emails => _emails.AsEnumerable();
}