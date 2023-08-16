using Microsoft.Extensions.Options;
using SharedKernel.Application.Events;
using SharedKernel.Domain.Events;
using SharedKernel.Infrastructure.Cqrs.Middlewares;
using SharedKernel.Infrastructure.Requests;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SharedKernel.Infrastructure.Events.ApacheActiveMq
{
    /// <summary>  </summary>
    public class ApacheActiveMqEventBus : ApacheActiveMqPublisher, IEventBus
    {
        private readonly IRequestSerializer _requestSerializer;
        private readonly IExecuteMiddlewaresService _executeMiddlewaresService;

        /// <summary>  </summary>
        public ApacheActiveMqEventBus(
            IRequestSerializer requestSerializer,
            IExecuteMiddlewaresService executeMiddlewaresService,
            IOptions<ApacheActiveMqConfiguration> configuration) : base(configuration)
        {
            _requestSerializer = requestSerializer;
            _executeMiddlewaresService = executeMiddlewaresService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="events"></param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns></returns>
        public Task Publish(IEnumerable<DomainEvent> events, CancellationToken cancellationToken)
        {
            return Task.WhenAll(events.Select(@event => Publish(@event, cancellationToken)));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="event"></param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns></returns>
        public Task Publish(DomainEvent @event, CancellationToken cancellationToken)
        {
            return _executeMiddlewaresService.ExecuteAsync(@event, cancellationToken, (req, _) =>
            {
                var serializedDomainEvent = _requestSerializer.Serialize(req);

                return PublishTopic(serializedDomainEvent, @event.GetEventName());
            });
        }
    }
}
