using System.Threading;
using System.Threading.Tasks;

namespace SharedKernel.Domain.Events
{
    public interface IDomainEventsConsumer
    {
        Task Consume(CancellationToken cancellationToken);
    }
}