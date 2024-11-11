using SharedKernel.Application.Auth.UnitOfWork;
using SharedKernel.Application.Auth.Users.Services;
using SharedKernel.Application.Cqrs.Queries;
using SharedKernel.Application.Events;
using SharedKernel.Domain.Events;
using System.Security.Claims;

namespace SharedKernel.Application.Auth.Users.Queries;

internal sealed class GetClaimsHandler : IQueryRequestHandler<GetClaims, List<Claim>>
{
    private readonly IAuthUnitOfWork _unitOfWork;
    private readonly IEventBus _eventBus;
    private readonly IUserManager _userManager;

    public GetClaimsHandler(
        IAuthUnitOfWork unitOfWork,
        IEventBus eventBus,
        IUserManager userManager)
    {
        _unitOfWork = unitOfWork;
        _eventBus = eventBus;
        _userManager = userManager;
    }

    public async Task<List<Claim>> Handle(GetClaims request, CancellationToken cancellationToken)
    {
        var claims = await _userManager.GetClaimsAsync(request.Id, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        // ReSharper disable once UseCollectionExpression
        await _eventBus.Publish(Enumerable.Empty<DomainEvent>(), cancellationToken);

        return claims;
    }
}