using System.Collections.Generic;
using SharedKernel.Domain.Aggregates;

namespace SharedKernel.Domain.Repositories
{
    /// <summary>
    ///     Interfaz para los repositorios de CRUD genéricos
    ///     https://buildplease.com/pages/repositories-dto/
    /// </summary>
    /// <typeparam name="TAggregate">Tipo de datos del repositorio</typeparam>
    public interface IDeleteRepository<in TAggregate> where TAggregate : IAggregateRoot
    {
        void Remove(TAggregate aggregate);

        void RemoveRange(IEnumerable<TAggregate> aggregate);
    }
}