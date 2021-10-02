using SharedKernel.Application.Logging;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SharedKernel.Infrastructure.Events.InMemory
{
    /// <summary>
    /// 
    /// </summary>
    public class DomainEventsToExecute
    {
        private readonly ICustomLogger<DomainEventsToExecute> _logger;
        private readonly TimeSpan _delayTimeSpan;
        private readonly SemaphoreSlim _semaphore;
        private readonly ConcurrentDictionary<Guid, Func<CancellationToken, Task>> _subscribers = new();
        private readonly ConcurrentDictionary<Guid, int> _retries = new();

        /// <summary>
        /// 
        /// </summary>
        public DomainEventsToExecute(
            ICustomLogger<DomainEventsToExecute> logger,
            TimeSpan delayTimeSpan)
        {
            _logger = logger;
            _delayTimeSpan = delayTimeSpan;
            _semaphore = new SemaphoreSlim(1, 1);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="func"></param>
        public void Add(Func<CancellationToken, Task> func)
        {
            var functionId = Guid.NewGuid();
            _subscribers.TryAdd(functionId, func);
            _retries.TryAdd(functionId, 0);
        }

        /// <summary>
        /// 
        /// </summary>
        public async Task ExecuteAll(CancellationToken cancellationToken)
        {
            await _semaphore.WaitAsync(cancellationToken);

            try
            {
                foreach (var subscriber in _subscribers.ToList())
                {
                    try
                    {
                        await subscriber.Value(cancellationToken);
                        _subscribers.TryRemove(subscriber.Key, out _);
                        _retries.TryRemove(subscriber.Key, out _);
                    }
                    catch (Exception e)
                    {
                        var retry = _retries[subscriber.Key];
                        _logger.Warn($"Retry number {retry}");
                        _logger?.Error(e, e.Message);
                        _retries.TryRemove(subscriber.Key, out _);
                        _retries.TryAdd(subscriber.Key, retry + 1);

                        if (retry > 5)
                        {
                            _subscribers.TryRemove(subscriber.Key, out _);
                            _retries.TryRemove(subscriber.Key, out _);
                        }
                    }
                }

                if (_subscribers.Any())
                    _logger?.Info($" {_subscribers.Count} subscribers with errors.");

                await Task.Delay(_delayTimeSpan, cancellationToken);
            }
            catch (Exception e)
            {
                _semaphore.Release();
                _logger?.Error(e, e.Message);
            }
        }
    }
}
