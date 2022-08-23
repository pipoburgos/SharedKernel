using SharedKernel.Domain.Events;
using System;
using System.Collections.Generic;

namespace SharedKernel.Infrastructure.Events.Shared.RegisterEventSubscribers
{
    /// <summary>  </summary>
    internal interface IDomainEventSubscriberProviderFactory
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="event"></param>
        /// <returns></returns>
        IEnumerable<Type> GetSubscribers(DomainEvent @event);

        List<IDomainEventSubscriberType> GetAll();
    }
}