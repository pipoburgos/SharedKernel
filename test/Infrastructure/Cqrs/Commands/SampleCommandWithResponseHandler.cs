﻿using SharedKernel.Application.Cqrs.Commands.Handlers;

namespace SharedKernel.Integration.Tests.Cqrs.Commands;

internal class SampleCommandWithResponseHandler : ICommandRequestHandler<SampleCommandWithResponse, int>
{
    public Task<int> Handle(SampleCommandWithResponse command, CancellationToken cancellationToken)
    {
        return Task.FromResult(command.Value);
    }
}