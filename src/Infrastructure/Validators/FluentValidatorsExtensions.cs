using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Infrastructure.System;
using System;
using System.Reflection;

namespace SharedKernel.Infrastructure.Validators
{
    /// <summary>
    /// 
    /// </summary>
    public static class FluentValidatorsExtensions
    {
        /// <summary>
        /// Register all AbstractValidator from library
        /// </summary>
        /// <param name="services"></param>
        /// <param name="assembly"></param>
        /// <returns></returns>
        public static IServiceCollection AddValidators(this IServiceCollection services, Assembly assembly)
        {
            return services.AddFromAssembly(assembly, typeof(IValidator<>), typeof(AbstractValidator<>));
        }

        /// <summary>
        /// Register all AbstractValidator from library
        /// </summary>
        /// <param name="services"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IServiceCollection AddValidators(this IServiceCollection services, Type type)
        {
            return services.AddFromAssembly(type.Assembly, typeof(IValidator<>), typeof(AbstractValidator<>));
        }
    }
}
