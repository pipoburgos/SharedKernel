using SharedKernel.Application.Auth.Roles.Services;
using SharedKernel.Application.Auth.UnitOfWork;
using SharedKernel.Application.Cqrs.Commands.Handlers;
using SharedKernel.Application.Events;
using SharedKernel.Domain.Events;

namespace SharedKernel.Application.Auth.Roles.Commands;

internal sealed class CreateRoleHandler : ICommandRequestHandler<CreateRole>
{
    private readonly IAuthUnitOfWork _unitOfWork;
    private readonly IEventBus _eventBus;
    private readonly IRoleManager _roleManager;

    public CreateRoleHandler(
        IAuthUnitOfWork unitOfWork,
        IEventBus eventBus,
        IRoleManager roleManager)
    {
        _unitOfWork = unitOfWork;
        _eventBus = eventBus;
        _roleManager = roleManager;
    }

    public async Task Handle(CreateRole request, CancellationToken cancellationToken)
    {
        await _roleManager.Create(request.Id, request.Name, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        // ReSharper disable once UseCollectionExpression
        await _eventBus.Publish(Enumerable.Empty<DomainEvent>(), cancellationToken);
    }
}