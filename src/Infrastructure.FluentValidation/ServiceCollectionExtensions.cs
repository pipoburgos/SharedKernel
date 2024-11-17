using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Application.Validator;
using SharedKernel.Domain;
using System.Reflection;

namespace SharedKernel.Infrastructure.FluentValidation;

/// <summary> . </summary>
public static class ServiceCollectionExtensions
{
    /// <summary> Register all AbstractValidator from library. </summary>
    public static IServiceCollection AddSharedKernelFluentValidation(this IServiceCollection services, Type type,
        ServiceLifetime serviceLifetime = ServiceLifetime.Transient, CascadeMode cascadeMode = CascadeMode.Stop)
    {
        return services.AddSharedKernelFluentValidation(type.Assembly, serviceLifetime, cascadeMode);
    }

    /// <summary> Register all AbstractValidator from library. </summary>
    public static IServiceCollection AddSharedKernelFluentValidation(this IServiceCollection services, Assembly assembly,
        ServiceLifetime serviceLifetime = ServiceLifetime.Transient, CascadeMode cascadeMode = CascadeMode.Stop)
    {
        ValidatorOptions.Global.DefaultRuleLevelCascadeMode = cascadeMode;

        return services
            .AddTransient(typeof(IClassValidator<>), typeof(FluentValidator<>))
            .AddValidatorsFromAssembly(typeof(SharedKernelDomainAssembly).Assembly, serviceLifetime,
                includeInternalTypes: true)
            .AddValidatorsFromAssembly(assembly, serviceLifetime, includeInternalTypes: true);
    }
}
