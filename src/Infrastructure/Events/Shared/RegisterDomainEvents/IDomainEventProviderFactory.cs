using System;

namespace SharedKernel.Infrastructure.Events.Shared.RegisterDomainEvents;

/// <summary>  </summary>
internal interface IDomainEventProviderFactory
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="uniqueName"></param>
    /// <returns></returns>
    Type Get(string uniqueName);
}