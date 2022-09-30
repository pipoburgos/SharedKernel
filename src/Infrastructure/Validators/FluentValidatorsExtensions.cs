using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
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
        /// <param name="serviceLifetime"></param>
        /// <param name="cultureInfo"></param>
        /// <returns></returns>
        public static IServiceCollection AddValidators(this IServiceCollection services, Type type,
            ServiceLifetime serviceLifetime = ServiceLifetime.Transient, string cultureInfo = "en")
        {
            return services.AddValidators(type.Assembly, serviceLifetime, cultureInfo);
        }

        /// <summary>
        /// Register all AbstractValidator from library
        /// </summary>
        /// <param name="services"></param>
        /// <param name="assembly"></param>
        /// <param name="serviceLifetime"></param>
        /// <param name="cultureInfo"></param>
        /// <returns></returns>
        public static IServiceCollection AddValidators(this IServiceCollection services, Assembly assembly,
            ServiceLifetime serviceLifetime = ServiceLifetime.Transient, string cultureInfo = "en")
        {
            ValidatorOptions.Global.LanguageManager.Culture = new CultureInfo(cultureInfo);
            ValidatorOptions.Global.DefaultRuleLevelCascadeMode = CascadeMode.Stop;

            services.AddValidatorsFromAssembly(typeof(PageOptionsValidator).Assembly, serviceLifetime,
                includeInternalTypes: true);

            return services.AddValidatorsFromAssembly(assembly, serviceLifetime, includeInternalTypes: true);
        }
    }
}
