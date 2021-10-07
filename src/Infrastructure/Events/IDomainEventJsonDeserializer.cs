using SharedKernel.Domain.Events;

namespace SharedKernel.Infrastructure.Events
{
    /// <summary>
    /// Domain event deserializer
    /// </summary>
    public interface IDomainEventJsonDeserializer
    {
        /// <summary>
        /// Domain event deserializer
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        DomainEvent Deserialize(string body);
    }
}