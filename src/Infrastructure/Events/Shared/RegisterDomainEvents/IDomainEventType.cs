using System;

namespace SharedKernel.Infrastructure.Events.Shared.RegisterDomainEvents
{
    /// <summary>  </summary>
    public interface IDomainEventType
    {
        /// <summary>  </summary>
        string UniqueName { get; }

        /// <summary>  </summary>
        Type Type { get; }
    }
}