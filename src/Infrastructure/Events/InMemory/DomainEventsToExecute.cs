using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SharedKernel.Application.Logging;

namespace SharedKernel.Infrastructure.Events.InMemory
{
    /// <summary>
    /// 
    /// </summary>
    public class DomainEventsToExecute
    {
        private readonly ICustomLogger<DomainEventsToExecute> _logger;
        private readonly TimeSpan _delayTimeSpan;

        /// <summary>
        /// 
        /// </summary>
        private ConcurrentBag<Func<CancellationToken, Task>> _subscribers = new();

        /// <summary>
        /// 
        /// </summary>
        public DomainEventsToExecute(
            ICustomLogger<DomainEventsToExecute> logger,
            TimeSpan delayTimeSpan)
        {
            _logger = logger;
            _delayTimeSpan = delayTimeSpan;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="func"></param>
        public void Add(Func<CancellationToken, Task> func)
        {
            _subscribers.Add(func);
        }

        /// <summary>
        /// 
        /// </summary>
        public async Task ExecuteAll(CancellationToken cancellationToken)
        {
            var semaphore = new SemaphoreSlim(1, 1);
            await semaphore.WaitAsync(cancellationToken);

            try
            {
                var pending = _subscribers.ToList();
                foreach (var func in _subscribers.ToList())
                {
                    try
                    {
                        await func(cancellationToken);
                        pending.Remove(func);
                    }
                    catch (Exception e)
                    {
                        _logger?.Error(e, e.Message);
                    }
                }

                _subscribers = new ConcurrentBag<Func<CancellationToken, Task>>();

                if(pending.Any())
                    _logger?.Info($" {pending.Count} subscribers with errors.");

                foreach (var func in pending)
                    _subscribers.Add(func);

                await Task.Delay(_delayTimeSpan, cancellationToken);
            }
            finally
            {
                semaphore.Release();
            }
        }
    }
}
