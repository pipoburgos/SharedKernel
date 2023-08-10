using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Application.Cqrs.Commands.Handlers;
using SharedKernel.Application.Logging;
using SharedKernel.Application.Security;
using SharedKernel.Application.Serializers;
using SharedKernel.Domain.Requests;
using SharedKernel.Infrastructure.Cqrs.Middlewares;
using SharedKernel.Infrastructure.Events.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace SharedKernel.Infrastructure.Requests;

/// <summary>
/// 
/// </summary>
internal class RequestMediator : IRequestMediator
{
    private readonly ICustomLogger<RequestMediator> _logger;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly IRequestDeserializer _requestDeserializer;
    private readonly IJsonSerializer _jsonSerializer;
    private readonly IExecuteMiddlewaresService _executeMiddlewaresService;

    /// <summary>
    /// Constructor
    /// </summary>
    public RequestMediator(
        ICustomLogger<RequestMediator> logger,
        IServiceScopeFactory serviceScopeFactory,
        IRequestDeserializer requestDeserializer,
        IJsonSerializer jsonSerializer,
        IExecuteMiddlewaresService executeMiddlewaresService)
    {
        _logger = logger;
        _serviceScopeFactory = serviceScopeFactory;
        _requestDeserializer = requestDeserializer;
        _jsonSerializer = jsonSerializer;
        _executeMiddlewaresService = executeMiddlewaresService;
    }

    /// <summary>  </summary>
    public Task ExecuteHandler(string eventSerialized, CancellationToken cancellationToken)
    {
        var eventDeserialized = _requestDeserializer.Deserialize(eventSerialized);

        var handler = typeof(ICommandRequestHandler<>).MakeGenericType(eventDeserialized.GetType());

        return ExecuteHandler(eventSerialized, eventDeserialized, handler, cancellationToken);
    }

    /// <summary>  </summary>
    public Task ExecuteHandler(string body, Request request, Type handlerType, CancellationToken cancellationToken)
    {
        return _executeMiddlewaresService.ExecuteAsync(request, cancellationToken, async (req, ct) =>
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var identityService = scope.ServiceProvider.GetService<IIdentityService>();
            AddIdentity(body, identityService);

            var handler = scope.ServiceProvider.GetRequiredService(handlerType);
            _logger.Info($"Executing {handlerType.FullName} with data: {body}");

            var parameters = new List<object> { req, ct }.ToArray();
            await ((Task)handlerType.GetMethod("Handle")?.Invoke(handler, parameters))!;
        });
    }

    private void AddIdentity(string body, IIdentityService identityService)
    {
        if (identityService == default)
            return;

        var eventData = _jsonSerializer.Deserialize<Dictionary<string, Dictionary<string, object>>>(body);
        if (eventData == default)
            throw new ArgumentException(nameof(eventData));

        var headers = eventData["headers"];

        var domainClaimsString = headers["claims"]?.ToString();
        if (domainClaimsString == null)
            return;

        var domainClaims = _jsonSerializer.Deserialize<List<DomainClaim>>(domainClaimsString!);
        if (domainClaims == default || !domainClaims.Any())
            return;

        identityService.User =
            new ClaimsPrincipal(new ClaimsIdentity(domainClaims.Select(dc => new Claim(dc.Type, dc.Value))));

        var authorization = headers["authorization"]?.ToString();
        if (!string.IsNullOrWhiteSpace(authorization))
            identityService.AddKeyValue("Authorization", authorization);
    }
}
