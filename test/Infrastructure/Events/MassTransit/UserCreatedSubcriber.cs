using MassTransit;
using SharedKernel.Application.Logging;
using SharedKernel.Domain.Tests.Users;
using System.Threading.Tasks;

namespace SharedKernel.Integration.Tests.Events.MassTransit
{
    internal class UserCreatedSubcriber : IConsumer<UserCreated>
    {
        private readonly ICustomLogger<UserCreatedSubcriber> _logger;

        public UserCreatedSubcriber(ICustomLogger<UserCreatedSubcriber> logger)
        {
            _logger = logger;
        }

        public Task Consume(ConsumeContext<UserCreated> context)
        {
            _logger.Info("Value: {Value}", context.Message.Id);
            return Task.CompletedTask;
        }
    }
}
