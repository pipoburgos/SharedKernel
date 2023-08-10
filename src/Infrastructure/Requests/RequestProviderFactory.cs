using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace SharedKernel.Infrastructure.Requests;

/// <summary>  </summary>
internal class RequestProviderFactory : IRequestProviderFactory
{
    private readonly IServiceProvider _serviceProvider;

    /// <summary>  </summary>
    public RequestProviderFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    /// <summary>  </summary>
    public Type Get(string uniqueName)
    {
        var providers = _serviceProvider.GetServices<IRequestType>().ToDictionary(e => e.UniqueName, a => a.Type);

        if (!providers.ContainsKey(uniqueName))
            throw new ArgumentException($"Request {uniqueName} not registered.");

        return providers[uniqueName];
    }
}