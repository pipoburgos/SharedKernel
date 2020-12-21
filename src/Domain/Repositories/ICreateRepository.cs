using System.Collections.Generic;
using SharedKernel.Domain.Aggregates;

namespace SharedKernel.Domain.Repositories
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TAggregate"></typeparam>
    public interface ICreateRepository<in TAggregate> where TAggregate : IAggregateRoot
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="aggregate"></param>
        void Add(TAggregate aggregate);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aggregates"></param>
        void AddRange(IEnumerable<TAggregate> aggregates);
    }
}