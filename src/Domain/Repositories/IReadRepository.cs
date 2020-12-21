using SharedKernel.Domain.Aggregates;

namespace SharedKernel.Domain.Repositories
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TAggregate"></typeparam>
    public interface IReadRepository<out TAggregate> where TAggregate : IAggregateRoot
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        TAggregate GetById<TKey>(TKey key);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        bool Any();

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        bool Any<TKey>(TKey key);
    }
}