using System;
using FluentValidation;

namespace SharedKernel.Infrastructure.Validators
{
    public static class DefaultValidatorExtensions
    {
        public static IRuleBuilderOptions<T, string> Uri<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder.Must(uri => global::System.Uri.TryCreate(uri, UriKind.Absolute, out _));
        }

        public static IRuleBuilderOptions<T, string> PhoneNumber<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder.Matches(@"^[0-9]{9}$");
        }
    }
}
