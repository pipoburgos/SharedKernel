using BankAccounts.Domain.BankAccounts;
using SharedKernel.Domain.RailwayOrientedProgramming;

namespace BankAccounts.Domain.Tests.Data;

internal static class UserTestFactory
{
    public static Result<User> Create(Guid? id = default, string? name = default, string? surname = default,
        DateTime? birthdate = default)
    {
        return User.Create(id ?? Guid.NewGuid(), name ?? "ABC", surname ?? "SUR",
            birthdate ?? new DateTime(1980, 2, 25));
    }
}
