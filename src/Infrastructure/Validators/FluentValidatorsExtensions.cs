using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Infrastructure.System;
using System;
using System.Globalization;
using System.Reflection;

namespace SharedKernel.Infrastructure.Validators
{
    /// <summary>  </summary>
    public static class FluentValidatorsExtensions
    {
        /// <summary>
        /// Register all AbstractValidator from library
        /// </summary>
        /// <param name="services"></param>
        /// <param name="type"></param>
        /// <param name="cultureInfo"></param>
        /// <returns></returns>
        public static IServiceCollection AddValidators(this IServiceCollection services, Type type, string cultureInfo = "en")
        {
            return services.AddValidators(type.Assembly, cultureInfo);
        }

        /// <summary>
        /// Register all AbstractValidator from library
        /// </summary>
        /// <param name="services"></param>
        /// <param name="assembly"></param>
        /// <param name="cultureInfo"></param>
        /// <returns></returns>
        public static IServiceCollection AddValidators(this IServiceCollection services, Assembly assembly, string cultureInfo = "en")
        {
            ValidatorOptions.Global.LanguageManager.Culture = new CultureInfo(cultureInfo);
            ValidatorOptions.Global.DefaultRuleLevelCascadeMode = CascadeMode.Stop;
            return services.AddFromAssembly(assembly, typeof(IValidator<>), typeof(AbstractValidator<>));
        }
    }
}
