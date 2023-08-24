namespace SharedKernel.Application.System.Threading
{
    /// <summary>
    /// Executes a for each operation on an <see cref="IEnumerable{TSource}"/> in which iterations may run in parallel
    /// </summary>
    public interface IParallel
    {
        /// <summary>Executes a for each operation on an <see cref="IEnumerable{TSource}"/> in which iterations may run in parallel.</summary>
        /// <typeparam name="TSource">The type of the data in the source.</typeparam>
        /// <param name="source">An enumerable data source.</param>
        /// <param name="body">An asynchronous delegate that is invoked once per element in the data source.</param>
        /// <exception cref="ArgumentNullException">The exception that is thrown when the <paramref name="source"/> argument or <paramref name="body"/> argument is null.</exception>
        /// <returns>A task that represents the entire for each operation.</returns>
        /// <remarks>The operation will execute at most <see cref="Environment.ProcessorCount"/> operations in parallel.</remarks>
        void ForEach<TSource>(IEnumerable<TSource> source, Action<TSource> body);

        /// <summary>Executes a for each operation on an <see cref="IEnumerable{TSource}"/> in which iterations may run in parallel.</summary>
        /// <typeparam name="TSource">The type of the data in the source.</typeparam>
        /// <param name="source">An enumerable data source.</param>
        /// <param name="cancellationToken">A cancellation token that may be used to cancel the for each operation.</param>
        /// <param name="body">An asynchronous delegate that is invoked once per element in the data source.</param>
        /// <exception cref="ArgumentNullException">The exception that is thrown when the <paramref name="source"/> argument or <paramref name="body"/> argument is null.</exception>
        /// <returns>A task that represents the entire for each operation.</returns>
        /// <remarks>The operation will execute at most <see cref="Environment.ProcessorCount"/> operations in parallel.</remarks>
        Task ForEachAsync<TSource>(IEnumerable<TSource> source, CancellationToken cancellationToken,
            Func<TSource, CancellationToken, Task> body);
    }
}