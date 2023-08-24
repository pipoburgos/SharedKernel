namespace SharedKernel.Domain.Repositories
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TAggregate"></typeparam>
    public interface IUpdateRepositoryAsync<in TAggregate> where TAggregate : IAggregateRoot
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="aggregateRoot"></param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns></returns>
        Task UpdateAsync(TAggregate aggregateRoot, CancellationToken cancellationToken);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aggregates"></param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns></returns>
        Task UpdateRangeAsync(IEnumerable<TAggregate> aggregates, CancellationToken cancellationToken);
    }
}