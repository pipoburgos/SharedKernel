﻿using SharedKernel.Application.Auth.UnitOfWork;
using SharedKernel.Application.Auth.Users.Services;
using SharedKernel.Application.Cqrs.Queries;
using SharedKernel.Application.Events;
using SharedKernel.Domain.Events;

namespace SharedKernel.Application.Auth.Users.Queries;

internal sealed class GeneratePasswordResetTokenHandler : IQueryRequestHandler<GeneratePasswordResetToken, string>
{
    private readonly IAuthUnitOfWork _unitOfWork;
    private readonly IEventBus _eventBus;
    private readonly IUserManager _userManager;

    public GeneratePasswordResetTokenHandler(
        IAuthUnitOfWork unitOfWork,
        IEventBus eventBus,
        IUserManager userManager)
    {
        _unitOfWork = unitOfWork;
        _eventBus = eventBus;
        _userManager = userManager;
    }

    public async Task<string> Handle(GeneratePasswordResetToken request, CancellationToken cancellationToken)
    {
        var token = await _userManager.GeneratePasswordResetTokenAsync(request.Email, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        // ReSharper disable once UseCollectionExpression
        await _eventBus.Publish(Enumerable.Empty<DomainEvent>(), cancellationToken);

        return token;
    }
}