using SharedKernel.Application.System.Threading;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
#if !NET6_0_OR_GREATER
using System.Collections.Concurrent;
using System.Linq;
#endif

namespace SharedKernel.Infrastructure.System.Threading
{
    /// <summary>
    /// Executes a for each operation on an <see cref="IEnumerable{TSource}"/> in which iterations may run in parallel
    /// </summary>
    public class Parallel : IParallel
    {
        /// <summary>Executes a for each operation on an <see cref="IEnumerable{TSource}"/> in which iterations may run in parallel.</summary>
        /// <typeparam name="TSource">The type of the data in the source.</typeparam>
        /// <param name="source">An enumerable data source.</param>
        /// <param name="body">An asynchronous delegate that is invoked once per element in the data source.</param>
        /// <exception cref="ArgumentNullException">The exception that is thrown when the <paramref name="source"/> argument or <paramref name="body"/> argument is null.</exception>
        /// <returns>A task that represents the entire for each operation.</returns>
        /// <remarks>The operation will execute at most <see cref="Environment.ProcessorCount"/> operations in parallel.</remarks>
        public void ForEach<TSource>(IEnumerable<TSource> source, Action<TSource> body)
        {
            global::System.Threading.Tasks.Parallel.ForEach(source, body);
        }

#if NET6_0_OR_GREATER
        /// <summary>Executes a for each operation on an <see cref="IEnumerable{TSource}"/> in which iterations may run in parallel.</summary>
        /// <typeparam name="TSource">The type of the data in the source.</typeparam>
        /// <param name="source">An enumerable data source.</param>
        /// <param name="cancellationToken">A cancellation token that may be used to cancel the for each operation.</param>
        /// <param name="body">An asynchronous delegate that is invoked once per element in the data source.</param>
        /// <exception cref="ArgumentNullException">The exception that is thrown when the <paramref name="source"/> argument or <paramref name="body"/> argument is null.</exception>
        /// <returns>A task that represents the entire for each operation.</returns>
        /// <remarks>The operation will execute at most <see cref="Environment.ProcessorCount"/> operations in parallel.</remarks>
        public Task ForEachAsync<TSource>(IEnumerable<TSource> source, CancellationToken cancellationToken,
            Func<TSource, CancellationToken, Task> body)
        {
            return global::System.Threading.Tasks.Parallel.ForEachAsync(source, cancellationToken,
                async (source1, token) => await body(source1, token));

        }
#else


        /// <summary>Executes a for each operation on an <see cref="IEnumerable{TSource}"/> in which iterations may run in parallel.</summary>
        /// <typeparam name="TSource">The type of the data in the source.</typeparam>
        /// <param name="source">An enumerable data source.</param>
        /// <param name="cancellationToken">A cancellation token that may be used to cancel the for each operation.</param>
        /// <param name="body">An asynchronous delegate that is invoked once per element in the data source.</param>
        /// <exception cref="ArgumentNullException">The exception that is thrown when the <paramref name="source"/> argument or <paramref name="body"/> argument is null.</exception>
        /// <returns>A task that represents the entire for each operation.</returns>
        /// <remarks>The operation will execute at most <see cref="Environment.ProcessorCount"/> operations in parallel.</remarks>
        public Task ForEachAsync<TSource>(IEnumerable<TSource> source, CancellationToken cancellationToken,
            Func<TSource, CancellationToken, Task> body)
        {
            async Task AwaitPartition(IEnumerator<TSource> partition)
            {
                using (partition)
                {
                    while (partition.MoveNext())
                    {
                        await body(partition.Current, cancellationToken);
                    }
                }
            }

            return Task.WhenAll(
                Partitioner
                    .Create(source)
                    .GetPartitions(Environment.ProcessorCount)
                    .AsParallel()
                    .Select(AwaitPartition)
            );
        }
#endif
    }
}