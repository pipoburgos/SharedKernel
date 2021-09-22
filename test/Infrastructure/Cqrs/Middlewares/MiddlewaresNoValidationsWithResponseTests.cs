using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Application.Cqrs.Commands;
using SharedKernel.Application.Cqrs.Middlewares;
using SharedKernel.Infrastructure;
using SharedKernel.Infrastructure.Cqrs.Commands;
using SharedKernel.Infrastructure.Cqrs.Middlewares;
using SharedKernel.Integration.Tests.Cqrs.Commands;
using SharedKernel.Integration.Tests.Shared;
using Xunit;

namespace SharedKernel.Integration.Tests.Cqrs.Middlewares
{
    public class MiddlewaresNoValidationsWithResponseTests : InfrastructureTestCase
    {
        protected override IServiceCollection ConfigureServices(IServiceCollection services)
        {
            return services
                .AddSharedKernel()
                .AddCommandsHandlers(typeof(SampleCommandWithResponseHandler))
                .AddScoped(typeof(IMiddleware<,>), typeof(TimerMiddleware<,>))
                .AddTransient(typeof(IMiddleware<,>), typeof(ValidationMiddleware<,>))
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