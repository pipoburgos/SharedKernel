using BankAccounts.Application.BankAccounts.Queries;

namespace BankAccounts.Infrastructure.BankAccounts.Validators.Queries;

internal sealed class BankAccountItemValidator : AbstractValidator<BankAccountItem>
{
    public BankAccountItemValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
