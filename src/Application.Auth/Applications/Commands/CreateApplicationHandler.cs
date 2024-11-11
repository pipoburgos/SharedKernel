using SharedKernel.Application.Auth.Applications.Services;
using SharedKernel.Application.Cqrs.Commands.Handlers;
using SharedKernel.Application.Security;
using SharedKernel.Application.Settings;
using SharedKernel.Application.System;

namespace SharedKernel.Application.Auth.Applications.Commands;

internal class CreateApplicationHandler : ICommandRequestHandler<CreateApplication>
{
    private readonly IApplicationManager _applicationManager;
    private readonly OpenIdOptions _options;

    public CreateApplicationHandler(
        IApplicationManager applicationManager,
        IOptionsService<OpenIdOptions> options)
    {
        _applicationManager = applicationManager;
        _options = options.Value;
    }

    public Task Handle(CreateApplication command, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(_options.ClientId) || string.IsNullOrWhiteSpace(_options.ClientSecret))
            return TaskHelper.CompletedTask;

        return _applicationManager.CreateClientCredentialsWithPassword(_options.ClientId!, command.Name,
            _options.ClientSecret!, command.Scope, cancellationToken);
    }
}
