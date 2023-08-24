using SharedKernel.Domain.Entities;

namespace SharedKernel.Domain.Tests.ValueObjects.CustomClasses;

public class User : Entity<int>
{
    public User(int id, IEnumerable<Address> addresses)
    {
        Id = id;
        Addresses = addresses;
    }

    public IEnumerable<Address> Addresses { get; private set; }
}