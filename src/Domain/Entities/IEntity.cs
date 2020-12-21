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
    /// <typeparam name="TKey"></typeparam>
    public interface IEntity<out TKey> : IEntity
    {
        /// <summary>
        /// The identifier of de entity
        /// </summary>
        TKey Id { get; }
    }
}