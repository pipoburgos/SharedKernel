using Bogus;
using SharedKernel.Domain.RailwayOrientedProgramming;

namespace SharedKernel.Domain.Tests.BankAccounts;

public class BankAccountMother
{
    internal static Result<BankAccount> Create(Guid? id = default, decimal? amount = default, DateTime? date = default)
    {
        var faker = new Faker();

        return BankAccount.Create(id ?? Guid.NewGuid(), amount ?? faker.Finance.Amount(),
            date ?? faker.Date.Past().ToUniversalTime());
    }
}
