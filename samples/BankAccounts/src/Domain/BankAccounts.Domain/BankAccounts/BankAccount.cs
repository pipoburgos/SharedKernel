using SharedKernel.Domain.Aggregates;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BankAccounts.Domain.BankAccounts
{
    public class BankAccount : AggregateRoot<Guid>
    {
        private readonly List<Movement> _movements;

        protected BankAccount()
        {
            _movements = new List<Movement>();
        }

        public BankAccount(Guid id, InternationalBankAccountNumber internationalBankAccountNumber, User owner,
            Movement initialMovement) : base(id)
        {
            _movements = new List<Movement>();
            InternationalBankAccountNumber = internationalBankAccountNumber;
            Owner = owner;
            _movements = new List<Movement> { initialMovement };
        }

        public InternationalBankAccountNumber InternationalBankAccountNumber { get; private set; }

        public User Owner { get; private set; }

        public decimal Balance => _movements.Sum(m => m.Amount);

        public IEnumerable<Movement> Movements => _movements.AsReadOnly();
    }
}
