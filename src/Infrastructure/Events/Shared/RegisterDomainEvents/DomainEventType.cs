using System;

namespace SharedKernel.Infrastructure.Events.Shared.RegisterDomainEvents
{

    /// <summary>  </summary>
    public class DomainEventType : IDomainEventType
    {
        /// <summary>  </summary>
        public DomainEventType(string uniqueName, Type type)
        {
            UniqueName = uniqueName;
            Type = type;
        }

        /// <summary>  </summary>
        public string UniqueName { get; }

        /// <summary>  </summary>
        public Type Type { get; }
    }
}