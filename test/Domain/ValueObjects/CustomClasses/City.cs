using System.Collections.Generic;
using SharedKernel.Domain.ValueObjects;

namespace SharedKernel.Domain.Tests.ValueObjects.CustomClasses;

public class City : ValueObject<City>
{
    public City(IEnumerable<Address> addresses)
    {
        Addresses = addresses;
    }

    public IEnumerable<Address> Addresses { get; private set; }
}