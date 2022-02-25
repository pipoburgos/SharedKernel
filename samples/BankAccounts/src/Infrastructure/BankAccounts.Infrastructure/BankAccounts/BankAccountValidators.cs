using BankAccounts.Domain.BankAccounts.Repository;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace BankAccounts.Infrastructure.BankAccounts
{
    internal static class BankAccountValidators
    {
        public static IRuleBuilderOptions<T, Guid> BankAccountExists<T>(this IRuleBuilder<T, Guid> ruleBuilder, IServiceProvider serviceProvider)
        {
            return ruleBuilder
                .MustAsync(serviceProvider.GetRequiredService<IBankAccountRepository>().AnyAsync)
                .WithMessage("Bank account not found.");
        }
    }
}
