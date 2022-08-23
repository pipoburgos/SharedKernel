using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SharedKernel.Infrastructure.Events.Shared.RegisterDomainEvents;

/// <summary>  </summary>
internal class DomainEventProviderFactory : IDomainEventProviderFactory
{
    private readonly Dictionary<string, Type> _providers;

    /// <summary>  </summary>
    public DomainEventProviderFactory(IServiceProvider serviceProvider)
    {
        _providers = serviceProvider.GetServices<IDomainEventType>().ToDictionary(e => e.UniqueName, a => a.Type);
    }

    /// <summary>  </summary>
    public Type Get(string uniqueName)
    {
        if (!_providers.ContainsKey(uniqueName))
            throw new ArgumentException($"Event {uniqueName} not registered.");

        return _providers[uniqueName];
    }
}