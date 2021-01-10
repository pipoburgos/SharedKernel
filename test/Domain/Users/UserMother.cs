using Bogus;
using System;

namespace SharedKernel.Domain.Tests.Users
{
    internal static class UserMother
    {
        public static User Create(Guid id = default, string name = default)
        {
            var faker = new Faker();

            if(id == default)
                id = faker.Random.Uuid();

            if (name == default)
                name = faker.Random.Word();

            return User.Create(id, name);
        }
    }

    internal static class AddressMother
    {
        public static Address Create(string company = default, int number = default, string street = default, string city = default)
        {
            var faker = new Faker();

            if (company == default)
                company = faker.Company.CompanyName();

            if (number == default)
                number = faker.Random.Number(1100);

            if (street == default)
                street = faker.Address.StreetName();

            if (city == default)
                city = faker.Address.City();

            return new Address(company, number, street, city);
        }
    }

    internal static class EmailMother
    {
        public static string Create()
        {
            var faker = new Faker();

            return faker.Person.Email;
        }
    }
}
