using System.Threading;
using System.Threading.Tasks;
using SharedKernel.Domain.Events;

namespace SharedKernel.Infrastructure.Events.InMemory
{
    /// <summary>
    /// 
    /// </summary>
    public interface IDomainEventsToExecute
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