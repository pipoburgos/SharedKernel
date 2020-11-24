using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SharedKernel.Domain.Aggregates;

namespace SharedKernel.Domain.Repositories
{
    /// <summary>
    ///     Interfaz para los repositorios de actualización
    ///     https://buildplease.com/pages/repositories-dto/
    /// </summary>
    /// <typeparam name="TAggregate">Tipo de datos del repositorio</typeparam>
    public interface IUpdateRepositoryAsync<in TAggregate> where TAggregate : IAggregateRoot
    {
        /// <summary>
        ///     Actualiza los datos de un elemento del repositorio
        /// </summary>
        /// <param name="entity">Elemento con los datos actualizados</param>
        /// <param name="cancellationToken"></param>
        Task UpdateAsync(TAggregate entity, CancellationToken cancellationToken);

        /// <summary>
        ///     Actualiza los datos de varios elementos del repositorio
        /// </summary>
        /// <param name="entities">Elemento con los datos actualizados</param>
        /// <param name="cancellationToken"></param>
        Task UpdateRangeAsync(IEnumerable<TAggregate> entities, CancellationToken cancellationToken);
    }
}