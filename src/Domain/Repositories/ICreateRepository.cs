using System.Collections.Generic;
using SharedKernel.Domain.Aggregates;

namespace SharedKernel.Domain.Repositories
{
    /// <summary>
    ///     Interfaz para los repositorios de creación
    ///     https://buildplease.com/pages/repositories-dto/
    /// </summary>
    /// <typeparam name="TAggregate">Tipo de datos del repositorio</typeparam>
    internal interface ICreateRepository<in TAggregate> where TAggregate : IAggregateRoot
    {
        /// <summary>
        ///     Inserta un nuevo elemento en el repositorio
        /// </summary>
        /// <param name="aggregate">Elemento a insertar</param>
        void Add(TAggregate aggregate);

        /// <summary>
        ///     Inserta una lista de elementos en el repositorio
        /// </summary>
        /// <param name="aggregates">Elemento a insertar</param>
        void AddRange(IEnumerable<TAggregate> aggregates);
    }
}