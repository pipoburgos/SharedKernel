namespace SharedKernel.Domain.Entities
{
    /// <summary>
    /// 
    /// </summary>
    public class AuditChange : AggregateRoot<Guid>
    {
        private AuditChange() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="registryId"></param>
        /// <param name="table"></param>
        /// <param name="property"></param>
        /// <param name="originalValue"></param>
        /// <param name="currentValue"></param>
        /// <param name="date"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public static AuditChange Create(Guid id, string registryId, string table, string property,
            string originalValue, string currentValue, DateTime date, State state)
        {
            return new AuditChange
            {
                Id = id,
                RegistryId = registryId,
                Table = table,
                Property = property,
                OriginalValue = originalValue,
                CurrentValue = currentValue,
                Date = date,
                State = state
            };
        }

        /// <summary>
        /// 
        /// </summary>
        public string RegistryId { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public string Table { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public string Property { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public string OriginalValue { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public string CurrentValue { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime Date { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public State State { get; private set; }
    }
}
