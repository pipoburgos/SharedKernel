using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Application.Reporting;

namespace SharedKernel.Infrastructure.Reporting;

/// <summary> . </summary>
public static class ServiceCollectionExtensions
{
    /// <summary> . </summary>
    public static IServiceCollection AddSharedKernelReportingRenderer(this IServiceCollection services)
    {
        return services
            .AddTransient<IReportRenderer, ReportRenderer>();
    }
}
