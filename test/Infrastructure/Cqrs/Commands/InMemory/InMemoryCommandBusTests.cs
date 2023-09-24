using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Application.Cqrs.Commands;
using SharedKernel.Application.Validator;
using SharedKernel.Infrastructure.Cqrs.Commands;

namespace SharedKernel.Integration.Tests.Cqrs.Commands.InMemory;

public class InMemoryCommandBusTests : CommandBusCommonTestCase
{
    protected override IServiceCollection ConfigureServices(IServiceCollection services)
    {
        return base.ConfigureServices(services).AddInMemoryCommandBus();
    }

    [Fact]
    public async Task DispatchCommandFromInMemoryCommandBus()
    {
        await DispatchCommand();
    }

    [Fact]
    public async Task TestCommandHandlerWithResponse()
    {
        const int value = 15;
        var request = new SampleCommandWithResponse(value);

        var response = await GetRequiredServiceOnNewScope<ICommandBus>().Dispatch(request, CancellationToken.None);

        response.Should().Be(value);
    }

    [Fact]
    public void TestCommandValidatorEmpty()
    {
        var request = new SampleCommand(default);

        // ReSharper disable once SuggestVarOrType_Elsewhere
        Func<Task> response = async () => await GetRequiredServiceOnNewScope<ICommandBus>().Dispatch(request, CancellationToken.None);

        response.Should().ThrowAsync<ValidationFailureException>().WithMessage("0 is invalid.");
    }

    [Fact]
    public void TestCommandValidatorWithResponseEmpty()
    {
        var request = new SampleCommandWithResponse(default);

        // ReSharper disable once SuggestVarOrType_Elsewhere
        Func<Task> response = async () => await GetRequiredServiceOnNewScope<ICommandBus>().Dispatch(request, CancellationToken.None);

        response.Should().ThrowAsync<ValidationFailureException>().WithMessage("'Value' must not be empty.");
    }

    [Fact]
    public void RegisterValidators()
    {
        var validator = GetServiceOnNewScope<IValidator<SampleCommandWithResponse>>();
        validator.Should().NotBeNull();
    }

    //[Fact]
    //public async Task DispatchInQueueSumTime()
    //{
    //    var commandBus = GetRequiredService<ICommandBus>();
    //    var stopWatch = new Stopwatch();
    //    stopWatch.Start();

    //    var response = commandBus.DispatchOnQueue(new DelayCommand(1), "queue", CancellationToken.None);
    //    var response2 = commandBus.DispatchOnQueue(new DelayCommand(3), "queue2", CancellationToken.None);
    //    var response3 = commandBus.DispatchOnQueue(new DelayCommand(1), "queue", CancellationToken.None);
    //    var response4 = commandBus.DispatchOnQueue(new DelayCommand(1), "queue", CancellationToken.None);

    //    await Task.WhenAll(response, response2, response3, response4);

    //    stopWatch.Stop();

    //    stopWatch.ElapsedMilliseconds.Should().BeGreaterOrEqualTo(3_000);
    //    stopWatch.ElapsedMilliseconds.Should().BeLessOrEqualTo(5_900);
    //}

}
