using System;
using FluentValidation;

namespace SharedKernel.Infrastructure.Validators
{
    /// <summary>
    /// 
    /// </summary>
    public static class DefaultValidatorExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ruleBuilder"></param>
        /// <returns></returns>
        public static IRuleBuilderOptions<T, string> Uri<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder.Must(uri => global::System.Uri.TryCreate(uri, UriKind.Absolute, out _));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ruleBuilder"></param>
        /// <returns></returns>
        public static IRuleBuilderOptions<T, string> PhoneNumber<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder.Matches(@"^[0-9]{9}$");
        }
    }
}
