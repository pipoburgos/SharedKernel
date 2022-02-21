using BankAccounts.Application.BankAccounts.Commands;
using BankAccounts.Domain.BankAccounts.Repository;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace BankAccounts.Infrastructure.BankAccounts.Commands.Validators
{
    // FluentValidation
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
