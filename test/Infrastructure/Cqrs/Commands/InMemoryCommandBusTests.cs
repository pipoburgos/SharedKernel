using FluentAssertions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Application.Cqrs.Commands;
using SharedKernel.Infrastructure;
using SharedKernel.Infrastructure.Cqrs.Commands;
using SharedKernel.Infrastructure.System.Threading;
using SharedKernel.Infrastructure.Validators;
using SharedKernel.Integration.Tests.Shared;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SharedKernel.Integration.Tests.Cqrs.Commands
{
    public class InMemoryCommandBusTests : InfrastructureTestCase
    {
        protected override IServiceCollection ConfigureServices(IServiceCollection services)
        {
            return services
                .AddSharedKernel()
                .AddCommandsHandlers(typeof(SampleCommandWithResponseHandler))
                .AddValidators(typeof(SampleCommandWithResponseHandler))
                .AddInMemoryCommandBus()
                .AddInMemoryMutex();
        }

        [Fact]
        public async Task TestCommandHandlerWithResponse()
        {
            const int value = 15;
            var request = new SampleCommandWithResponse(value);

            var response = await GetRequiredService<ICommandBus>().Dispatch(request, CancellationToken.None);

            response.Should().Be(value);
        }

        [Fact]
        public void RegisterValidators()
        {
            var validator = GetService<IValidator<SampleCommandWithResponse>>();
            validator.Should().NotBeNull();
        }

        [Fact]
        public async Task DispatchInQueueSumTime()
        {
            var delayCommand = new DelayCommand(2);

            var commandBus = GetRequiredService<ICommandBus>();
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            var response = commandBus.DispatchOnQueue(delayCommand, "queue", CancellationToken.None);
            var response2 = commandBus.DispatchOnQueue(delayCommand, "queue2", CancellationToken.None);
            var response3 = commandBus.DispatchOnQueue(delayCommand, "queue", CancellationToken.None);
            var response4 = commandBus.DispatchOnQueue(delayCommand, "queue", CancellationToken.None);

            await Task.WhenAll(response, response2, response3, response4);

            stopWatch.Stop();

            stopWatch.ElapsedMilliseconds.Should().BeGreaterOrEqualTo(6_000);
            stopWatch.ElapsedMilliseconds.Should().BeLessOrEqualTo(6_250);
        }

    }
}
