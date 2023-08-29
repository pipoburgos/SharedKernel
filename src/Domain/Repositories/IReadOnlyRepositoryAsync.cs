namespace SharedKernel.Domain.Repositories
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TAggregate"></typeparam>
    public interface IReadOnlyRepositoryAsync<TAggregate> where TAggregate : IAggregateRoot
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TId"></typeparam>
        /// <param name="key"></param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns></returns>
        Task<TAggregate?> GetByIdReadOnlyAsync<TId>(TId key, CancellationToken cancellationToken);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TId"></typeparam>
        /// <param name="key"></param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns></returns>
        Task<TAggregate?> GetDeleteByIdReadOnlyAsync<TId>(TId key, CancellationToken cancellationToken);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TAggregate"></typeparam>
    /// <typeparam name="TId"></typeparam>
    public interface IReadOnlyRepositoryAsync<TAggregate, in TId> where TAggregate : IAggregateRoot
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns></returns>
        Task<TAggregate?> GetByIdReadOnlyAsync(TId key, CancellationToken cancellationToken);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns></returns>
        Task<TAggregate?> GetDeleteByIdReadOnlyAsync(TId key, CancellationToken cancellationToken);
    }
}