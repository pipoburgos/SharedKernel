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

        ///// <summary>
        ///// No funciona con "https://localhost:44331/Account/ResetPassword"
        ///// TODO PENDIENTE DE TESTING
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="ruleBuilder"></param>
        ///// <returns></returns>
        //public static IRuleBuilderOptions<T, string> Url<T>(this IRuleBuilder<T, string> ruleBuilder)
        //{
        //    return ruleBuilder.Matches("(http:\\/\\/|https:\\/\\/)?[a-z0-9]+([\\-\\.]{1}[a-z0-9]+)*\\.[a-z]{2,5}(:[0-9]{1,5})?(\\/.*)?");
        //}

        public static IRuleBuilderOptions<T, string> PhoneNumber<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder.Matches(@"^[0-9]{9}$");
        }
    }
}
