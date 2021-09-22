using SharedKernel.Application.Cqrs.Commands.Handlers;
using System.Threading;
using System.Threading.Tasks;

namespace SharedKernel.Integration.Tests.Cqrs.Commands
{
    internal class SampleCommandHandler : ICommandRequestHandler<SampleCommand>
    {
        public Task Handle(SampleCommand command, CancellationToken cancellationToken)
        {
            return Task.FromResult(command.Value);
        }
    }
}
