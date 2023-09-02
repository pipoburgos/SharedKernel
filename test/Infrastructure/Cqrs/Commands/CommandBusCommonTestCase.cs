using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SharedKernel.Api.Security;
using SharedKernel.Application.Cqrs.Commands;
using SharedKernel.Application.Security;
using SharedKernel.Infrastructure;
using SharedKernel.Infrastructure.Cqrs.Commands;
using SharedKernel.Infrastructure.FluentValidation;
using SharedKernel.Infrastructure.NetJson;
using SharedKernel.Infrastructure.Polly.Requests.Middlewares;
using SharedKernel.Infrastructure.Requests.Middlewares;
using SharedKernel.Testing.Infrastructure;
using System.Security.Claims;

namespace SharedKernel.Integration.Tests.Cqrs.Commands;

public abstract class CommandBusCommonTestCase : InfrastructureTestCase<FakeStartup>
{
    protected override IServiceCollection ConfigureServices(IServiceCollection services)
    {
        return services
            .AddSharedKernel()
            .AddCommandsHandlers(typeof(SampleCommandWithResponseHandler))
            .AddFluentValidation(typeof(SampleCommandValidator))
            .AddNetJsonSerializer()
            .AddValidationMiddleware()
            .AddRetryPolicyMiddleware<RetryPolicyExceptionHandler>(Configuration)
            .AddSingleton<SaveValueSingletonService>()
            .RemoveAll<IIdentityService>()
            .AddScoped<IIdentityService, HttpContextAccessorIdentityService>()
            .AddHttpContextAccessor();
    }

    protected async Task DispatchCommand()
    {
        var httpContextAccessor = GetServiceOnNewScope<IIdentityService>();

        if (httpContextAccessor != default)
        {
            httpContextAccessor.User =
                new ClaimsPrincipal(new ClaimsIdentity(new List<Claim> { new Claim("Name", "Peter") }));

            httpContextAccessor.AddKeyValue("Authorization", "Prueba");
        }

        var commandBus = GetRequiredServiceOnNewScope<ICommandBus>();
        var command = new SampleCommand(3);
        await commandBus.Dispatch(command, CancellationToken.None);

        // Esperar a que terminen los manejadores de los eventos junto con la política de reintentos
        var saveValueSingletonService = GetRequiredServiceOnNewScope<SaveValueSingletonService>();
        await Task.Delay(TimeSpan.FromSeconds(2), CancellationToken.None);

        saveValueSingletonService.Id.Should().Be(command.Value);
    }

    protected async Task DispatchCommandAsync(int secondsRelay = 10)
    {
        var httpContextAccessor = GetServiceOnNewScope<IIdentityService>();

        if (httpContextAccessor != default)
        {
            httpContextAccessor.User =
                new ClaimsPrincipal(new ClaimsIdentity(new List<Claim> { new Claim("Name", "Peter") }));

            httpContextAccessor.AddKeyValue("Authorization", "Prueba");
        }

        var commandBus = GetRequiredServiceOnNewScope<ICommandBusAsync>();

        await Task.Delay(TimeSpan.FromSeconds(secondsRelay));
        var command = new SampleCommand(3);
        await commandBus.Dispatch(command, CancellationToken.None);

        // Esperar a que terminen los manejadores de los eventos junto con la política de reintentos
        await Task.Delay(TimeSpan.FromSeconds(secondsRelay));

        var saveValueSingletonService = GetRequiredServiceOnNewScope<SaveValueSingletonService>();
        saveValueSingletonService.Id.Should().Be(command.Value);
    }
}
