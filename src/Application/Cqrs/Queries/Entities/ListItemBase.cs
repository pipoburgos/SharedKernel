namespace SharedKernel.Application.Cqrs.Queries.Entities
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ListItemBase<T>
    {
        /// <summary>
        /// 
        /// </summary>
        public bool Deleted { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public T Id { get; set; }
    }
}
