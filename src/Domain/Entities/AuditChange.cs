using System;
using SharedKernel.Domain.Aggregates;

namespace SharedKernel.Domain.Entities
{
    public class AuditChange : AggregateRoot<Guid>
    {
        private AuditChange() { }

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

        public string RegistryId { get; private set; }

        public string Table { get; private set; }

        public string Property { get; private set; }

        public string OriginalValue { get; private set; }

        public string CurrentValue { get; private set; }

        public DateTime Date { get; private set; }

        public State State { get; private set; }
    }
}
