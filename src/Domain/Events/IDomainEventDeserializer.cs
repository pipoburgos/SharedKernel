namespace SharedKernel.Domain.Events
{
    public interface IDomainEventDeserializer
    {
        DomainEvent Deserialize(string domainEvent);
    }
}