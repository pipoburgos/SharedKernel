using BankAccounts.Application.BankAccounts.Queries;
using BankAccounts.Domain.BankAccounts.Repository;

namespace BankAccounts.Infrastructure.BankAccounts.Queries.Validators;

internal class GetBankAccountBalanceValidator : AbstractValidator<GetBankAccountBalance>
{
    public GetBankAccountBalanceValidator(
        IBankAccountRepository bankAccountRepository)
    {
        RuleFor(e => e.BankAccountId)
            .NotEmpty()
            .BankAccountExists(bankAccountRepository);
    }
}