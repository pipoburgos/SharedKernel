using BankAccounts.Application.BankAccounts.Queries;
using BankAccounts.Domain.BankAccounts.Repository;

namespace BankAccounts.Infrastructure.BankAccounts.Validators.Queries;

internal sealed class GetBankAccountBalanceValidator : AbstractValidator<GetBankAccountBalance>
{
    public GetBankAccountBalanceValidator(
        IBankAccountRepository bankAccountRepository)
    {
        RuleFor(e => e.BankAccountId)
            .NotEmpty()
            .BankAccountExists(bankAccountRepository);
    }
}