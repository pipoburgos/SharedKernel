using BankAccounts.Domain.BankAccounts.Repository;

namespace BankAccounts.Infrastructure.BankAccounts
{
    internal static class BankAccountValidators
    {
        public static IRuleBuilderOptions<T, Guid> BankAccountExists<T>(this IRuleBuilder<T, Guid> ruleBuilder,
            IBankAccountRepository bankAccountRepository)
        {
            return ruleBuilder
                .MustAsync(bankAccountRepository.AnyAsync)
                .WithMessage("Bank account not found.");
        }
    }
}
