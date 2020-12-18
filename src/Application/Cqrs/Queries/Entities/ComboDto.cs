namespace SharedKernel.Application.Cqrs.Queries.Entities
{
    /// <summary>
    /// Combo item
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    public class ComboDto<TKey>
    {
        /// <summary>
        /// Combo value
        /// </summary>
        public TKey Value { get; set; }

        /// <summary>
        /// Combo text
        /// </summary>
        public string Text { get; set; }
    }
}