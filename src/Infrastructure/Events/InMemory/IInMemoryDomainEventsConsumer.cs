using SharedKernel.Domain.Events;
using System.Threading;
using System.Threading.Tasks;

namespace SharedKernel.Infrastructure.Events.InMemory
{
    /// <summary>
    /// 
    /// </summary>
    public interface IInMemoryDomainEventsConsumer
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="domainEvent"></param>
        void Add(DomainEvent domainEvent);

        /// <summary>
        /// 
        /// </summary>
        Task ExecuteAll(CancellationToken cancellationToken);
    }
}