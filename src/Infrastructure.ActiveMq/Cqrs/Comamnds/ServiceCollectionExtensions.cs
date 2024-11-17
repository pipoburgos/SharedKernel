using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Application.Cqrs.Commands;

namespace SharedKernel.Infrastructure.ActiveMq.Cqrs.Comamnds;

/// <summary> . </summary>
public static class ServiceCollectionExtensions
{
    /// <summary> . </summary>
    public static IServiceCollection AddSharedKernelActiveMqCommandBusAsync(this IServiceCollection services, IConfiguration configuration)
    {
        return services
            .AddActiveMqHealthChecks(configuration, "ActiveMq Command Bus", "ActiveMq", "CommandBus")
            .AddHostedService<ActiveMqQueueConsumer>()
            .AddTransient<ICommandBusAsync, ActiveMqCommandBusAsync>();
    }
}