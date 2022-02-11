using SharedKernel.Domain.Events;
using System.Collections.Generic;

namespace BankAccounts.Domain.BankAccounts.Events
{
    public class BankAccountCreated : DomainEvent
    {
        public BankAccountCreated(string aggregateId, string eventId = null, string occurredOn = null)
            : base(aggregateId, eventId, occurredOn)
        { }

        public override string GetEventName()
        {
            return "bankAccounts.bankAccounts.bankAccountCreated";
        }

        public override DomainEvent FromPrimitives(string aggregateId, Dictionary<string, string> body, string eventId,
            string occurredOn)
        {
            return new BankAccountCreated(aggregateId, eventId, occurredOn);
        }
    }
}
