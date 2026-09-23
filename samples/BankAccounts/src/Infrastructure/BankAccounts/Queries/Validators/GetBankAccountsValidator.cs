using BankAccounts.Application.BankAccounts.Queries;
using SharedKernel.Infrastructure.FluentValidation;

namespace BankAccounts.Infrastructure.BankAccounts.Queries.Validators;

internal sealed class GetBankAccountsValidator : AbstractValidator<GetBankAccounts>
{
    public GetBankAccountsValidator(
        PageOptionsValidator pageOptionsValidator)
    {
        RuleFor(e => e.PageOptions).SetValidator(pageOptionsValidator!);
    }
}