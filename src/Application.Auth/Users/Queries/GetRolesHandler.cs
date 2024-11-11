using SharedKernel.Application.Auth.UnitOfWork;
using SharedKernel.Application.Auth.Users.Services;
using SharedKernel.Application.Cqrs.Queries;
using SharedKernel.Application.Events;
using SharedKernel.Domain.Events;

namespace SharedKernel.Application.Auth.Users.Queries;

internal sealed class GetRolesHandler : IQueryRequestHandler<GetRoles, List<string>>
{
    private readonly IAuthUnitOfWork _unitOfWork;
    private readonly IEventBus _eventBus;
    private readonly IUserManager _userManager;

    public GetRolesHandler(
        IAuthUnitOfWork unitOfWork,
        IEventBus eventBus,
        IUserManager userManager)
    {
        _unitOfWork = unitOfWork;
        _eventBus = eventBus;
        _userManager = userManager;
    }

    public async Task<List<string>> Handle(GetRoles request, CancellationToken cancellationToken)
    {
        var roles = await _userManager.GetRolesAsync(request.Id, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        // ReSharper disable once UseCollectionExpression
        await _eventBus.Publish(Enumerable.Empty<DomainEvent>(), cancellationToken);

        return roles;
    }
}