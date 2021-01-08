using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace SharedKernel.Infrastructure.Events.InMemory
{
    /// <summary>
    /// 
    /// </summary>
    public class DomainEventsToExecute
    {
        /// <summary>
        /// 
        /// </summary>
        public ConcurrentBag<Func<CancellationToken, Task>> Subscribers = new();

        // TODO: Falta poner privado el ConcurrentBag y crear métodos públicos de acceso
    }
}
