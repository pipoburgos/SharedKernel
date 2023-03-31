using BankAccounts.Application.BankAccounts.Commands;
using BankAccounts.Domain.BankAccounts.Repository;

namespace BankAccounts.Infrastructure.BankAccounts.Commands.Validators
{
    internal class CreateBankAccountValidator : AbstractValidator<CreateBankAccount>
    {
        public CreateBankAccountValidator(IServiceProvider serviceProvider)
        {
            RuleFor(e => e.Id)
                .NotEmpty()
                .MustAsync(async (prop, c) => !await
                    serviceProvider.GetRequiredService<IBankAccountRepository>().AnyAsync(prop, c));

            RuleFor(e => e.Name)
                .NotEmpty()
                .MaximumLength(100);
        }
    }
}
