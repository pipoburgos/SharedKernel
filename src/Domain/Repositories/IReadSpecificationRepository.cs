using System.Collections.Generic;
using SharedKernel.Domain.Aggregates;
using SharedKernel.Domain.Specifications.Common;

namespace SharedKernel.Domain.Repositories
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TAggregateRoot"></typeparam>
    public interface IReadSpecificationRepository<TAggregateRoot> where TAggregateRoot : class, IAggregateRoot
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="spec"></param>
        /// <returns></returns>
        List<TAggregateRoot> Where(ISpecification<TAggregateRoot> spec);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="spec"></param>
        /// <returns></returns>
        TAggregateRoot Single(ISpecification<TAggregateRoot> spec);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="spec"></param>
        /// <returns></returns>
        TAggregateRoot SingleOrDefault(ISpecification<TAggregateRoot> spec);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="spec"></param>
        /// <returns></returns>
        bool Any(ISpecification<TAggregateRoot> spec);
    }
}