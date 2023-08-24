namespace BankAccounts.Domain.BankAccounts.Factories
{
    internal static class UserFactory
    {
        public static Result<User> CreateUser(Guid id, string name, string surname, DateTime dateOfBirth)
        {
            return new User(id, name, surname, dateOfBirth);
        }
    }
}
