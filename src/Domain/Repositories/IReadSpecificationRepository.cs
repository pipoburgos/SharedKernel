using System.Collections.Generic;
using SharedKernel.Domain.Aggregates;
using SharedKernel.Domain.Specifications.Common;

namespace SharedKernel.Domain.Repositories
{
    public interface IReadSpecificationRepository<TAggregateRoot> where TAggregateRoot : class, IAggregateRoot
    {
        List<TAggregateRoot> Where(ISpecification<TAggregateRoot> spec);

        TAggregateRoot Single(ISpecification<TAggregateRoot> spec);

        TAggregateRoot SingleOrDefault(ISpecification<TAggregateRoot> spec);

        bool Any(ISpecification<TAggregateRoot> spec);
    }
}