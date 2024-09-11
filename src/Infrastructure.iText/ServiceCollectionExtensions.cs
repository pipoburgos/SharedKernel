using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Application.Security;

namespace SharedKernelInfrastructure.iText;

/// <summary> . </summary>
public static class ServiceCollectionExtensions
{
    /// <summary> . </summary>
    public static IServiceCollection AddiTextElectronicSignatureValidator(this IServiceCollection services)
    {
        return services
            .AddTransient<IElectronicSignatureValidator, ElectronicSignatureValidator>();
    }
}
