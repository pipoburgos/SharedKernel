namespace SharedKernel.Application.Cqrs.Queries.Entities
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Results<T>
    {
        /// <summary>
        /// 
        /// </summary>
        public int Draw { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int RecordsTotal { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int RecordsFiltered { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<T> Data { get; set; }
    }
}
