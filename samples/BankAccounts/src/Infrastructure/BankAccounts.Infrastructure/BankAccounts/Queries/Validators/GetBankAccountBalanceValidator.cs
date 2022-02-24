using BankAccounts.Application.BankAccounts.Queries;
using FluentValidation;

namespace BankAccounts.Infrastructure.BankAccounts.Queries.Validators
{
    internal class GetBankAccountBalanceValidator : AbstractValidator<GetBankAccountBalance>
    {
        public GetBankAccountBalanceValidator()
        {
            RuleFor(e => e.BankAccountId)
                .NotEmpty();
        }
    }
}
