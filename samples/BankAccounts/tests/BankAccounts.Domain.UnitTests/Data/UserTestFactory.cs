using BankAccounts.Domain.BankAccounts;

namespace BankAccounts.Domain.Tests.Data
{
    internal static class UserTestFactory
    {
        public static User Create(Guid? id = default, string name = default, string surname = default,
            DateTime? birthdate = default)
        {
            return new User(id ?? Guid.NewGuid(), name ?? "ABC", surname ?? "SUR",
                birthdate ?? new DateTime(1980, 2, 25));
        }
    }
}
