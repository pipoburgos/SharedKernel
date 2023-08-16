using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SharedKernel.Infrastructure.Requests;

/// <summary>  </summary>
internal class RequestProviderFactory : IRequestProviderFactory
{
    private readonly Dictionary<string, Type> _providers;

    /// <summary>  </summary>
    public RequestProviderFactory(IServiceProvider serviceProvider)
    {
        _providers = serviceProvider.GetServices<IRequestType>().ToDictionary(e => e.UniqueName, a => a.Type);
    }

    /// <summary>  </summary>
    public Type Get(string uniqueName)
    {
        if (!_providers.ContainsKey(uniqueName))
            throw new ArgumentException($"Request {uniqueName} not registered.");

        return _providers[uniqueName];
    }
}