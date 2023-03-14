using SharedKernel.Domain.Aggregates;
using System.Threading;
using System.Threading.Tasks;

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
        /// <typeparam name="TKey"></typeparam>
        /// <param name="key"></param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns></returns>
        Task<TAggregate> GetByIdReadOnlyAsync<TKey>(TKey key, CancellationToken cancellationToken);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="key"></param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns></returns>
        Task<TAggregate> GetDeleteByIdReadOnlyAsync<TKey>(TKey key, CancellationToken cancellationToken);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TAggregate"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    public interface IReadOnlyRepositoryAsync<TAggregate, in TKey> where TAggregate : IAggregateRoot
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns></returns>
        Task<TAggregate> GetByIdReadOnlyAsync(TKey key, CancellationToken cancellationToken);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns></returns>
        Task<TAggregate> GetDeleteByIdReadOnlyAsync(TKey key, CancellationToken cancellationToken);
    }
}