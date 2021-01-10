using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Application.Cqrs.Commands;
using SharedKernel.Infrastructure;
using SharedKernel.Infrastructure.Cqrs.Commands;
using SharedKernel.Infraestructure.Tests.Shared;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SharedKernel.Infraestructure.Tests.Cqrs.Commands
{
    public class InMemoryCommandBusTests : InfrastructureTestCase
    {
        protected override IServiceCollection ConfigureServices(IServiceCollection services)
        {
            return services
                .AddSharedKernel()
                .AddCommandsHandlers(typeof(SampleCommandWithResponseHandler))
                .AddInMemoryCommandBus();
        }

        [Fact]
        public async Task TestCommandHandlerWithResponse()
        {
            const int value = 15;
            var request = new SampleCommandWithResponse(value);

            var response = await GetRequiredService<ICommandBus>().Dispatch(request, CancellationToken.None);

            Assert.Equal(value, response);
        }
    }
}
