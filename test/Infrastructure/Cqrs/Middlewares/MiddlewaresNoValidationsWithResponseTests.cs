using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Application.Cqrs.Commands;
using SharedKernel.Infrastructure;
using SharedKernel.Infrastructure.Cqrs.Commands;
using SharedKernel.Infrastructure.Cqrs.Middlewares;
using SharedKernel.Integration.Tests.Cqrs.Commands;
using SharedKernel.Testing.Infrastructure;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SharedKernel.Integration.Tests.Cqrs.Middlewares
{
    public class MiddlewaresNoValidationsWithResponseTests : InfrastructureTestCase<FakeStartup>
    {
        protected override IServiceCollection ConfigureServices(IServiceCollection services)
        {
            return services
                .AddSharedKernel()
                .AddCommandsHandlers(typeof(SampleCommandWithResponseHandler))
                .AddTimerMiddleware()
                .AddInMemoryCommandBus();
        }

        [Fact]
        public async Task TestCommandHandlerWithResponse()
        {
            var request = new SampleCommandWithResponse(0);
            var result = await GetRequiredService<ICommandBus>().Dispatch(request, CancellationToken.None);
            result.Should().Be(0);
        }
    }
}