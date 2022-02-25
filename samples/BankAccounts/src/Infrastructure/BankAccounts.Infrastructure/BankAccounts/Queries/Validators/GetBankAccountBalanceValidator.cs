using BankAccounts.Application.BankAccounts.Queries;
using FluentValidation;
using System;

namespace BankAccounts.Infrastructure.BankAccounts.Queries.Validators
{
    internal class GetBankAccountBalanceValidator : AbstractValidator<GetBankAccountBalance>
    {
        public GetBankAccountBalanceValidator(IServiceProvider serviceProvider)
        {
            RuleFor(e => e.BankAccountId)
                .NotEmpty()
                .BankAccountExists(serviceProvider);
        }
    }
}
