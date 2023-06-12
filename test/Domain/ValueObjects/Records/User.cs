using SharedKernel.Domain.Entities;
using System.Collections.Generic;

namespace SharedKernel.Domain.Tests.ValueObjects.Records;

public class User : Entity<int>
{
    public User(int id, IEnumerable<Address> addresses)
    {
        Id = id;
        Addresses = addresses;
    }

    public IEnumerable<Address> Addresses { get; private set; }
}