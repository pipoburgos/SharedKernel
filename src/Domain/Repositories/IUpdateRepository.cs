using System.Collections.Generic;
using SharedKernel.Domain.Aggregates;

namespace SharedKernel.Domain.Repositories
{
    /// <summary>
    ///     Interfaz para los repositorios de actualización
    ///     https://buildplease.com/pages/repositories-dto/
    /// </summary>
    /// <typeparam name="TAggregate">Tipo de datos del repositorio</typeparam>
    public interface IUpdateRepository<in TAggregate> where TAggregate : IAggregateRoot
    {
        /// <summary>
        ///     Actualiza los datos de un elemento del repositorio
        /// </summary>
        /// <param name="aggregate">Elemento con los datos actualizados</param>
        void Update(TAggregate aggregate);

        /// <summary>
        ///     Actualiza los datos de varios elementos del repositorio
        /// </summary>
        /// <param name="aggregates">Elemento con los datos actualizados</param>
        void UpdateRange(IEnumerable<TAggregate> aggregates);
    }
}