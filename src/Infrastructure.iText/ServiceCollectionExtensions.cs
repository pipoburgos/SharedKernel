using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Application.Security;

namespace SharedKernelInfrastructure.iText;

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
