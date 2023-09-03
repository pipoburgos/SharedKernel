using Bogus;

namespace SharedKernel.Domain.Tests.Users
{
    internal class UserMother
    {
        public static User Create(Guid? id = default, string? name = default, DateTime? birthday = default,
            int? numberOfChildren = default)
        {
            var faker = new Faker();

            var user = User.Create(id ?? faker.Random.Uuid(), name ?? faker.Random.Word(),
                birthday ?? faker.Date.Past().ToUniversalTime());

            user.ChangeNumberOfChildren(numberOfChildren ?? faker.Person.Random.Number(1, 7));

            return user;
        }
    }

    internal class AddressMother
    {
        public static Address Create(string? company = default, int number = default, string? street = default,
            string? city = default)

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

    internal class EmailMother
    {
        public static string Create()
        {
            var faker = new Faker();

            return faker.Person.Email;
        }
    }
}
