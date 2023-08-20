using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Application.Validator;
using System.Globalization;
using System.Reflection;

namespace SharedKernel.Infrastructure.FluentValidation;

/// <summary>  </summary>
public static class ServiceCollectionExtensions
{
    /// <summary> Register all AbstractValidator from library. </summary>
    public static IServiceCollection AddFluentValidationValidators(this IServiceCollection services, Type type,
        ServiceLifetime serviceLifetime = ServiceLifetime.Transient, string cultureInfo = "en")
    {
        return services.AddFluentValidationValidators(type.Assembly, serviceLifetime, cultureInfo);
    }

    /// <summary> Register all AbstractValidator from library. </summary>
    public static IServiceCollection AddFluentValidationValidators(this IServiceCollection services, Assembly assembly,
        ServiceLifetime serviceLifetime = ServiceLifetime.Transient, string cultureInfo = "en")
    {
        ValidatorOptions.Global.LanguageManager.Culture = new CultureInfo(cultureInfo);
        ValidatorOptions.Global.DefaultRuleLevelCascadeMode = CascadeMode.Stop;

        services.AddValidatorsFromAssembly(typeof(PageOptionsValidator).Assembly, serviceLifetime,
            includeInternalTypes: true);

        return services
            .AddTransient(typeof(IEntityValidator<>), typeof(FluentValidator<>))
            .AddValidatorsFromAssembly(assembly, serviceLifetime, includeInternalTypes: true);
    }
}
