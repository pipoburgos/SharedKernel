using System.Collections.Generic;
using SharedKernel.Domain.ValueObjects;

namespace SharedKernel.Domain.Tests.ValueObjects.CustomClasses;

public class House : ValueObject<House>
{
    public House(IEnumerable<User> users)
    {
        Users = users;
    }

    public IEnumerable<User> Users { get; private set; }
}