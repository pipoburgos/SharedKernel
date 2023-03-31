namespace BankAccounts.Domain.BankAccounts
{
    internal class Movement : Entity<Guid>
    {
        protected Movement() { }

        public Movement(Guid id, string concept, decimal amount, DateTime date) : base(id)
        {
            Concept = concept;
            Amount = amount;
            Date = date;
        }

        public string Concept { get; private set; }

        public decimal Amount { get; private set; }

        public DateTime Date { get; private set; }

        public Guid BankAccountId { get; private set; }
    }
}
