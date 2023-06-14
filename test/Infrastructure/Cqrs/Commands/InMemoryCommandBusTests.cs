using FluentAssertions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Application.Cqrs.Commands;
using SharedKernel.Application.Validator;
using SharedKernel.Infrastructure;
using SharedKernel.Infrastructure.Cqrs.Commands;
using SharedKernel.Infrastructure.Cqrs.Middlewares;
using SharedKernel.Infrastructure.System.Threading;
using SharedKernel.Infrastructure.Validators;
using SharedKernel.Testing.Infrastructure;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SharedKernel.Integration.Tests.Cqrs.Commands
{
    public class InMemoryCommandBusTests : InfrastructureTestCase<FakeStartup>
    {
        protected override IServiceCollection ConfigureServices(IServiceCollection services)
        {
            return services
                .AddSharedKernel()
                .AddCommandsHandlers(typeof(SampleCommandWithResponseHandler))
                .AddValidators(typeof(SampleCommandValidator))
                .AddInMemoryCommandBus()
                .AddInMemoryMutex()
                .AddValidationMiddleware();
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
        public void TestCommandValidatorEmpty()
        {
            var request = new SampleCommand(default);

            // ReSharper disable once SuggestVarOrType_Elsewhere
            Func<Task> response = async () => await GetRequiredService<ICommandBus>().Dispatch(request, CancellationToken.None);

            response.Should().ThrowAsync<ValidationFailureException>().WithMessage("0 is invalid.");
        }

        [Fact]
        public void TestCommandValidatorWithResponseEmpty()
        {
            var request = new SampleCommandWithResponse(default);

            // ReSharper disable once SuggestVarOrType_Elsewhere
            Func<Task> response = async () => await GetRequiredService<ICommandBus>().Dispatch(request, CancellationToken.None);

            response.Should().ThrowAsync<ValidationFailureException>().WithMessage("'Value' must not be empty.");
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
