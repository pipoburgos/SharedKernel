using SharedKernel.Domain.ValueObjects;

namespace SharedKernel.Domain.Tests.Users
{
    public class Address : ValueObject<Address>
    {
        public Address(string company, int number, string street, string city)
        {
            Company = company;
            Number = number;
            Street = street;
            City = city;
        }

        public string Company { get;  }
        public int Number { get;  }
        public string Street { get;  }
        public string City { get;  }
    }
}
