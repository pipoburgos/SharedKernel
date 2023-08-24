namespace SharedKernel.Domain.Entities.Paged
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DomainPagedList<T>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="items"></param>
        /// <param name="total"></param>
        /// <param name="totalFiltered"></param>
        public DomainPagedList(IEnumerable<T> items, int total, int totalFiltered)
        {
            Items = items ?? throw new ArgumentNullException(nameof(items));
            Total = total;
            TotalFiltered = totalFiltered;
        }

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<T> Items { get; }

        /// <summary>
        /// 
        /// </summary>
        public int Total { get; }

        /// <summary>
        /// 
        /// </summary>
        public int TotalFiltered { get; }
    }
}
