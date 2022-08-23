using System;

namespace SharedKernel.Infrastructure.Events.Shared.RegisterEventSubscribers;

/// <summary>  </summary>
public interface IDomainEventSubscriberType
{
    /// <summary>
    /// 
    /// </summary>
    Type SubscribedEvent { get; }

    /// <summary> Gets the type of the subscriber class. </summary>
    Type GetSubscriber();

    /// <summary> Gets the name of the subscriber class. </summary>
    string SubscriberName();
}