using BankAccounts.Domain.BankAccounts;
using BankAccounts.Domain.BankAccounts.Repository;

namespace BankAccounts.Infrastructure.BankAccounts;

internal static class BankAccountValidators
{
    public static IRuleBuilderOptions<T, Guid> BankAccountExists<T>(this IRuleBuilder<T, Guid> ruleBuilder,
        IBankAccountRepository bankAccountRepository)
    {
        return ruleBuilder
            .MustAsync(async (id, ct) => await bankAccountRepository.AnyAsync(BankAccountId.Create(id), ct))
            .WithMessage("Bank account not found.");
    }
}