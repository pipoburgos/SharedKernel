using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Application.Security;

#pragma warning disable SA1300
namespace SharedKernelInfrastructure.iText;
#pragma warning restore SA1300

/// <summary> . </summary>
public static class ServiceCollectionExtensions
{
    /// <summary> . </summary>
    public static IServiceCollection AddSharedKerneliTextElectronicSignatureValidator(this IServiceCollection services)
    {
        return services
            .AddTransient<IElectronicSignatureValidator, ElectronicSignatureValidator>();
    }
}
