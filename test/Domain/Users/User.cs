using SharedKernel.Domain.Aggregates;
using SharedKernel.Domain.Validators;

// ReSharper disable FieldCanBeMadeReadOnly.Local

namespace SharedKernel.Domain.Tests.Users;

public class User : AggregateRootAuditable<Guid>, IValidatableObject
{
    private List<string> _emails;
    private List<Address> _addresses;

    private User()
    {
        Name = default!;
        _emails = new List<string>();
        _addresses = new List<Address>();
    }

    private User(Guid id, string name, DateTime birthdate, User? parent) : this()
    {
        Id = id;
        Name = name;
        Birthdate = birthdate;
        _emails = new List<string>();
        _addresses = new List<Address>();
        Parent = parent;
    }

    public static User Create(Guid id, string name, DateTime birthdate, User? parent)
    {
        var user = new User(id, name, birthdate, parent);
        user.Record(new UserCreated(id, name, id.ToString()));
        return user;
    }

    public string Name { get; private set; }

    public int? NumberOfChildren { get; private set; }

    public DateTime Birthdate { get; private set; }

    public User? Parent { get; private set; }

    public IEnumerable<string> Emails => _emails;

    public IEnumerable<Address> Addresses => _addresses;

    public User ChangeName(string name)
    {
        Name = name;
        return this;
    }

    public User ChangeNumberOfChildren(int? numberOfChildren)
    {
        NumberOfChildren = numberOfChildren;
        return this;
    }

    public User AddEmail(string email)
    {
        _emails.Add(email);
        return this;
    }

    public User AddAddress(Address address)
    {
        _addresses.Add(address);
        return this;
    }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        return Enumerable.Empty<ValidationResult>();
    }
}
