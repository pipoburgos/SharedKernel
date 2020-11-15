using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SharedKernel.Domain.Aggregates;
using SharedKernel.Domain.Specifications.Common;

namespace SharedKernel.Domain.Repositories
{
    /// <summary>
    ///     Interfaz para el repositorio de lectura
    ///     https://buildplease.com/pages/repositories-dto/
    /// </summary>
    /// <typeparam name="TAggregateRoot">Tipo de datos del repositorio</typeparam>
    internal interface IReadSpecificationRepositoryAsync<TAggregateRoot> where TAggregateRoot : class, IAggregateRoot
    {
        Task<List<TAggregateRoot>> WhereAsync(ISpecification<TAggregateRoot> spec, CancellationToken cancellationToken);

        Task<TAggregateRoot> SingleAsync(ISpecification<TAggregateRoot> spec, CancellationToken cancellationToken);

        Task<TAggregateRoot> SingleOrDefaultAsync(ISpecification<TAggregateRoot> spec, CancellationToken cancellationToken);

        Task<bool> AnyAsync(ISpecification<TAggregateRoot> spec, CancellationToken cancellationToken);
    }
}