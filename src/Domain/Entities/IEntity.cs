namespace SharedKernel.Domain.Entities
{
    /// <summary>
    /// 
    /// </summary>
    public interface IEntity
    {
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TId"></typeparam>
    public interface IEntity<out TId> : IEntity
    {
        /// <summary>
        /// The identifier of de entity
        /// </summary>
        TId Id { get; }
    }
}