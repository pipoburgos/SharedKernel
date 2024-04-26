using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Application.Cqrs.Commands;
using SharedKernel.Application.Cqrs.Commands.Handlers;
using SharedKernel.Application.Cqrs.Middlewares;
using SharedKernel.Application.Events;
using SharedKernel.Application.Security;
using SharedKernel.Application.Serializers;
using System.Security.Claims;
// ReSharper disable ConditionalAccessQualifierIsNonNullableAccordingToAPIContract

namespace SharedKernel.Infrastructure.Requests;

/// <summary>
/// 
/// </summary>
internal class RequestMediator : IRequestMediator
{
    private readonly ILogger<RequestMediator> _logger;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly IRequestDeserializer _requestDeserializer;
    private readonly IJsonSerializer _jsonSerializer;
    private readonly IPipeline _pipeline;

    /// <summary>
    /// Constructor
    /// </summary>
    public RequestMediator(
        ILogger<RequestMediator> logger,
        IServiceScopeFactory serviceScopeFactory,
        IRequestDeserializer requestDeserializer,
        IJsonSerializer jsonSerializer,
        IPipeline pipeline)
    {
        _logger = logger;
        _serviceScopeFactory = serviceScopeFactory;
        _requestDeserializer = requestDeserializer;
        _jsonSerializer = jsonSerializer;
        _pipeline = pipeline;
    }

    public bool HandlerImplemented(string requestSerialized)
    {
        var eventDeserialized = _requestDeserializer.Deserialize(requestSerialized);

        var handlerType = typeof(ICommandRequestHandler<>).MakeGenericType(eventDeserialized.GetType());

        using var scope = _serviceScopeFactory.CreateScope();

        return scope.ServiceProvider.GetService(handlerType) != null;
    }

    /// <summary>  </summary>
    public Task Execute(string requestSerialized, Type type, string method, CancellationToken cancellationToken)
    {
        var eventDeserialized = _requestDeserializer.Deserialize(requestSerialized);

        var handlerType = type.MakeGenericType(eventDeserialized.GetType());

        return Execute(requestSerialized, eventDeserialized, handlerType, method, cancellationToken);
    }

    /// <summary>  </summary>
    public Task Execute(string requestSerialized, Request request, Type handlerType, string method, CancellationToken cancellationToken)
    {
        return _pipeline.ExecuteAsync(request, cancellationToken, async (req, ct) =>
        {
            try
            {
                _logger.LogInformation($"Executing {handlerType.FullName} with data: {requestSerialized}");

                using var scope = _serviceScopeFactory.CreateScope();

                AddIdentity(requestSerialized, scope);

                var parameters = new List<object> { req, ct }.ToArray();

                switch (method)
                {
                    case nameof(ICommandRequestHandler<CommandRequest>.Handle):
                        {

                            // not call GetRequiredService Async commands are in other api
                            var handler = scope.ServiceProvider.GetService(handlerType);

                            if (handler != default)
                                await ((Task)handlerType.GetMethod(method)?.Invoke(handler, parameters)!)!;

                            break;
                        }
                    case nameof(IDomainEventSubscriber<DomainEvent>.On):
                        {
                            var types = scope.ServiceProvider.GetServices(handlerType);
                            await Task.WhenAll(types.Select(type =>
                                (Task)handlerType.GetMethod(method)?.Invoke(type, parameters)!));
                        }

                        break;
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
            }
        });
    }

    private void AddIdentity(string body, IServiceScope scope)
    {
        var identityService = scope.ServiceProvider.GetService<IIdentityService>();

        if (identityService == default)
            return;

        var eventData = _jsonSerializer.Deserialize<Dictionary<string, Dictionary<string, object>>>(body, NamingConvention.PascalCase);
        if (eventData == default)
            throw new ArgumentException(nameof(eventData));

        var headers = eventData[RequestExtensions.Headers];

        var authorization = headers[RequestExtensions.Authorization]?.ToString();
        if (authorization != default && !string.IsNullOrWhiteSpace(authorization))
            identityService.AddKeyValue("Authorization", authorization);

        var domainClaimsString = headers[RequestExtensions.Claims]?.ToString();
        if (domainClaimsString == null)
            return;

        var domainClaims = _jsonSerializer.Deserialize<List<RequestClaim>?>(domainClaimsString!, NamingConvention.PascalCase);
        if (domainClaims == default || !domainClaims.Any())
            return;

        identityService.User =
            new ClaimsPrincipal(new ClaimsIdentity(domainClaims.Select(dc => new Claim(dc.Type, dc.Value))));
    }
}
