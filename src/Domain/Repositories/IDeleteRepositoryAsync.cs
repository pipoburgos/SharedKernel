using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SharedKernel.Domain.Aggregates;

namespace SharedKernel.Domain.Repositories
{
    /// <summary>
    ///     Interfaz para los repositorios de CRUD genéricos
    ///     https://buildplease.com/pages/repositories-dto/
    /// </summary>
    /// <typeparam name="TAggregate">Tipo de datos del repositorio</typeparam>
    internal interface IDeleteRepositoryAsync<in TAggregate> where TAggregate : IAggregateRoot
    {
        Task RemoveAsync(TAggregate aggregate, CancellationToken cancellationToken);

        Task RemoveRangeAsync(IEnumerable<TAggregate> aggregate, CancellationToken cancellationToken);
    }
}