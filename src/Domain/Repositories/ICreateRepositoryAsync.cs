using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SharedKernel.Domain.Aggregates;

namespace SharedKernel.Domain.Repositories
{
    /// <summary>
    ///     Interfaz para los repositorios de creación
    ///     https://buildplease.com/pages/repositories-dto/
    /// </summary>
    /// <typeparam name="TAggregate">Tipo de datos del repositorio</typeparam>
    public interface ICreateRepositoryAsync<in TAggregate> where TAggregate : IAggregateRoot
    {
        /// <summary>
        ///     Inserta un nuevo elemento en el repositorio
        /// </summary>
        /// <param name="aggregate">Elemento a insertar</param>
        /// <param name="cancellationToken"></param>
        Task AddAsync(TAggregate aggregate, CancellationToken cancellationToken);

        /// <summary>
        ///     Inserta una lista de elementos en el repositorio
        /// </summary>
        /// <param name="aggregates">Elemento a insertar</param>
        /// <param name="cancellationToken"></param>
        Task AddRangeAsync(IEnumerable<TAggregate> aggregates, CancellationToken cancellationToken);
    }
}