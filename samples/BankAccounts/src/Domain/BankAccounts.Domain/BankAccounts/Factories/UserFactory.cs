using System;

namespace BankAccounts.Domain.BankAccounts.Factories
{
    public static class UserFactory
    {
        public static User CreateUser(Guid id, string name, string surname, DateTime dateOfBirth)
        {
            return new User(id, name, surname, dateOfBirth);
        }
    }
}
