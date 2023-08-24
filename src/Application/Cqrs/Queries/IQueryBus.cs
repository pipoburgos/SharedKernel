namespace SharedKernel.Application.Cqrs.Queries
{
    /// <summary>
    /// Query bus abstraction
    /// </summary>
    public interface IQueryBus
    {
        /// <summary>
        /// Ask a query and return a data transfer object
        /// </summary>
        /// <typeparam name="TResponse"></typeparam>
        /// <param name="query"></param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns></returns>
        Task<TResponse> Ask<TResponse>(IQueryRequest<TResponse> query, CancellationToken cancellationToken);
    }
}
