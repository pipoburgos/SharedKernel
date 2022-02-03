using SharedKernel.Domain.Entities;
using System;

namespace BankAccounts.Domain.BankAccounts
{
    public class Movement : Entity<Guid>
    {
        protected Movement() { }

        public Movement(Guid id, string concept, decimal quantity, DateTime date) : base(id)
        {
            Concept = concept;
            Quantity = quantity;
            Date = date;
        }

        public string Concept { get; private set; }
        public decimal Quantity { get; private set; }
        public DateTime Date { get; private set; }
    }
}
