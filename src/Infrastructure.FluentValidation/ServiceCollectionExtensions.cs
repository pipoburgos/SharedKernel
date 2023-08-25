using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Application.Validator;
using SharedKernel.Domain;
using System.Globalization;
using System.Reflection;

namespace SharedKernel.Infrastructure.FluentValidation;

/// <summary>  </summary>
public static class ServiceCollectionExtensions
{
    /// <summary> Register all AbstractValidator from library. </summary>
    public static IServiceCollection AddFluentValidation(this IServiceCollection services, Type type,
        ServiceLifetime serviceLifetime = ServiceLifetime.Transient, string cultureInfo = "en",
        CascadeMode cascadeMode = CascadeMode.Stop)
    {
        return services.AddFluentValidation(type.Assembly, serviceLifetime, cultureInfo, cascadeMode);
    }

    /// <summary> Register all AbstractValidator from library. </summary>
    public static IServiceCollection AddFluentValidation(this IServiceCollection services, Assembly assembly,
        ServiceLifetime serviceLifetime = ServiceLifetime.Transient, string cultureInfo = "en",
        CascadeMode cascadeMode = CascadeMode.Stop)
    {
        ValidatorOptions.Global.LanguageManager.Culture = new CultureInfo(cultureInfo);
        ValidatorOptions.Global.DefaultRuleLevelCascadeMode = cascadeMode;

        return services
            .AddTransient(typeof(IEntityValidator<>), typeof(FluentValidator<>))
            .AddValidatorsFromAssembly(typeof(SharedKernelDomainAssembly).Assembly, serviceLifetime,
                includeInternalTypes: true)
            .AddValidatorsFromAssembly(assembly, serviceLifetime, includeInternalTypes: true);
    }
}
