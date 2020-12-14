using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SharedKernel.Application.Cqrs.Commands;
using SharedKernel.Application.Cqrs.Commands.Handlers;
using SharedKernel.Infrastructure.Cqrs.Middlewares;

namespace SharedKernel.Infrastructure.Cqrs.Commands.InMemory
{
    /// <summary>
    /// 
    /// </summary>
    public class InMemoryCommandBus : ICommandBus
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IBackgroundTaskQueue _taskQueue;
        private readonly ExecuteMiddlewaresService _executeMiddlewaresService;
        private readonly IHostApplicationLifetime _applicationLifetime;

        private static readonly ConcurrentDictionary<Type, IEnumerable<CommandHandlerWrapper>>
            CommandHandlers = new ConcurrentDictionary<Type, IEnumerable<CommandHandlerWrapper>>();

        private static readonly ConcurrentDictionary<Type, object> CommandHandlersResponse = new ConcurrentDictionary<Type, object>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <param name="serviceScopeFactory"></param>
        /// <param name="taskQueue"></param>
        /// <param name="executeMiddlewaresService"></param>
        /// <param name="applicationLifetime"></param>
        public InMemoryCommandBus(
            IServiceProvider serviceProvider,
            IServiceScopeFactory serviceScopeFactory,
            IBackgroundTaskQueue taskQueue,
            ExecuteMiddlewaresService executeMiddlewaresService,
            IHostApplicationLifetime applicationLifetime)
        {
            _serviceProvider = serviceProvider;
            _serviceScopeFactory = serviceScopeFactory;
            _taskQueue = taskQueue;
            _executeMiddlewaresService = executeMiddlewaresService;
            _applicationLifetime = applicationLifetime;
        }

        public async Task<TResponse> Dispatch<TResponse>(ICommandRequest<TResponse> command, CancellationToken cancellationToken)
        {
            await _executeMiddlewaresService.ExecuteAsync(command, cancellationToken);

            var wrappedHandlers = GetWrappedHandlers(command);

            if (wrappedHandlers == null)
                throw new CommandNotRegisteredError(command.ToString());

            return await wrappedHandlers.Handle(command, _serviceProvider, cancellationToken);
        }

        public async Task Dispatch(ICommandRequest command, CancellationToken cancellationToken = default)
        {
            await _executeMiddlewaresService.ExecuteAsync(command, cancellationToken);

            var wrappedHandlers = GetWrappedHandlers(command);

            if (wrappedHandlers == null)
                throw new CommandNotRegisteredError(command.ToString());

            var tasks = wrappedHandlers.Select(handler => handler.Handle(command, _serviceProvider, cancellationToken));

            await Task.WhenAll(tasks);
        }

        public Task DispatchInBackground(ICommandRequest command, CancellationToken cancellationToken)
        {
            return Task.Run(async () => await Dispatch(command, cancellationToken), cancellationToken);
        }

        public Task QueueInBackground(ICommandRequest command)
        {
            _taskQueue.QueueBackground(async _ =>
            {
                using var scope = _serviceScopeFactory.CreateScope();
                var commandBus = scope.ServiceProvider.GetRequiredService<ICommandBus>();
                await commandBus.Dispatch(command, _applicationLifetime.ApplicationStopping);
            });

            return Task.CompletedTask;
        }

        private IEnumerable<CommandHandlerWrapper> GetWrappedHandlers(ICommandRequest command)
        {
            var handlerType = typeof(ICommandRequestHandler<>).MakeGenericType(command.GetType());
            var wrapperType = typeof(CommandHandlerWrapper<>).MakeGenericType(command.GetType());

            var handlers =
                (IEnumerable)_serviceProvider.GetRequiredService(typeof(IEnumerable<>).MakeGenericType(handlerType));

            var instance = handlers.Cast<object>()
                .Select(_ => (CommandHandlerWrapper)Activator.CreateInstance(wrapperType));

            var wrappedHandlers = CommandHandlers
                .GetOrAdd(command.GetType(), instance);

            return wrappedHandlers;
        }

        private CommandHandlerWrapperResponse<TResponse> GetWrappedHandlers<TResponse>(ICommandRequest<TResponse> command)
        {
            var handlerType = typeof(ICommandRequestHandler<,>).MakeGenericType(command.GetType());
            var wrapperType = typeof(CommandHandlerWrapperResponse<,>).MakeGenericType(command.GetType());

            var handlers =
                (IEnumerable)_serviceProvider.GetRequiredService(typeof(IEnumerable<>).MakeGenericType(handlerType));

            var wrappedHandlers = (CommandHandlerWrapperResponse<TResponse>)CommandHandlersResponse.GetOrAdd(command.GetType(), handlers.Cast<object>()
                .Select(_ => (CommandHandlerWrapperResponse<TResponse>)Activator.CreateInstance(wrapperType)).FirstOrDefault());

            return wrappedHandlers;
        }
    }
}
